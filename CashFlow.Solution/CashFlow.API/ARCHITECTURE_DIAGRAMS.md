# 📊 Multi-Tenant Architecture - Visual Reference

## System Architecture Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                    CLIENT LAYER                            │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐     │
│  │  WinForms    │  │  Web App     │  │  Mobile App  │     │
│  └──────┬───────┘  └──────┬───────┘  └──────┬───────┘     │
└─────────┼─────────────────┼──────────────────┼─────────────┘
          │                 │                  │
          │  JWT Token      │  JWT Token       │  JWT Token
          │  (with tenantId)│                  │
          └─────────────────┼──────────────────┘
                            │
        ┌───────────────────▼────────────────────┐
        │   ASP.NET CORE 10 - API LAYER          │
        │  ┌─────────────────────────────────────┤
        │  │  AUTHENTICATION MIDDLEWARE          │
        │  │  ├─ JWT Validation                  │
        │  │  ├─ Claim Extraction                │
        │  │  └─ TenantId Resolution             │
        │  └─────────────────────────────────────┤
        │                                        │
        │  ┌─────────────────────────────────────┤
        │  │  CONTROLLERS                        │
        │  │  ├─ FundsController                 │
        │  │  ├─ UsersController (TODO)          │
        │  │  ├─ TransactionsController (TODO)   │
        │  │  └─ ReportsController (TODO)        │
        │  └─────────────────────────────────────┤
        │                                        │
        │  ┌─────────────────────────────────────┤
        │  │  INFRASTRUCTURE LAYER               │
        │  │  ├─ TenantService                   │
        │  │  │  ├─ GetCurrentTenantId()         │
        │  │  │  └─ GetCurrentUserId()           │
        │  │  │                                  │
        │  │  ├─ JwtTokenGenerator               │
        │  │  │  └─ GenerateToken()              │
        │  │  │                                  │
        │  │  └─ ServiceCollectionExtensions     │
        │  │     ├─ AddMultiTenancy()            │
        │  │     ├─ AddMultiTenantDbContext()    │
        │  │     ├─ AddJwtAuthentication()       │
        │  │     └─ AddCorsConfiguration()       │
        │  └─────────────────────────────────────┤
        │                                        │
        │  ┌─────────────────────────────────────┤
        │  │  DATA ACCESS LAYER                  │
        │  │  ┌─────────────────────────────────-┤
        │  │  │ AppDbContext                     │
        │  │  │ ├─ DbSet<Tenant>                 │
        │  │  │ ├─ DbSet<User>                   │
        │  │  │ ├─ DbSet<Role>                   │
        │  │  │ ├─ DbSet<Permission>             │
        │  │  │ ├─ DbSet<Fund>                   │
        │  │  │ ├─ DbSet<Transaction>            │
        │  │  │ └─ DbSet<AuditLog>               │
        │  │  │                                  │
        │  │  └─ FEATURES:                       │
        │  │     ├─ Global Query Filters         │
        │  │     │  └─ Automatic TenantId filter │
        │  │     │                               │
        │  │     ├─ Change Tracking              │
        │  │     │  ├─ Auto TenantId Injection   │
        │  │     │  ├─ Auto Timestamp            │
        │  │     │  └─ Auto UserId               │
        │  │     │                               │
        │  │     └─ Audit Trail                  │
        │  │        └─ Auto AuditLog Creation    │
        │  └─────────────────────────────────────┤
        │                                        │
        └────────────────┬───────────────────────┘
                         │
         ┌───────────────▼───────────────┐
         │   ENTITY FRAMEWORK CORE 10    │
         │   ┌───────────────────────────┤
         │   │ Migration System          │
         │   │ Relationship Mapping      │
         │   │ Query Translation         │
         │   │ Change Tracking           │
         │   └───────────────────────────┤
         └───────────────┬───────────────┘
                         │
         ┌───────────────▼───────────────┐
         │   SQL SERVER DATABASE         │
         │   ┌───────────────────────────┤
         │   │ Tenant Table              │
         │   │ User Table (TenantId)     │
         │   │ Role Table (TenantId)     │
         │   │ Permission Table (Shared) │
         │   │ Fund Table (TenantId)     │
         │   │ Transaction Table (TenantId)
         │   │ AuditLog Table (TenantId) │
         │   └───────────────────────────┤
         └───────────────────────────────┘
