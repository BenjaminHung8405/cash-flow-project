# 📁 Cấu trúc dự án - Feature-Based Architecture

## 🎯 Tổng quan

Dự án sử dụng **Feature-Based Architecture** với module-based organization. Mỗi feature/module là một đơn vị độc lập chứa:
- Controllers (HTTP endpoints)
- Services (Business logic)
- DTOs (Data transfer objects)
- Interfaces (Contracts)

## 📂 Cấu trúc thư mục

```
CashFlow.API/
│
├── Domain/                          [Shared - Không thay đổi]
│   └── Entities/
│       ├── BaseTenantEntity.cs     (Base class cho multi-tenancy)
│       ├── Tenant.cs
│       ├── User.cs
│       ├── Role.cs
│       ├── Permission.cs
│       ├── RolePermission.cs
│       ├── Fund.cs
│       ├── Transaction.cs
│       └── AuditLog.cs
│
├── Infrastructure/                  [Shared - Không thay đổi]
│   ├── Services/
│   │   ├── ITenantService.cs       (Giải quyết TenantId từ JWT)
│   │   └── TenantService.cs
│   ├── Security/
│   │   └── JwtTokenGenerator.cs    (Tạo JWT Token)
│   └── Extensions/
│       ├── ServiceCollectionExtensions.cs
│       └── ModuleServiceExtensions.cs (Đăng ký module services)
│
├── Data/                            [Shared - Không thay đổi]
│   └── AppDbContext.cs             (EF Core DbContext)
│
├── Modules/                         [Feature-based modules]
│   ├── Auth/
│   │   ├── Controllers/
│   │   │   └── AuthController.cs   (Login, Register, Refresh Token)
│   │   ├── DTOs/
│   │   │   └── AuthDTOs.cs         (LoginRequest, LoginResponse, etc.)
│   │   ├── Services/
│   │   │   └── AuthService.cs      (Business logic xác thực)
│   │   └── Interfaces/
│   │       └── IAuthService.cs     (Contract của service)
│   │
│   ├── Funds/
│   │   ├── Controllers/
│   │   │   └── FundsController.cs  (CRUD quỹ/tài khoản)
│   │   ├── DTOs/
│   │   │   └── FundDTOs.cs         (FundDto, CreateFundRequest, etc.)
│   │   ├── Services/
│   │   │   └── FundService.cs      (Business logic quỹ)
│   │   └── Interfaces/
│   │       └── IFundService.cs     (Contract của service)
│   │
│   ├── Users/                       [TODO]
│   │   ├── Controllers/
│   │   ├── DTOs/
│   │   ├── Services/
│   │   └── Interfaces/
│   │
│   ├── Roles/                       [TODO]
│   │   ├── Controllers/
│   │   ├── DTOs/
│   │   ├── Services/
│   │   └── Interfaces/
│   │
│   └── Transactions/                [TODO]
│       ├── Controllers/
│       ├── DTOs/
│       ├── Services/
│       └── Interfaces/
│
├── Program.cs                       (Startup configuration)
├── appsettings.json                (Configuration)
│
└── Documentation/
    ├── README.md                    (Project overview)
    └── QUICKSTART.md               (Getting started)
```

## 🔄 Luồng dữ liệu

### 1️⃣ **Login (xác thực)**

```
┌─────────────────┐
│  WinForms/Web   │
└────────┬────────┘
         │ 1. POST /api/auth/login
         │    { username, password }
         ▼
┌──────────────────────────┐
│ AuthController.Login()   │
└────────┬─────────────────┘
         │
         │ 2. Call IAuthService.LoginAsync()
         ▼
┌──────────────────────────┐
│ AuthService.LoginAsync() │
├──────────────────────────┤
│ • Hash password (MD5)    │
│ • Query DB bằng Dapper  │
│ • Get UserId, TenantId  │
│ • Create JWT Token      │
│ • Return LoginResponse  │
└────────┬─────────────────┘
         │
         │ 3. HTTP 200 OK
         │    {
         │      "token": "eyJhbGc...",
         │      "fullName": "John Doe",
         │      "role": "Admin"
         │    }
         ▼
┌─────────────────┐
│ Client stores   │
│ token in        │
│ localStorage    │
└─────────────────┘
```

