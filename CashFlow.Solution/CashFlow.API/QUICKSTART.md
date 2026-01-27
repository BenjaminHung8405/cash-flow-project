# Quick Start Guide - Multi-Tenant Financial Application

## 🚀 Getting Started

### 1. Database Setup

Create the initial database migration:

```powershell
cd C:\Users\ACER\source\repos\cash-flow-project\CashFlow.Solution
dotnet ef migrations add InitialCreate -p CashFlow.API
dotnet ef database update -p CashFlow.API
```

### 2. Configuration

Update `appsettings.json` with your database connection string and JWT secret:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=CashFlowDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "your-secret-key-minimum-32-characters-long!!",
    "Issuer": "CashFlowAPI",
    "Audience": "CashFlowWinForms"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  }
}
```

### 3. Run the Application

```powershell
dotnet run --project CashFlow.API
```

The API will be available at `https://localhost:5001` (or the configured port).

## 🔐 Authentication Flow

### 1. Login (Create User & Get Token)

Use the AuthController to authenticate:

```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "john.doe",
  "password": "password123"
}
```

Response includes JWT token with:
- `tenantId` claim
- `userId` claim
- `role` claim(s)

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "userId": "user123",
  "tenantId": "550e8400-e29b-41d4-a716-446655440000"
}
```

### 2. Use Token for API Calls

Include the JWT token in the Authorization header:

```http
GET /api/funds
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## 📚 API Examples

### Get All Funds (Tenant-Isolated)

```http
GET /api/funds
Authorization: Bearer [token]
```

Response:
```json
[
  {
    "id": "550e8400-e29b-41d4-a716-446655440002",
    "tenantId": "550e8400-e29b-41d4-a716-446655440000",
    "fundName": "Main Cash",
    "fundType": "CASH",
    "accountNumber": null,
    "currentBalance": 50000,
    "isActive": true,
    "createdAt": "2024-01-15T10:30:00Z",
    "createdBy": "admin123"
  }
]
```

### Create Fund (Auto-TenantId Injection)

```http
POST /api/funds
Authorization: Bearer [token]
Content-Type: application/json

{
  "fundName": "Vietcombank Account",
  "fundType": "BANK",
  "accountNumber": "0123456789",
  "initialBalance": 100000
}
```

Response:
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440003",
  "tenantId": "550e8400-e29b-41d4-a716-446655440000",
  "fundName": "Vietcombank Account",
  "fundType": "BANK",
  "accountNumber": "0123456789",
  "currentBalance": 100000,
  "isActive": true,
  "createdAt": "2024-01-15T10:35:00Z",
  "createdBy": "user123"
}
```

### Update Fund

```http
PUT /api/funds/550e8400-e29b-41d4-a716-446655440003
Authorization: Bearer [token]
Content-Type: application/json

{
  "fundName": "Vietcombank Updated",
  "currentBalance": 105000
}
```

### Get Fund Audit Trail

```http
GET /api/funds/550e8400-e29b-41d4-a716-446655440003/audit-trail
Authorization: Bearer [token]
```

Response:
```json
[
  {
    "id": "550e8400-e29b-41d4-a716-446655440004",
    "tenantId": "550e8400-e29b-41d4-a716-446655440000",
    "userId": "user123",
    "action": "INSERT",
    "tableName": "Fund",
    "recordId": "550e8400-e29b-41d4-a716-446655440003",
    "oldValues": null,
    "newValues": "{\"Id\":\"550e8400-e29b-41d4-a716-446655440003\",\"TenantId\":\"550e8400-e29b-41d4-a716-446655440000\",\"FundName\":\"Vietcombank Account\",\"FundType\":\"BANK\",\"CurrentBalance\":100000}",
    "timestamp": "2024-01-15T10:35:00Z"
  },
  {
    "id": "550e8400-e29b-41d4-a716-446655440005",
    "tenantId": "550e8400-e29b-41d4-a716-446655440000",
    "userId": "user123",
    "action": "UPDATE",
    "tableName": "Fund",
    "recordId": "550e8400-e29b-41d4-a716-446655440003",
    "oldValues": "{\"FundName\":\"Vietcombank Account\",\"CurrentBalance\":100000}",
    "newValues": "{\"FundName\":\"Vietcombank Updated\",\"CurrentBalance\":105000}",
    "timestamp": "2024-01-15T10:40:00Z"
  }
]
```

## 🏗️ Architecture Overview

### Key Components

1. **Domain Layer** (`CashFlow.API/Domain/Entities/`)
   - Entity models inheriting from `BaseTenantEntity`
   - Database relationships and constraints

2. **Infrastructure Layer** (`CashFlow.API/Infrastructure/`)
   - `TenantService` - Multi-tenancy resolution
   - `JwtTokenGenerator` - Token creation
   - Service collection extensions

3. **Data Layer** (`CashFlow.API/Data/`)
   - `AppDbContext` - EF Core context
   - Global query filters for automatic tenant isolation
   - Automatic audit trail generation

4. **Application Layer** (`CashFlow.API/Controllers/`)
   - REST API endpoints
   - Request/response DTOs

### Multi-Tenancy Flow

```
1. User logs in with credentials
   ↓