```

---

## Request/Response Flow

```
┌─────────────────────────────────────────────────────────────┐
│  1. CLIENT SENDS REQUEST WITH JWT TOKEN                     │
│                                                             │
│  GET /api/funds                                             │
│  Authorization: Bearer eyJhbGc... (contains tenantId claim) │
└────────────────────┬────────────────────────────────────────┘
                     │
        ┌────────────▼────────────┐
        │  2. AUTHENTICATION      │
        │     MIDDLEWARE          │
        │  ✓ Validate JWT         │
        │  ✓ Extract Claims       │
        └────────────┬────────────┘
                     │
        ┌────────────▼────────────┐
        │  3. TENANT SERVICE      │
        │  ✓ Read tenantId claim  │
        │  ✓ Read userId claim    │
        │  ✓ Store in context     │
        └────────────┬────────────┘
                     │
        ┌────────────▼────────────┐
        │  4. ROUTE TO            │
        │     CONTROLLER          │
        │  FundsController        │
        │  GetFunds()             │
        └────────────┬────────────┘
                     │
        ┌────────────▼────────────────────────┐
        │  5. BUILD QUERY                     │
        │  var funds = _context.Funds         │
        │    .Where(f => f.IsActive)          │
        │    .ToListAsync()                   │
        └────────────┬────────────────────────┘
                     │
        ┌────────────▼────────────────────────────────┐
        │  6. APPLY GLOBAL QUERY FILTER               │
        │  WHERE Funds.TenantId = {currentTenantId}   │
        │  (Automatic - no code needed!)              │
        └────────────┬────────────────────────────────┘
                     │
        ┌────────────▼───────────────────────┐
        │  7. EXECUTE AT DATABASE            │
        │  SELECT * FROM Funds               │
        │  WHERE TenantId = @tenantId        │
        │  AND IsActive = 1                  │
        └────────────┬───────────────────────┘
                     │
        ┌────────────▼────────────┐
        │  8. RETURN RESULTS      │
        │  Only this tenant's     │
        │  active funds           │
        └────────────┬────────────┘
                     │
        ┌────────────▼──────────────────────────┐
        │  9. RESPONSE SENT TO CLIENT           │
        │  HTTP 200 OK                          │
        │  Content-Type: application/json       │
        │  [                                    │
        │    {                                  │
        │      "id": "550e8400...",             │
        │      "tenantId": "550e8400...",       │
        │      "fundName": "Main Cash",         │
        │      ...                              │
        │    }                                  │
        │  ]                                    │
        └───────────────────────────────────────┘
