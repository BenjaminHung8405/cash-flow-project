# 📱 WinForms Integration với Multi-Tenant API

## Tổng Quan Kiến Trúc

```
WINFORMS APPLICATION
(CashFlow.WinForms)
        │
        ├─ Forms/
        │  └─ LoginForm
        │  └─ FundsManagementForm
        │  └─ TransactionForm
        │
        ├─ Services/
        │  └─ ApiClient.cs ← HTTP Client
        │
        └─ Settings/
           └─ JWT Token (Local Storage)

                    ↕ HTTP/HTTPS

REST API
(CashFlow.API)
        │
        ├─ Controllers/
        │  ├─ AuthController
        │  └─ FundsController
        │
        ├─ Infrastructure/
        │  ├─ TenantService (JWT Claims)
        │  └─ JwtTokenGenerator
        │
        └─ Data/
           └─ AppDbContext (EF Core + Query Filters)

                    ↕ SQL

SQL SERVER DATABASE
CashFlowDB
        │
        ├─ Tenants (Organization)
        ├─ Users (TenantId)
        ├─ Funds (TenantId)
        ├─ Transactions (TenantId)
        └─ AuditLogs (TenantId)
```

---

## 🔐 Luồng Đăng Nhập

### 1. WinForms - LoginForm.cs

```csharp
private async void LoginButton_Click(object sender, EventArgs e)
{
    try
    {
        var apiClient = new ApiClient("https://localhost:5001");
        
        // 1. Gọi API đăng nhập
        var response = await apiClient.LoginAsync(
            UsernameTextBox.Text,
            PasswordTextBox.Text
        );
        
        // 2. Lưu JWT Token vào Settings
        Properties.Settings.Default["JwtToken"] = response.Token;
        Properties.Settings.Default.Save();
        
        // 3. Lưu thông tin user
        Properties.Settings.Default["UserName"] = response.FullName;
        Properties.Settings.Default["UserRole"] = response.Role;
        Properties.Settings.Default.Save();
        
        // 4. Mở MainForm
        var mainForm = new MainForm();
        mainForm.Show();
        this.Close();
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Login failed: {ex.Message}", "Error");
    }
}
```

### 2. API - AuthController

```csharp
[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginRequest request)
{
    // 1. Validate credentials
    var user = await _context.Users
        .Include(u => u.Role)
        .FirstOrDefaultAsync(u => u.Username == request.Username);
    
    if (user == null)
        return Unauthorized();
    
    // 2. Generate JWT với tenantId claim
    var token = _jwtTokenGenerator.GenerateToken(
        userId: user.Id.ToString(),
        username: user.Username,
        tenantId: user.TenantId,  // ← MULTI-TENANCY
        roles: new[] { user.Role.RoleName }
    );
    
    // 3. Trả về response
    return Ok(new LoginResponse
    {
        Token = token,
        FullName = user.Username,
        Role = user.Role.RoleName
    });
}
```

### 3. JWT Token Payload

```json
{
  "sub": "user-123",
  "name": "john.doe",
  "tenantId": "tenant-abc",     ← CRITICAL
  "userId": "user-123",         ← For Audit
  "role": "Accountant",
  "iat": 1705315800,
  "exp": 1705319400
}
```

---

## 📥 Luồng Lấy Dữ Liệu (GET)

### WinForms Request

```csharp
// 1. WinForms gọi API
private async Task LoadFundsAsync()
{
    var apiClient = new ApiClient();
    var token = Properties.Settings.Default["JwtToken"].ToString();
    apiClient.SetJwtToken(token);
    
    var funds = await apiClient.GetFundsAsync();
    FundsDataGridView.DataSource = funds;
}
```

### API Request Flow

```
WinForms
   ↓
   GET /api/funds
   Authorization: Bearer eyJhbGc...
   ↓
FundsController.GetFunds()
   ├─ [Authorize] middleware kiểm tra JWT
   ├─ TenantService.GetCurrentTenantId()
   │  └─ Đọc claim "tenantId" từ JWT
   │     → "tenant-abc"
   ├─ Query Funds
   │  └─ WHERE Funds.TenantId = "tenant-abc"
   └─ Return Funds
      ↓
   HTTP 200 OK
   [
      {
         "id": "fund-1",
         "tenantId": "tenant-abc",
         "fundName": "Cash",
         "currentBalance": 50000
      }
   ]
   ↓
WinForms DataGridView hiển thị
```