### 2️⃣ **Lấy danh sách Funds (với multi-tenancy)**

```
┌─────────────────┐
│  WinForms/Web   │
└────────┬────────┘
         │ 1. GET /api/funds
         │    Header: Authorization: Bearer {token}
         ▼
┌──────────────────────────────┐
│ JWT Middleware               │
├──────────────────────────────┤
│ • Validate token signature   │
│ • Extract claims             │
│ • Store in User.Claims       │
└────────┬─────────────────────┘
         │
         │ 2. TenantService.GetCurrentTenantId()
         │    Extract tenantId claim từ JWT
         ▼
┌──────────────────────────┐
│ FundsController.GetFunds()│
└────────┬─────────────────┘
         │
         │ 3. Call IFundService.GetAllFundsAsync()
         ▼
┌──────────────────────────┐
│ FundService              │
├──────────────────────────┤
│ • Query AppDbContext    │
│ • Global filter auto    │
│   WHERE TenantId = @id  │ ← Automatic!
│ • Return FundDto list   │
└────────┬─────────────────┘
         │
         │ 4. AppDbContext
         │    ↓
         │    SQL Server Database
         │    SELECT * FROM Funds
         │    WHERE TenantId = 'tenant-123'
         │    AND IsActive = 1
         ▼
┌──────────────────────────┐
│ HTTP 200 OK              │
│ [                        │
│   {                      │
│     "id": "...",        │
│     "fundName": "Cash",  │
│     "balance": 50000,    │
│     ...                  │
│   }                      │
│ ]                        │
└────────┬─────────────────┘
         │
         ▼
┌─────────────────┐
│ Client displays │
│ funds in table  │
└─────────────────┘
```

### 3️⃣ **Tạo Fund mới (với auto TenantId injection)**

```
┌─────────────────┐
│  WinForms/Web   │
└────────┬────────┘
         │ 1. POST /api/funds
         │    Body: {
         │      "fundName": "Vietcombank",
         │      "fundType": "BANK",
         │      "accountNumber": "0123456789",
         │      "initialBalance": 100000
         │    }
         ▼
┌──────────────────────────┐
│ FundsController.Create() │
└────────┬─────────────────┘
         │
         │ 2. Call IFundService.CreateFundAsync()
         ▼
┌──────────────────────────┐
│ FundService              │
├──────────────────────────┤
│ • Tạo Fund entity       │
│ • Set properties        │
│ • NOT set TenantId yet! │
│ • Add vào context       │
│ • Call SaveChangesAsync │
└────────┬─────────────────┘
         │
         │ 3. AppDbContext.SaveChangesAsync()
         │    ProcessChangeTracker()
         │    ├─ Get TenantId từ JWT: "tenant-123"
         │    ├─ Inject vào entity: fund.TenantId = "tenant-123"
         │    ├─ Inject CreatedAt, CreatedBy
         │    ├─ Tạo AuditLog entry
         │    └─ Save toàn bộ vào DB
         ▼
┌──────────────────────┐
│ SQL Server           │
├──────────────────────┤
│ INSERT Funds (...)   │
│ INSERT AuditLogs (...│ ← Auto
└────────┬─────────────┘
         │
         │ 4. HTTP 201 Created
         │    Location: /api/funds/{id}
         │    Body: {
         │      "id": "550e8400...",
         │      "fundName": "Vietcombank",
         │      "tenantId": "tenant-123", ← Auto injected!
         │      "currentBalance": 100000,
         │      "createdAt": "2024-01-15T10:30:00Z",
         │      "createdBy": "user-123"
         │    }
         ▼
┌─────────────────┐
│ Client receives │
│ new fund data   │
└─────────────────┘
```

## 🎯 Cách thêm Module mới

### Ví dụ: Thêm Users Module

#### Step 1: Tạo folder structure
```powershell
Modules/Users/
├── Controllers/
├── DTOs/
├── Services/
└── Interfaces/
```

#### Step 2: Tạo các file cần thiết
```
Users/DTOs/UserDTOs.cs
Users/Interfaces/IUserService.cs
Users/Services/UserService.cs
Users/Controllers/UsersController.cs
```

