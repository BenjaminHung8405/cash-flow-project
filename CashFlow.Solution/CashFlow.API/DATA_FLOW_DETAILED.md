# 🔄 Chi Tiết Luồng Dữ Liệu - WinForms ↔ API ↔ Database

## Luồng TOÀN BỘ từ WinForms Login đến hiển thị dữ liệu

```
┌─────────────────────────────────────────────────────────────────┐
│ STEP 1: USER ENTERS CREDENTIALS IN WINFORMS                    │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  LoginForm.xaml.cs                                              │
│  ┌──────────────────────────────────┐                           │
│  │ [Username: john.doe             ]                           │
│  │ [Password: ••••••••••           ]                           │
│  │ [  Login Button                 ]                           │
│  └──────────────────────────────────┘                           │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘

                            ↓

┌─────────────────────────────────────────────────────────────────┐
│ STEP 2: WINFORMS SENDS HTTP REQUEST TO API                     │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  WinForms Code:                                                 │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │ var apiClient = new ApiClient("https://localhost:5001");   │ │
│  │ var response = await apiClient.LoginAsync(                 │ │
│  │     "john.doe",      // Username                           │ │
│  │     "password123"    // Password                           │ │
│  │ );                                                         │ │
│  └────────────────────────────────────────────────────────────┘ │
│                                                                  │
│  HTTP Request:                                                   │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │ POST /api/auth/login HTTP/1.1                              │ │
│  │ Host: localhost:5001                                       │ │
│  │ Content-Type: application/json                             │ │
│  │ Content-Length: 42                                         │ │
│  │                                                            │ │
│  │ {                                                          │ │
│  │   "username": "john.doe",                                  │ │
│  │   "password": "password123"                                │ │
│  │ }                                                          │ │
│  └────────────────────────────────────────────────────────────┘ │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘

                            ↓

┌─────────────────────────────────────────────────────────────────┐
│ STEP 3: API RECEIVES REQUEST & QUERIES DATABASE                │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  AuthController.cs                                              │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │ [HttpPost("login")]                                        │ │
│  │ public async Task<IActionResult> Login(                    │ │
│  │     [FromBody] LoginRequest request)                       │ │
│  │ {                                                          │ │
│  │     // 1. Find user in database                            │ │
│  │     var user = await _context.Users                        │ │
│  │         .Include(u => u.Role)                              │ │
│  │         .FirstOrDefaultAsync(u =>                          │ │
│  │             u.Username == request.Username);              │ │
│  │                                                            │ │
│  │     // 2. Verify password                                  │ │
│  │     if (!VerifyPassword(request.Password,                 │ │
│  │          user.PasswordHash))                              │ │
│  │         return Unauthorized();                            │ │
│  │                                                            │ │
│  │     // 3. Generate JWT with tenantId                       │ │
│  │     var token = _jwtTokenGenerator.GenerateToken(         │ │
│  │         userId: user.Id.ToString(),                       │ │
│  │         tenantId: user.TenantId,     ← CRITICAL!          │ │
│  │         roles: new[] { user.Role.RoleName }               │ │
│  │     );                                                    │ │
│  │                                                            │ │
│  │     return Ok(new LoginResponse {                         │ │
│  │         Token = token,                                    │ │
│  │         FullName = user.Username,                         │ │
│  │         Role = user.Role.RoleName                         │ │
│  │     });                                                   │ │
│  │ }                                                          │ │
│  └────────────────────────────────────────────────────────────┘ │
│                                                                  │
│  Database Query:                                                 │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │ SELECT u.*, r.*                                            │ │
│  │ FROM Users u                                               │ │
│  │ INNER JOIN Roles r ON u.RoleId = r.Id                     │ │
│  │ WHERE u.Username = 'john.doe'                              │ │
│  │                                                            │ │
│  │ Result:                                                    │ │
│  │ ┌─────────────────────────────────────────────────────┐   │ │
│  │ │ Id: 550e8400...                                     │   │ │
│  │ │ TenantId: 550e8400-e29b-41d4-a716-446655440001 ← HERE │ │
│  │ │ Username: john.doe                                  │   │ │
│  │ │ RoleId: 550e8400...                                 │   │ │
│  │ │ RoleName: Accountant                                │   │ │
│  │ └─────────────────────────────────────────────────────┘   │ │
│  └────────────────────────────────────────────────────────────┘ │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘

                            ↓

┌─────────────────────────────────────────────────────────────────┐
│ STEP 4: API GENERATES JWT TOKEN WITH TENANTID CLAIM            │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  JwtTokenGenerator.cs                                            │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │ var claims = new List<Claim>                              │ │
│  │ {                                                          │ │
│  │     new Claim(ClaimTypes.NameIdentifier, "550e8400..."),  │ │
│  │     new Claim(ClaimTypes.Name, "john.doe"),               │ │
│  │     new Claim("tenantId",                                 │ │
│  │         "550e8400-e29b-41d4-a716-446655440001"),  ← KEY! │ │
│  │     new Claim("userId", "550e8400..."),                   │ │
│  │     new Claim(ClaimTypes.Role, "Accountant")              │ │
│  │ };                                                         │ │
│  │                                                            │ │
│  │ var token = new JwtSecurityToken(                         │ │
│  │     issuer: "CashFlowAPI",                                │ │
│  │     audience: "CashFlowWinForms",                         │ │
│  │     claims: claims,                                       │ │
│  │     expires: DateTime.UtcNow.AddMinutes(60),              │ │
│  │     signingCredentials: credentials                       │ │
│  │ );                                                         │ │
│  └────────────────────────────────────────────────────────────┘ │
│                                                                  │
│  JWT Token Structure:                                            │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │ eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.                    │ │
│  │ {                                                          │ │
│  │   "sub": "550e8400...",                                    │ │
│  │   "name": "john.doe",                                      │ │
│  │   "tenantId": "550e8400-e29b-41d4-a716-446655440001",     │ │
│  │   "userId": "550e8400...",                                 │ │
│  │   "role": "Accountant",                                    │ │
│  │   "iat": 1705315800,                                       │ │
│  │   "exp": 1705319400                                        │ │
│  │ }.                                                         │ │
│  │ [HMACSHA256 SIGNATURE]                                     │ │
│  └────────────────────────────────────────────────────────────┘ │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘

                            ↓

┌─────────────────────────────────────────────────────────────────┐
│ STEP 5: API RETURNS JWT TOKEN TO WINFORMS                      │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  HTTP Response:                                                  │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │ HTTP/1.1 200 OK                                            │ │
│  │ Content-Type: application/json                             │ │
│  │                                                            │ │
│  │ {                                                          │ │
│  │   "token": "eyJhbGciOiJIUzI1NiIsInR5cCI...",              │ │
│  │   "fullName": "john.doe",                                  │ │
│  │   "role": "Accountant"                                     │ │
│  │ }                                                          │ │
│  └────────────────────────────────────────────────────────────┘ │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘

                            ↓

┌─────────────────────────────────────────────────────────────────┐
│ STEP 6: WINFORMS SAVES JWT & OPENS MAIN FORM                   │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  WinForms Code:                                                 │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │ // Save token to local settings                            │ │
│  │ Properties.Settings.Default["JwtToken"] =                  │ │
│  │     response.Token;                                        │ │
│  │ Properties.Settings.Default.Save();                        │ │
│  │                                                            │ │
│  │ // Open main form                                          │ │
│  │ var mainForm = new FundsManagementForm();                  │ │
│  │ mainForm.Show();                                           │ │
│  │ this.Close();                                              │ │
│  └────────────────────────────────────────────────────────────┘ │
│                                                                  │
│  Local Storage:                                                  │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │ app.config / user.config (Encrypted)                       │ │
│  │                                                            │ │
│  │ <setting name="JwtToken" serializeAs="String">            │ │
│  │   <value>eyJhbGciOiJIUzI1NiIsInR5cCI...</value>           │ │
│  │ </setting>                                                 │ │
│  └────────────────────────────────────────────────────────────┘ │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘

                            ↓

┌─────────────────────────────────────────────────────────────────┐
│ STEP 7: WINFORMS FORM LOAD - GET /api/funds                    │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  FundsManagementForm_Load()                                      │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │ private async void FundsManagementForm_Load(...)           │ │
│  │ {                                                          │ │
│  │     var token = Properties.Settings.Default["JwtToken"];  │ │
│  │     var apiClient = new ApiClient();                       │ │
│  │     apiClient.SetJwtToken(token);                         │ │
│  │                                                            │ │
│  │     var funds = await apiClient.GetFundsAsync();          │ │
│  │     FundsDataGridView.DataSource = funds;                 │ │
│  │ }                                                          │ │
│  └────────────────────────────────────────────────────────────┘ │
│                                                                  │
│  HTTP Request:                                                   │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │ GET /api/funds HTTP/1.1                                    │ │
│  │ Host: localhost:5001                                       │ │
│  │ Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI... ← KEY │ │
│  │ Accept: application/json                                   │ │
│  └────────────────────────────────────────────────────────────┘ │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘

                            ↓

┌─────────────────────────────────────────────────────────────────┐
│ STEP 8: API RECEIVES GET /api/funds REQUEST                    │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  Middleware:                                                     │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │ 1. [Authorize] - Validate JWT                              │ │
│  │ 2. Extract claims from JWT                                 │ │
│  │    - tenantId: "550e8400-e29b-41d4-a716-446655440001"     │ │
│  │    - userId: "550e8400..."                                 │ │
│  │ 3. Store in HttpContext                                    │ │
│  └────────────────────────────────────────────────────────────┘ │
│                                                                  │
│  FundsController.GetFunds()                                      │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │ [Authorize]                                                │ │
│  │ [HttpGet]                                                  │ │
│  │ public async Task<ActionResult<IEnumerable<Fund>>>         │ │
│  │     GetFunds()                                             │ │
│  │ {                                                          │ │
│  │     try                                                    │ │
│  │     {                                                      │ │
│  │         // TenantService reads tenantId from JWT           │ │
│  │         var currentTenantId =                              │ │
│  │             _tenantService.GetCurrentTenantId();          │ │
│  │                                                            │ │
│  │         var funds = await _context.Funds               │ │
│  │             .AsNoTracking()                                │ │
│  │             .Where(f => f.IsActive)                       │ │
│  │             .ToListAsync();                                │ │
│  │                                                            │ │
│  │         // Global Query Filter AUTO-APPLIED:               │ │
│  │         // .Where(f => f.TenantId == currentTenantId) ← ! │ │
│  │                                                            │ │
│  │         return Ok(funds);                                 │ │
│  │     }                                                      │ │
│  │     catch (InvalidOperationException ex)                  │ │
│  │     {                                                      │ │
│  │         return Unauthorized(...);                         │ │
│  │     }                                                      │ │
│  │ }                                                          │ │
│  └────────────────────────────────────────────────────────────┘ │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘

                            ↓

┌─────────────────────────────────────────────────────────────────┐
│ STEP 9: DBCONTEXT APPLIES GLOBAL QUERY FILTER                  │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  AppDbContext.OnModelCreating()                                  │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │ // Global Query Filter - Auto-applied to ALL queries       │ │
│  │ modelBuilder.Entity<Fund>()                                │ │
│  │     .HasQueryFilter(x =>                                   │ │
│  │         x.TenantId == currentTenantId);                    │ │
│  │                                                            │ │
│  │ // currentTenantId = "550e8400-e29b-41d4-..."  (from JWT) │ │
│  └────────────────────────────────────────────────────────────┘ │
│                                                                  │
│  EF Core builds SQL query:                                       │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │ SELECT f.*                                                 │ │
│  │ FROM Funds f                                               │ │
│  │ WHERE f.TenantId = '550e8400-e29b-41d4-a716...'   ← AUTO  │ │
│  │ AND f.IsActive = 1                                         │ │
│  │                                                            │ │
│  │ Parameters:                                                │ │
│  │ - @p0: 550e8400-e29b-41d4-a716-446655440001 (tenantId)   │ │
│  │ - @p1: true (IsActive)                                     │ │
│  └────────────────────────────────────────────────────────────┘ │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘

                            ↓

┌─────────────────────────────────────────────────────────────────┐
│ STEP 10: DATABASE EXECUTES QUERY & RETURNS DATA                │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  SQL Server Database:                                            │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │ Funds Table:                                               │ │
│  │                                                            │ │
│  │ ID | TenantId | FundName | FundType | CurrentBalance | ... │ │
│  │────|──────────|──────────|──────────|────────────────│    │ │
│  │ F1 │ ABC ← A  │ Cash     │ CASH     │ 50,000         │    │ │
│  │ F2 │ ABC ← A  │ Bank     │ BANK     │ 100,000        │    │ │
│  │ F3 │ DEF ← B  │ Cash     │ CASH     │ 75,000         │    │ │
│  │ F4 │ DEF ← B  │ Bank     │ BANK     │ 150,000        │    │ │
│  │────|──────────|──────────|──────────|────────────────│    │ │
│  │                                                            │ │
│  │ Query returns:                                             │ │
│  │ - F1 (ABC matches tenantId ✓)                              │ │
│  │ - F2 (ABC matches tenantId ✓)                              │ │
│  │ NOT returned:                                              │ │
│  │ - F3 (DEF ≠ ABC)                                           │ │
│  │ - F4 (DEF ≠ ABC)                                           │ │
│  └────────────────────────────────────────────────────────────┘ │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘

                            ↓

┌─────────────────────────────────────────────────────────────────┐
│ STEP 11: API RETURNS JSON DATA TO WINFORMS                     │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  HTTP Response:                                                  │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │ HTTP/1.1 200 OK                                            │ │
│  │ Content-Type: application/json                             │ │
│  │                                                            │ │
│  │ [                                                          │ │
│  │   {                                                        │ │
│  │     "id": "550e8400-e29b-41d4-a716-446655440010",        │ │
│  │     "tenantId": "550e8400-e29b-41d4-a716-446655440001",  │ │
│  │     "fundName": "Tiền mặt VP",                             │ │
│  │     "fundType": "CASH",                                    │ │
│  │     "accountNumber": null,                                 │ │
│  │     "currentBalance": 50000.00,                            │ │
│  │     "isActive": true,                                      │ │
│  │     "createdAt": "2024-01-15T10:30:00Z",                  │ │
│  │     "createdBy": "john.doe"                                │ │
│  │   },                                                       │ │
│  │   {                                                        │ │
│  │     "id": "550e8400-e29b-41d4-a716-446655440011",        │ │
│  │     "tenantId": "550e8400-e29b-41d4-a716-446655440001",  │ │
│  │     "fundName": "Vietcombank",                             │ │
│  │     "fundType": "BANK",                                    │ │
│  │     "accountNumber": "0123456789",                         │ │
│  │     "currentBalance": 100000.00,                           │ │
│  │     "isActive": true,                                      │ │
│  │     "createdAt": "2024-01-15T11:00:00Z",                  │ │
│  │     "createdBy": "john.doe"                                │ │
│  │   }                                                        │ │
│  │ ]                                                          │ │
│  └────────────────────────────────────────────────────────────┘ │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘

                            ↓

┌─────────────────────────────────────────────────────────────────┐
│ STEP 12: WINFORMS DESERIALIZES & DISPLAYS DATA                 │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  WinForms Code:                                                 │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │ var funds = JsonSerializer.Deserialize<List<FundDto>>(...) │ │
│  │                                                            │ │
│  │ FundsDataGridView.DataSource = funds;                      │ │
│  └────────────────────────────────────────────────────────────┘ │
│                                                                  │
│  DataGridView Display:                                           │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │ Funds Management                                           │ │
│  │ ┌──────────────────────────────────────────────────────┐  │ │
│  │ │ FundName      │ FundType │ Balance │ CreatedAt      │  │ │
│  │ ├──────────────────────────────────────────────────────┤  │ │
│  │ │ Tiền mặt VP   │ CASH     │ 50,000  │ 2024-01-15... │  │ │
│  │ │ Vietcombank   │ BANK     │ 100,000 │ 2024-01-15... │  │ │
│  │ └──────────────────────────────────────────────────────┘  │ │
│  │                                                            │ │
│  │ [ Add Fund ]  [ Edit ]  [ Delete ]  [ View Audit Trail ]  │ │
│  └────────────────────────────────────────────────────────────┘ │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

---

## Tóm Tắt Luồng Dữ Liệu

```
┌──────────────┐
│   WINFORMS   │
│    Client    │
└──────┬───────┘
       │
       │ 1. POST /api/auth/login
       │    (username, password)
       │
       ├─→ API validates & queries DB
       ├─→ API finds user with TenantId
       ├─→ API generates JWT with tenantId claim
       │
       │ 2. Response: JWT Token
       │
       └─ Saves JWT locally
       │
       │ 3. GET /api/funds
       │    Authorization: Bearer [JWT]
       │
       ├─→ API middleware validates JWT
       ├─→ API extracts tenantId from JWT
       ├─→ Global Query Filter applied
       │    WHERE TenantId = @tenantId
       ├─→ Query executes in DB
       ├─→ Only tenant's data returned
       │
       │ 4. Response: JSON Array
       │
       └─ Displays in DataGridView
```

**KEY INSIGHT**: Dữ liệu được filter tự động ở API level, không bao giờ về client mà không được kiểm tra TenantId! 🔒