2. AuthController generates JWT with tenantId claim
   ↓
3. Client includes JWT in Authorization header
   ↓
4. TenantService extracts tenantId from JWT
   ↓
5. Global query filters use tenantId for automatic filtering
   ↓
6. Entity created with tenantId automatically injected
   ↓
7. AuditLog created with change tracking
```

## 🔍 Debugging Tips

### View Generated SQL

Enable SQL logging in `Program.cs`:

```csharp
options.LogTo(Console.WriteLine);
options.EnableSensitiveDataLogging();
```

### Check Audit Logs

Query audit logs from the database:

```sql
SELECT * FROM AuditLogs 
WHERE TableName = 'Fund' 
ORDER BY Timestamp DESC
```

### Verify Tenant Isolation

Query multiple funds from different tenants:

```sql
SELECT TenantId, COUNT(*) as FundCount 
FROM Funds 
GROUP BY TenantId
```

## 🧪 Testing

### Unit Test Example

```csharp
[TestClass]
public class MultiTenancyTests
{
    [TestMethod]
    public async Task CreateFund_AutoInjectsCurrentTenantId()
    {
        // Arrange
        var mockTenantService = new Mock<ITenantService>();
        mockTenantService.Setup(x => x.GetCurrentTenantId())
            .Returns(Guid.Parse("550e8400-e29b-41d4-a716-446655440000"));

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        var context = new AppDbContext(options, mockTenantService.Object);

        // Act
        var fund = new Fund { FundName = "Test Fund" };
        context.Funds.Add(fund);
        await context.SaveChangesAsync();

        // Assert
        Assert.AreEqual("550e8400-e29b-41d4-a716-446655440000", fund.TenantId);
    }
}
```

## 📖 Additional Resources

- [MULTI_TENANCY_ARCHITECTURE.md](./MULTI_TENANCY_ARCHITECTURE.md) - Detailed architecture guide
- [IMPLEMENTATION_COMPLETE.md](./IMPLEMENTATION_COMPLETE.md) - Implementation summary
- [Entity Framework Core Docs](https://learn.microsoft.com/en-us/ef/core/)
- [JWT Authentication](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/jwt-authn)

## ⚠️ Common Issues

### Issue: "TenantId claim not found in JWT token"

**Solution**: Ensure your JWT token includes the `tenantId` claim.

```csharp
var claims = new List<Claim>
{
    new Claim(ClaimTypes.NameIdentifier, userId),
    new Claim("tenantId", tenantId.ToString())  // ← Required
};
```

### Issue: "Query filters used in Database context initialization"

**Solution**: This is expected. Global query filters are applied during context initialization.

### Issue: "No rows returned when querying funds"

**Solution**: Verify that:
1. Funds exist in the database with your tenant ID
2. JWT token contains the correct `tenantId` claim
3. Query filter is not too restrictive

## 🎯 Next Steps

1. ✅ Set up database
2. ✅ Configure appsettings.json
3. ✅ Run migrations
4. ✅ Start the API
5. Create AuthController endpoints for user login
6. Implement role-based authorization
7. Add more domain entities (Transactions, Users, etc.)
8. Create comprehensive unit tests
9. Set up CI/CD pipeline
10. Deploy to production

---

**Ready to build your multi-tenant SaaS!** 🎉