#### Step 3: Đăng ký trong `ModuleServiceExtensions.cs`
```csharp
public static IServiceCollection AddModuleServices(this IServiceCollection services)
{
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<IFundService, FundService>();
    services.AddScoped<IUserService, UserService>(); // ← Add here
    return services;
}
```

#### Step 4: Sử dụng
```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }
}
```

## 🔌 Integration với WinForms

### ApiClient (trong CashFlow.WinForms)

```csharp
public class ApiClient
{
    private const string BaseUrl = "https://localhost:5001/api";
    private string _token;

    // Login
    public async Task<LoginResponse> LoginAsync(string username, string password)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{BaseUrl}/auth/login",
            new { username, password }
        );
        
        var data = await response.Content.ReadAsAsync<LoginResponse>();
        _token = data.Token;
        
        return data;
    }

    // Lấy Funds
    public async Task<List<FundDto>> GetFundsAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/funds");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        
        var response = await _httpClient.SendAsync(request);
        return await response.Content.ReadAsAsync<List<FundDto>>();
    }

    // Tạo Fund
    public async Task<FundDto> CreateFundAsync(CreateFundRequest request)
    {
        var httpRequest = new HttpRequestMessage(
            HttpMethod.Post,
            $"{BaseUrl}/funds"
        );
        httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        httpRequest.Content = new StringContent(
            JsonConvert.SerializeObject(request),
            Encoding.UTF8,
            "application/json"
        );
        
        var response = await _httpClient.SendAsync(httpRequest);
        return await response.Content.ReadAsAsync<FundDto>();
    }
}
```

### WinForms Usage

```csharp
public partial class FundsForm : Form
{
    private ApiClient _apiClient = new ApiClient();

    private async void LoadFunds()
    {
        try
        {
            var funds = await _apiClient.GetFundsAsync();
            dataGridViewFunds.DataSource = funds;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi: {ex.Message}");
        }
    }

    private async void btnCreate_Click(object sender, EventArgs e)
    {
        var request = new CreateFundRequest
        {
            FundName = txtFundName.Text,
            FundType = cmbType.SelectedItem.ToString(),
            AccountNumber = txtAccount.Text,
            InitialBalance = decimal.Parse(txtBalance.Text)
        };

        var fund = await _apiClient.CreateFundAsync(request);
        MessageBox.Show($"Tạo quỹ thành công: {fund.Id}");
        LoadFunds();
    }
}
```

## 📊 Multi-Tenancy Workflow

```
Company A (TenantId: ABC...)
├─ User: John
│   └─ Login
│   └─ Token: { tenantId: ABC, userId: john }
│   └─ GET /api/funds
│   └─ Result: Chỉ Funds của Company A

Company B (TenantId: DEF...)
├─ User: Alice
│   └─ Login
│   └─ Token: { tenantId: DEF, userId: alice }
│   └─ GET /api/funds
│   └─ Result: Chỉ Funds của Company B

Database (Shared):
├─ Fund 1: TenantId: ABC
├─ Fund 2: TenantId: ABC
├─ Fund 3: TenantId: DEF
└─ Fund 4: TenantId: DEF

Tự động: Global Query Filter WHERE TenantId = @currentTenantId
```

## ✨ Lợi ích của cấu trúc này

✅ **Feature-based**: Dễ dàng thêm feature mới  
✅ **Modular**: Mỗi module độc lập, tái sử dụng được  
✅ **Maintainable**: Code dễ hiểu, dễ bảo trì  
✅ **Scalable**: Cấu trúc hỗ trợ scaling  
✅ **Testable**: Dễ viết unit tests  
✅ **Multi-tenant**: Tự động tenant isolation  

## 🚀 Next Steps

1. ✅ Xây dựng cấu trúc Modules (DONE)
2. ⬜ Thêm Users Module
3. ⬜ Thêm Roles Module
4. ⬜ Thêm Transactions Module
5. ⬜ Tối ưu hóa API endpoints
6. ⬜ Thêm unit tests
7. ⬜ Deploy to production

---

**Project ready for feature development!** 🎉