### Database Query

```sql
-- EF Core sinh ra:
SELECT f.*
FROM Funds f
WHERE f.TenantId = @tenantId  -- Auto-added by Global Query Filter
AND f.IsActive = 1

-- Ví dụ dữ liệu:
Company A sees:
- Funds với TenantId = tenant-abc

Company B sees:
- Funds với TenantId = tenant-def

(Tự động isolation - không cần manual WHERE)
```

---

## 📤 Luồng Tạo Dữ Liệu (POST)

### WinForms Form

```csharp
private async void AddFundButton_Click(object sender, EventArgs e)
{
    // 1. Validate input
    if (string.IsNullOrWhiteSpace(FundNameTextBox.Text))
    {
        MessageBox.Show("Enter fund name");
        return;
    }
    
    // 2. Tạo request
    var request = new ApiClient.CreateFundRequest
    {
        FundName = FundNameTextBox.Text,
        FundType = FundTypeComboBox.SelectedValue.ToString(),
        AccountNumber = AccountNumberTextBox.Text,
        InitialBalance = decimal.Parse(BalanceTextBox.Text)
    };
    
    // 3. Gọi API
    var apiClient = new ApiClient();
    apiClient.SetJwtToken(Properties.Settings.Default["JwtToken"].ToString());
    
    var newFund = await apiClient.CreateFundAsync(request);
    
    // 4. Refresh danh sách
    await LoadFundsAsync();
}
```

### API Processing

```
FundsController.CreateFund(request)
   │
   ├─ 1. Get current tenant
   │  └─ TenantService.GetCurrentTenantId()
   │     → từ JWT claims: "tenant-abc"
   │
   ├─ 2. Create Fund entity
   │  └─ new Fund
   │     {
   │        FundName = "New Fund",
   │        FundType = "CASH",
   │        CurrentBalance = 50000
   │        // TenantId NOT SET YET
   │     }
   │
   ├─ 3. Add to context
   │  └─ _context.Funds.Add(fund)
   │
   ├─ 4. SaveChangesAsync()
   │  │
   │  ├─ ProcessChangeTracker()
   │  │  ├─ Detect entity in Added state
   │  │  ├─ Inject TenantId: fund.TenantId = "tenant-abc"
   │  │  ├─ Set CreatedAt, CreatedBy
   │  │  └─ Create AuditLog entry
   │  │
   │  └─ base.SaveChangesAsync()
   │     └─ INSERT Fund
   │        INSERT AuditLog
   │
   └─ Return HTTP 201 Created with new Fund
```

### Database Changes

```sql
-- Funds table mới thêm:
INSERT INTO Funds 
(Id, TenantId, FundName, FundType, CurrentBalance, CreatedAt, CreatedBy)
VALUES
('550e8400...', 'tenant-abc', 'New Fund', 'CASH', 50000, NOW(), 'john.doe')

-- AuditLogs tự động tạo:
INSERT INTO AuditLogs
(Id, TenantId, UserId, Action, TableName, RecordId, NewValues, Timestamp)
VALUES
('550e8401...', 'tenant-abc', 'john.doe', 'INSERT', 'Fund', '550e8400...', '{...}', NOW())
```

---

## 🔄 Luồng Cập Nhật Dữ Liệu (PUT)

### WinForms Form

```csharp
private async void EditFundButton_Click(object sender, EventArgs e)
{
    // 1. Select row từ DataGridView
    var selectedRow = FundsDataGridView.SelectedRows[0];
    var fundId = selectedRow.Cells["Id"].Value.ToString();
    
    // 2. Tạo update request
    var request = new ApiClient.UpdateFundRequest
    {
        FundName = FundNameTextBox.Text,
        FundType = FundTypeComboBox.SelectedValue.ToString()
    };
    
    // 3. Gọi API PUT
    var apiClient = new ApiClient();
    apiClient.SetJwtToken(GetStoredToken());
    await apiClient.UpdateFundAsync(fundId, request);
    
    // 4. Refresh
    await LoadFundsAsync();
}
```