```

---

## Data Modification Flow

```
┌─────────────────────────────────────────────────────────────┐
│  1. POST /api/funds - CREATE REQUEST                        │
│                                                             |
│  {                                                          │
│    "fundName": "Vietcombank",                               │
│    "fundType": "BANK",                                      │
│    "initialBalance": 100000                                 │
│  }                                                          │
└────────────────────┬────────────────────────────────────────┘
                     │
        ┌────────────▼────────────┐
        │  2. CONTROLLER RECEIVES │
        │     Request             │
        │  Creates Fund entity    │
        │  (no TenantId set yet)  │
        └────────────┬────────────┘
                     │
        ┌────────────▼──────────────────────────┐
        │  3. ADD TO CONTEXT                    │
        │  _context.Funds.Add(fund)             │
        │  (Fund in Added state)                │
        └────────────┬──────────────────────────┘
                     │
        ┌────────────▼──────────────────────────┐
        │  4. CALL SaveChangesAsync()           │
        │                                       │
        │  ┌────────────────────────────────┐   │
        │  │ ProcessChangeTracker()         │   │
        │  │ ├─ Get currentTenantId from    │   │
        │  │ │  TenantService               │   │
        │  │ │                              │   │
        │  │ │  tenantId = "550e8400..."    │   │
        │  │ │                              │   │
        │  │ ├─ Inject into Fund entity:    │   │
        │  │ │  fund.TenantId = tenantId    │   │
        │  │ │  fund.CreatedAt = DateTime   │   │
        │  │ │  fund.CreatedBy = userId     │   │
        │  │ │                              │   │
        │  │ ├─ Create AuditLog:            │   │
        │  │ │  new AuditLog {              │   │
        │  │ │    Action: "INSERT",         │   │
        │  │ │    TableName: "Fund",        │   │
        │  │ │    TenantId: tenantId,       │   │
        │  │ │    UserId: userId,           │   │
        │  │ │    NewValues: {...json...}   │   │
        │  │ │  }                           │   │
        │  │ │                              │   │
        │  │ └─ Add to AuditLogs            │   │
        │  └────────────────────────────────┘   │
        └────────────┬──────────────────────────┘
                     │
        ┌────────────▼──────────────────────────┐
        │  5. SAVE TO DATABASE                  │
        │  INSERT Funds (...)                   │
        │  INSERT AuditLogs (...)               │
        │  (Transaction ensures atomicity)      │
        └────────────┬──────────────────────────┘
                     │
        ┌────────────▼──────────────────────────┐
        │  6. RETURN HTTP 201 CREATED           │
        │  Location: /api/funds/{id}            │
        │  Body: Created Fund object            │
        └───────────────────────────────────────┘
```

---

## Entity Relationships Diagram

```
                    Tenant
                   (System)
                      │
        ┌─────────────┼─────────────┐
        │             │             │
        ↓             ↓             ↓
    User (1:N)    Role (1:N)    Fund (1:N)
    [TenantId]    [TenantId]    [TenantId]
        │             │             │
        │             │             │
        │        RolePermission     │
        │             │             │
        │        Permission ◄───────┘
        │         (Shared)
        │
        └──────────► Transaction (1:N)
                    [TenantId]

Legend:
───────────────────
• (1:N)  = One-to-many relationship
• [TenantId] = Tenant-scoped table
• (Shared) = Shared across all tenants
```

---

## Multi-Tenancy Flow Diagram

```
COMPANY A                          COMPANY B
(TenantId: ABC...)                (TenantId: DEF...)
     │                                  │
     │                                  │
     ├─ User: John                      ├─ User: Alice
     │  └─ Token contains:              │  └─ Token contains:
     │     tenantId: ABC...             │     tenantId: DEF...
     │                                  │
     ├─ Fund: Cash (50,000)             ├─ Fund: Bank (100,000)
     │                                  │
     ├─ Query: SELECT * FROM Funds      ├─ Query: SELECT * FROM Funds
     │  Result: ONLY Cash               │  Result: ONLY Bank
     │  (TenantId=ABC)                  │  (TenantId=DEF)
     │                                  │
     ├─ Create Transaction:             ├─ Create Transaction:
     │  Amount: 5,000                   │  Amount: 10,000
     │  TenantId: ABC (auto)            │  TenantId: DEF (auto)
     │                                  │
     └─ Audit: John changed             └─ Audit: Alice changed
        Transaction #1                     Transaction #1
        (Different records)                (Different records)


Database (Single, Shared)
├─ Fund: "Cash" (TenantId: ABC)
├─ Fund: "Bank" (TenantId: DEF)
├─ Transaction #1: TenantId: ABC, UserId: John
└─ Transaction #1: TenantId: DEF, UserId: Alice
```

---

## Query Filter Concept

```
Without Global Query Filters (MANUAL):
────────────────────────────────────────
var currentTenantId = _tenantService.GetCurrentTenantId();
var funds = await _context.Funds
    .Where(f => f.TenantId == currentTenantId)  ◄── MUST REMEMBER
    .Where(f => f.IsActive)
    .ToListAsync();

Problem: Easy to forget, easy to get wrong


With Global Query Filters (AUTOMATIC):
──────────────────────────────────────────
var funds = await _context.Funds
    .Where(f => f.IsActive)
    .ToListAsync();

Result: TenantId filter applied automatically!
No forgetting, always correct, one place to configure
```

---

## Audit Trail Example

```
Transaction Timeline:
─────────────────────

