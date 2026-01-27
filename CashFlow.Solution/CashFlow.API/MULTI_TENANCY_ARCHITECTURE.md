# Multi-Tenant Financial Application Architecture

## Overview

This project implements a **Shared Database, Shared Schema** multi-tenancy architecture for a SaaS financial application using .NET 8, C#, and Entity Framework Core with SQL Server.

## Architecture Components

### 1. Domain Entities (`CashFlow.API/Domain/Entities/`)

All tenant-scoped entities inherit from `BaseTenantEntity`:

- **BaseTenantEntity**: Abstract base class with `Id`, `TenantId`, `CreatedAt`, `CreatedBy`
- **Tenant**: Represents organizations/companies in the system
- **User**: Represents users within a tenant
- **Role**: Represents roles within a tenant
- **Permission**: System-wide permissions
- **RolePermission**: Many-to-many junction for Role-Permission relationship
- **Fund**: Represents cash/bank accounts (account types)
- **Transaction**: Represents financial transactions (vouchers/receipts/payments)
- **AuditLog**: Tracks all changes across entities

### 2. Multi-Tenancy Resolution (`CashFlow.API/Infrastructure/Services/`)

**ITenantService**: Interface for tenant resolution
- `GetCurrentTenantId()`: Extracts TenantId from JWT claims
- `GetCurrentUserId()`: Extracts UserId from JWT claims

**TenantService**: Implementation using `IHttpContextAccessor`
- Reads JWT token claims from the HTTP context
- Looks for `tenantId` claim in the token
- Falls back to `ClaimTypes.GroupSid` if not found

### 3. Database Context (`CashFlow.API/Data/AppDbContext.cs`)

**Key Features:**

#### Multi-Tenancy with Global Query Filters
```csharp
modelBuilder.Entity<User>()
    .HasQueryFilter(x => x.TenantId == currentTenantId);
```
- Automatically filters all queries by the current tenant
- Prevents data leakage between tenants
- Applied to: User, Role, Fund, Transaction, AuditLog

#### Automatic Audit Trail
- Overrides `SaveChanges()` and `SaveChangesAsync()`
- Processes `ChangeTracker.Entries()` to:
  - Inject `TenantId` for new `BaseTenantEntity` descendants
  - Create `AuditLog` entries for INSERT, UPDATE, DELETE operations
  - Serialize old and new values to JSON

#### Configuration
- Composite key for `RolePermission`
- Foreign key relationships with cascade rules
- Seed data for default permissions

### 4. Security & JWT (`CashFlow.API/Infrastructure/Security/`)

**JwtTokenGenerator**: Generates JWT tokens with tenant claims
- Includes `tenantId` claim for multi-tenancy
- Includes `userId` claim for user identification
- Includes role claims for authorization
- Configurable expiration

### 5. Extension Methods (`CashFlow.API/Infrastructure/Extensions/`)

**ServiceCollectionExtensions**: Fluent dependency injection configuration
- `AddMultiTenancy()`: Registers tenant resolution services
- `AddMultiTenantDbContext()`: Configures AppDbContext
- `AddJwtAuthentication()`: Sets up JWT authentication
- `AddCorsConfiguration()`: Configures CORS policies

### 6. Program.cs Configuration

Clean, organized dependency injection setup:

```csharp
builder.Services.AddMultiTenancy();
builder.Services.AddMultiTenantDbContext(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCorsConfiguration();
```

## Database Schema

### Multi-Tenancy Approach: Shared Database, Shared Schema

```
Tenants
├── Users (TenantId)
├── Roles (TenantId)
│   └── RolePermissions
│       └── Permissions (Shared)
├── Funds (TenantId)
│   └── Transactions (TenantId)
└── AuditLogs (TenantId)
```

### Key Characteristics

- **Tenants**: Not filtered by query filters (Tenant table is system-wide)
- **Permission**: Shared across all tenants (no TenantId column)
- **Other entities**: All scoped to their tenant via TenantId + Global Query Filters

## JWT Token Claims

The JWT token should include these claims for proper operation:

```json
{
  "tenantId": "550e8400-e29b-41d4-a716-446655440000",
  "userId": "user123",
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier": "user123",
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": "john.doe",
  "role": "Administrator"
}
```

### Usage in Code