### API Processing

```
FundsController.UpdateFund(id, request)
   │
   ├─ 1. Find Fund
   │  └─ var fund = _context.Funds.FindAsync(id)
   │
   ├─ 2. Update properties
   │  └─ fund.FundName = request.FundName
   │     fund.FundType = request.FundType
   │
   ├─ 3. SaveChangesAsync()
   │  │
   │  ├─ ProcessChangeTracker()
   │  │  ├─ Detect entity in Modified state
   │  │  ├─ Get OLD values
   │  │  ├─ Get NEW values
   │  │  └─ Create AuditLog with BOTH
   │  │     {
   │  │        "OldValues": {"FundType": "CASH"},
   │  │        "NewValues": {"FundType": "BANK"}
   │  │     }
   │  │
   │  └─ base.SaveChangesAsync()
   │     └─ UPDATE Fund
   │        INSERT AuditLog
   │
   └─ Return HTTP 200 OK with updated Fund
```

### Audit Log Created

```json
{
  "Id": "550e8401...",
  "TenantId": "tenant-abc",
  "UserId": "john.doe",
  "Action": "UPDATE",
  "TableName": "Fund",
  "RecordId": "550e8400...",
  "OldValues": "{\"FundType\":\"CASH\"}",
  "NewValues": "{\"FundType\":\"BANK\"}",
  "Timestamp": "2024-01-15T14:30:00Z"
}
```

---

## 🗑️ Luồng Xóa Dữ Liệu (DELETE)

### WinForms Form

```csharp
private async void DeleteFundButton_Click(object sender, EventArgs e)
{
    // 1. Confirm xóa
    var result = MessageBox.Show(
        "Are you sure?",
        "Confirm",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning
    );
    
    if (result != DialogResult.Yes)
        return;
    
    // 2. Lấy Fund ID
    var selectedRow = FundsDataGridView.SelectedRows[0];
    var fundId = selectedRow.Cells["Id"].Value.ToString();
    
    // 3. Gọi API DELETE
    var apiClient = new ApiClient();
    apiClient.SetJwtToken(GetStoredToken());
    await apiClient.DeleteFundAsync(fundId);
    
    // 4. Refresh
    await LoadFundsAsync();
}
```

### API Processing

```
FundsController.DeleteFund(id)
   │
   ├─ 1. Find Fund
   │  └─ var fund = _context.Funds.FindAsync(id)
   │
   ├─ 2. Mark as deleted (soft delete)
   │  └─ fund.IsActive = false
   │
   ├─ 3. SaveChangesAsync()
   │  │
   │  ├─ ProcessChangeTracker()
   │  │  ├─ Detect Modified state (IsActive changed)
   │  │  ├─ Get all current values
   │  │  └─ Create AuditLog
   │  │
   │  └─ base.SaveChangesAsync()
   │     └─ UPDATE Fund SET IsActive = 0
   │        INSERT AuditLog
   │
   └─ Return HTTP 200 OK
```

---

## 📊 Multi-Tenancy Bảo Mật

### Scenario: Hai công ty khác nhau

```
COMPANY A (TenantId: abc)
   User: john
   Login → Token with tenantId: "abc"
   
   Request: GET /api/funds
   JWT: {tenantId: "abc"}
   ↓
   API Query: WHERE TenantId = "abc"
   ↓
   Result: [Fund1, Fund2] (chỉ của công ty A)

---

COMPANY B (TenantId: def)
   User: alice
   Login → Token with tenantId: "def"
   
   Request: GET /api/funds
   JWT: {tenantId: "def"}
   ↓
   API Query: WHERE TenantId = "def"
   ↓
   Result: [Fund3, Fund4] (chỉ của công ty B)

---

HACKING ATTEMPT:
User A gửi: GET /api/funds?tenantId=def
            (cố gắng thay đổi tenantId)

API vẫn dùng: WHERE TenantId = "abc"
(từ JWT claims, không từ query parameter)

Result: Query Filter vẫn lọc theo "abc"
Không thể truy cập dữ liệu công ty B ✅
```