T0: Initial State
   Fund: "Cash", Balance: 50,000

T1: User "John" Creates Record
   AuditLog:
   {
     Action: "INSERT",
     TableName: "Fund",
     UserId: "john123",
     OldValues: null,
     NewValues: {...full fund data...},
     Timestamp: 2024-01-15T10:30:00Z
   }

T2: User "John" Updates Record (Balance: 55,000)
   AuditLog:
   {
     Action: "UPDATE",
     TableName: "Fund",
     UserId: "john123",
     OldValues: {...Balance: 50000...},
     NewValues: {...Balance: 55000...},
     Timestamp: 2024-01-15T10:35:00Z
   }

T3: User "Admin" Deletes Record
   AuditLog:
   {
     Action: "DELETE",
     TableName: "Fund",
     UserId: "admin456",
     OldValues: {...full fund data...},
     NewValues: null,
     Timestamp: 2024-01-15T10:40:00Z
   }


Complete History: Available for compliance, debugging, audit
```

---

## JWT Token Structure

```
HEADER.PAYLOAD.SIGNATURE

HEADER:
{
  "alg": "HS256",
  "typ": "JWT"
}

PAYLOAD:
{
  "sub": "user123",
  "name": "john.doe",
  "tenantId": "550e8400-e29b-41d4-a716-446655440000",  ◄─── CRITICAL
  "userId": "user123",                                  ◄─── FOR AUDIT
  "role": "Admin",
  "iat": 1705315800,
  "exp": 1705319400,
  "iss": "CashFlowAPI",
  "aud": "CashFlowWinForms"
}

SIGNATURE:
HMACSHA256(
  base64UrlEncode(header) + "." +
  base64UrlEncode(payload),
  secret
)

Flow:
1. Token contains tenantId claim ─┐
2. TenantService extracts it      │
3. Applied to all queries         ├─ AUTOMATIC ISOLATION
4. No manual filtering needed     │
5. Data automatically scoped ─────┘
```

---

## Database Schema (Simplified)

```
TENANTS (Shared)
┌─────────────────────────────────┐
│ PK │ CompanyName    │ IsActive  │
├─────────────────────────────────┤
│ A  │ Company A      │ 1         │
│ B  │ Company B      │ 1         │
└─────────────────────────────────┘

USERS (Tenant-Scoped)
┌───────────────────────────────────────┐
│ PK │ TenantId │ Username    │ RoleId  │
├───────────────────────────────────────┤
│ 1  │ A        │ john        │ Admin   │
│ 2  │ B        │ alice       │ Manager │
│ 3  │ A        │ bob         │ User    │
└───────────────────────────────────────┘

FUNDS (Tenant-Scoped)
┌──────────────────────────────────────────┐
│ PK │ TenantId │ FundName       │ Balance │
├──────────────────────────────────────────┤
│ 1  │ A        │ Cash           │ 50,000  │
│ 2  │ A        │ Bank           │ 100,000 │
│ 3  │ B        │ Cash           │ 75,000  │
│ 4  │ B        │ Bank           │ 150,000 │
└──────────────────────────────────────────┘

Global Query Filter: WHERE TenantId = @tenantId
Company A sees: Funds 1, 2
Company B sees: Funds 3, 4
```

---

## Dependency Injection Container

```
Services Registered:
─────────────────────

IHttpContextAccessor
    ↓
    Provides: HTTP context

ITenantService (Scoped)
    ├─ Implementation: TenantService
    ├─ Depends on: IHttpContextAccessor
    └─ Provides: Current TenantId, UserId

AppDbContext (Scoped)
    ├─ Depends on: DbContextOptions<AppDbContext>
    ├─ Depends on: ITenantService
    └─ Provides: Database access

JwtTokenGenerator (Scoped)
    ├─ Depends on: IConfiguration
    └─ Provides: JWT tokens

IAuthenticationService (Built-in)
    ├─ Implementation: JWT Bearer
    ├─ Depends on: IConfiguration
    └─ Provides: Token validation