```csharp
// In your controller
[Authorize]
public class FundsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ITenantService _tenantService;

    public FundsController(AppDbContext context, ITenantService tenantService)
    {
        _context = context;
        _tenantService = tenantService;
    }

    [HttpGet]
    public async Task<IActionResult> GetFunds()
    {
        var currentTenantId = _tenantService.GetCurrentTenantId();
        var funds = await _context.Funds.ToListAsync();
        // funds are automatically filtered by currentTenantId
        return Ok(funds);
    }

    [HttpPost]
    public async Task<IActionResult> CreateFund([FromBody] CreateFundDto dto)
    {
        var currentUserId = _tenantService.GetCurrentUserId();
        
        var fund = new Fund
        {
            FundName = dto.FundName,
            FundType = dto.FundType,
            CurrentBalance = dto.InitialBalance
            // TenantId will be automatically injected
        };

        _context.Funds.Add(fund);
        await _context.SaveChangesAsync();
        // Audit log created automatically

        return Created(nameof(CreateFund), fund);
    }
}
```

## Audit Trail Example

When a User is modified:

```
AuditLog Entry:
{
  "Id": "...",
  "TenantId": "550e8400-e29b-41d4-a716-446655440000",
  "UserId": "admin123",
  "Action": "UPDATE",
  "TableName": "User",
  "RecordId": "user456",
  "OldValues": "{\"IsActive\":true,\"LastLoginAt\":\"2024-01-15T10:30:00Z\"}",
  "NewValues": "{\"IsActive\":false,\"LastLoginAt\":\"2024-01-15T14:45:00Z\"}",
  "Timestamp": "2024-01-15T14:45:01Z"
}
```

## Configuration (appsettings.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=CashFlowDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "your-secret-key-min-32-characters",
    "Issuer": "CashFlowAPI",
    "Audience": "CashFlowWinForms"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

## Security Considerations

1. **JWT Token Validation**
   - Validates signature, issuer, audience, and expiration
   - Implements 0 clock skew for strict time validation
   - Custom error handling for expired tokens

2. **Multi-Tenancy Enforcement**
   - Global query filters prevent accidental data leakage
   - TenantId automatically injected for new entities
   - Query filters applied to all tenant-scoped entity queries

3. **Audit Trail**
   - Automatic logging of all changes
   - Captures who made changes and when
   - JSON serialization of modified values

4. **CORS Configuration**
   - Development: Allow all origins
   - Production: Restrict to specific origins

## Database Migrations

Create initial migration:

```bash
dotnet ef migrations add InitialCreate -p CashFlow.API
```

Apply migrations:

```bash
dotnet ef database update -p CashFlow.API
```

## File Structure

```
CashFlow.API/
├── Domain/
│   └── Entities/
│       ├── BaseTenantEntity.cs
│       ├── Tenant.cs
│       ├── User.cs
│       ├── Role.cs
│       ├── Permission.cs
│       ├── RolePermission.cs
│       ├── Fund.cs
│       ├── Transaction.cs
│       └── AuditLog.cs
├── Data/
│   └── AppDbContext.cs
├── Infrastructure/
│   ├── Services/
│   │   ├── ITenantService.cs
│   │   └── TenantService.cs
│   ├── Security/
│   │   └── JwtTokenGenerator.cs
│   └── Extensions/
│       └── ServiceCollectionExtensions.cs
├── Controllers/
│   └── [Your API controllers]
├── appsettings.json
└── Program.cs
```

## Clean Architecture Principles

- **Domain Layer**: Entity models with no external dependencies
- **Infrastructure Layer**: Data access, services, and external integrations
- **Application Layer**: Controllers, DTOs, business logic
- **Async/Await**: All database operations use async/await pattern
- **Dependency Injection**: All dependencies injected via constructor

## Next Steps

1. Create Entity Framework Core migrations
2. Implement repository pattern (optional, for data access abstraction)
3. Create DTOs and mapping profiles
4. Implement authentication endpoints
5. Create business logic services
6. Add unit tests for multi-tenancy logic
7. Configure for production deployment

## Additional Resources

- [Entity Framework Core Documentation](https://learn.microsoft.com/en-us/ef/core/)
- [ASP.NET Core Authentication & Authorization](https://learn.microsoft.com/en-us/aspnet/core/security/)
- [JWT Bearer in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/jwt-authn)
- [Multi-Tenancy Patterns](https://learn.microsoft.com/en-us/dotnet/architecture/multitenant-web-app/)