---

## 🔍 Audit Trail trong WinForms

### Xem lịch sử thay đổi

```csharp
private async void ViewAuditButton_Click(object sender, EventArgs e)
{
    // 1. Select Fund
    var fundId = FundsDataGridView.SelectedRows[0].Cells["Id"].Value.ToString();
    
    // 2. Gọi API get audit trail
    var apiClient = new ApiClient();
    apiClient.SetJwtToken(GetStoredToken());
    var auditLogs = await apiClient.GetFundAuditTrailAsync(fundId);
    
    // 3. Hiển thị trong form
    foreach (var log in auditLogs)
    {
        Console.WriteLine($"User: {log.UserId}");
        Console.WriteLine($"Action: {log.Action}");
        Console.WriteLine($"Time: {log.Timestamp}");
        Console.WriteLine($"Old: {log.OldValues}");
        Console.WriteLine($"New: {log.NewValues}");
        Console.WriteLine("---");
    }
}
```

---

## ✅ So Sánh: WinForms vs Direct DB Connection

| Feature | WinForms + API | WinForms + Direct DB |
|---------|---|---|
| **Multi-Tenancy** | ✅ Automatic (JWT) | ⚠️ Manual WHERE clause |
| **Security** | ✅ JWT Token | ❌ Connection String |
| **Audit Trail** | ✅ Automatic | ❌ Manual |
| **Scalability** | ✅ API tier | ❌ DB connections |
| **Maintenance** | ✅ Centralized | ❌ Distributed |
| **Data Leakage** | ✅ Impossible | ❌ Easy mistake |
| **Encryption** | ✅ HTTPS | ⚠️ Depends on setup |

---

## 🚀 Lợi Ích Kiến Trúc API

### 1. Multi-Tenancy An Toàn
```
Direct DB: Developer quên WHERE TenantId
Api: Global Filter bắt buộc
```

### 2. Audit Trail Tự Động
```
Direct DB: Phải code manual
API: Tự động ProcessChangeTracker
```

### 3. Bảo Mật Tốt Hơn
```
Direct DB: Connection string ở client
API: JWT token + secure API server
```

### 4. Dễ Mở Rộng
```
Direct DB: Chỉ WinForms
API: WinForms + Web + Mobile + Desktop
```

### 5. Quản Lý Tập Trung
```
Direct DB: Thay đổi phải cập nhật client
API: Thay đổi chỉ trên server
```

---

## 📋 Checklist WinForms Integration

- [ ] Tạo `ApiClient` service
- [ ] Tạo `LoginForm` gọi `/api/auth/login`
- [ ] Lưu JWT Token vào `Properties.Settings`
- [ ] Tạo `FundsManagementForm`
- [ ] Implement GET /api/funds
- [ ] Implement POST /api/funds (Create)
- [ ] Implement PUT /api/funds/{id} (Update)
- [ ] Implement DELETE /api/funds/{id} (Delete)
- [ ] Hiển thị Audit Trail
- [ ] Test multi-tenant isolation
- [ ] Add error handling
- [ ] Add loading indicators

---

## 📝 Kết Luận

**CÓ, cấu trúc này hoàn toàn thích hợp cho WinForms!**

**Lợi ích:**
1. ✅ Multi-tenancy tự động
2. ✅ Audit trail tự động
3. ✅ Bảo mật tốt hơn
4. ✅ Dễ bảo trì
5. ✅ Dễ mở rộng

**Cách hoạt động:**
- WinForms gửi HTTP request với JWT Token
- API xử lý request, tự động lọc theo TenantId
- Database chỉ trả về dữ liệu của tenant hiện tại
- Audit trail tự động ghi lại mọi thay đổi

**Không cần kéo dữ liệu trực tiếp từ DB - API xử lý tất cả!** 🎯