Request Scope:
──────────────
New instances per request:
    ├─ AppDbContext
    ├─ TenantService
    ├─ Controllers
    └─ Any scoped services

Shared instances:
    ├─ Configuration
    ├─ Logging
    └─ Any singleton services
```

---

## Clean Architecture Layers

```
┌─────────────────────────────────────┐
│   PRESENTATION LAYER                │ HTTP Requests/Responses
│   ├─ Controllers                    │ (FundsController.cs)
│   ├─ DTOs                           │
│   └─ Middleware                     │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│   APPLICATION LAYER                 │ Business Logic
│   ├─ Use Cases                      │ (TODO: Services)
│   ├─ Application Services           │
│   └─ DTOs                           │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│   DOMAIN LAYER                      │ Business Rules
│   ├─ Entities                       │ (Domain/Entities/)
│   ├─ Value Objects                  │
│   └─ Domain Interfaces              │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│   INFRASTRUCTURE LAYER              │ External Systems
│   ├─ Data Access (EF Core)          │ (Data/, Services/)
│   ├─ Third-party Services           │
│   ├─ Repositories (optional)        │
│   └─ External APIs                  │
└─────────────────────────────────────┘

Dependencies always flow INWARD
Entities have no external dependencies
Each layer depends on abstractions
```

---

## Deployment Architecture

```
┌───────────────────────────────────────────────────┐
│              PRODUCTION ENVIRONMENT               │
├───────────────────────────────────────────────────┤
│                                                   │
│  ┌──────────────────────────────────────────┐   │
│  │  Azure / AWS / On-Premises               │   │
│  │                                          │   │
│  │  ┌────────────────────────────────────┐  │   │
│  │  │  App Service / EC2                 │  │   │
│  │  │  ├─ .NET 10 Application            │  │   │
│  │  │  ├─ 2+ instances (scaling)         │  │   │
│  │  │  └─ Auto-restart on failure        │  │   │
│  │  └────────────────────────────────────┘  │   │
│  │                                          │   │
│  │  ┌────────────────────────────────────┐  │   │
│  │  │  Azure SQL / RDS                   │  │   │
│  │  │  ├─ Highly available               │  │   │
│  │  │  ├─ Automated backups              │  │   │
│  │  │  ├─ Geo-replication                │  │   │
│  │  │  └─ All tenants in one DB          │  │   │
│  │  └────────────────────────────────────┘  │   │
│  │                                          │   │
│  │  ┌────────────────────────────────────┐  │   │
│  │  │  Azure Key Vault                   │  │   │
│  │  │  ├─ JWT Secret                     │  │   │
│  │  │  ├─ DB Connection Strings          │  │   │
│  │  │  └─ API Keys                       │  │   │
│  │  └────────────────────────────────────┘  │   │
│  │                                          │   │
│  │  ┌────────────────────────────────────┐  │   │
│  │  │  Azure Application Insights        │  │   │
│  │  │  ├─ Logging                        │  │   │
│  │  │  ├─ Performance Monitoring         │  │   │
│  │  │  └─ Alerts                         │  │   │
│  │  └────────────────────────────────────┘  │   │
│  │                                          │   │
│  └──────────────────────────────────────────┘   │
│                                                   │
└───────────────────────────────────────────────────┘
```

---

## Performance Considerations

```
Optimized for Multi-Tenancy:
──────────────────────────────

Query Performance:
✅ Global query filters at database level
   (WHERE TenantId = @tenantId pushed down)

✅ Indexes on (TenantId, OtherColumns)
   Example: (TenantId, FundId, Amount)

✅ Query projection (select only needed columns)
   .Select(f => new { f.Id, f.FundName })

Connection Pooling:
✅ Built-in EF Core connection pooling
✅ Reduces connection overhead

Caching:
⚠️ Per-tenant caching recommended
   Cache key should include tenantId

Scaling:
✅ Shared database handles many tenants
✅ Horizontal scaling of API tier
✅ Read replicas for reporting

Monitoring:
✅ Query performance metrics
✅ Connection pool status
✅ Transaction duration
```

---

This visual reference provides diagrams and flows for understanding the multi-tenant architecture!
