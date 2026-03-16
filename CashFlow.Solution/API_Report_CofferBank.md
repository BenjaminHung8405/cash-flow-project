# Báo Cáo API Hệ Thống Cơ Sở Dữ Liệu Tập Trung (CofferBank)

## 1. Tổng quan Kiến trúc Hệ thống

### 1.1 CQRS-lite (Command Query Responsibility Segregation - Phiên bản rút gọn)
Hệ thống áp dụng mô hình CQRS-lite với triết lý tối ưu hóa tốc độ và quản lý rủi ro trên CSDL:
- **Write Operations (Command)**: Được xử lý bằng **Entity Framework Core** (`AppDbContext`). Điểm mạnh là có thể dễ dàng tận dụng tính năng theo dõi sự thay đổi dòng dữ liệu (Change Tracker) để tạo Audit Trail một cách hoàn toàn tự động, đảm bảo mọi chi tiết (INSERT, UPDATE, DELETE) đều được log đầy đủ (với `OldValues`, `NewValues`). Các thay đổi cũng được đảm bảo tính toàn vẹn thông qua các Transaction (`BeginTransactionAsync`).
- **Read Operations (Query)**: Được xử lý bằng **Dapper** (qua `SqlConnection`). Dapper giúp phân tích truy vấn nhanh do không tốn chi phí của Change Tracker, truy vấn trực tiếp SQL cho các tác vụ lấy dữ liệu (nhất là trong dashboard hoặc load đồ thị, danh sách).

### 1.2 Multi-tenancy
Chiến lược **Shared Database, Shared Schema** được áp dụng (1 Database duy nhất, các bản ghi đều chia chung bảng).
- Mọi bảng quan trọng đều kế thừa `BaseTenantEntity` chứa trường `TenantId` (Guid).
- Cơ chế bảo mật ngầm định qua EF Core là **Global Query Filters** (`HasQueryFilter(e => e.TenantId == CurrentTenantId)`), đảm bảo các tenant sẽ không bao giờ truy vấn nhầm sang dữ liệu của nhau ở các thao tác Read/Write thông qua ORM. Đối với Dapper ở phía Read, `TenantId = @TenantId` được truyền thủ công vào câu lệnh SQL một cách kiên cố.

---

## 2. Danh sách các API Endpoints

### Auth APIs / User Management
| Method | Route                       | Role        | Chức năng                                                 |
|--------|-----------------------------|-------------|---------------------------------------------------------|
| POST   | `/api/Auth/register`        | *Anonymous* | Đăng ký mới công ty/tenant và tạo tài khoản Admin.       |
| POST   | `/api/Auth/login`           | *Anonymous* | Xác thực và trả về JWT Bearer Token.                      |
| GET    | `/api/Auth/me`              | *All*       | Lấy thông tin user hiện tại (Dapper Query).              |

### Fund APIs (Quỹ)
| Method | Route                       | Role        | Chức năng                                                 |
|--------|-----------------------------|-------------|---------------------------------------------------------|
| GET    | `/api/Funds`                | *All*       | Lấy danh sách toàn bộ quỹ đang Active (Dapper Query).      |
| POST   | `/api/Funds`                | *All*       | Tạo quỹ (EF Core).                                        |
| PUT    | `/api/Funds/{id}`           | *All*       | Cập nhật thông tin quỹ (EF Core).                         |
| DELETE | `/api/Funds/{id}`           | *All*       | Xóa quỹ - Soft Delete (EF Core).                          |

### Transaction APIs (Giao dịch)
| Method | Route                       | Role        | Chức năng                                                 |
|--------|-----------------------------|-------------|---------------------------------------------------------|
| POST   | `/api/Transactions`         | *All*       | Lập giao dịch IN/OUT (EF Core & DB Transaction).      |
| GET    | `/api/Transactions/recent`  | *All*       | Lấy các giao dịch gần đây nhất (Dapper Query).            |
| GET    | `/api/Transactions/donut-chart` | *All*   | Dữ liệu vẽ biểu đồ Chi tiêu tổng hợp theo hệ Category Group (Dapper). |

---

## 3. Payload & Response mẫu cho từng API quan trọng

### 3.1 Register
**Cấu trúc dữ liệu yêu cầu tạo Tenant, Admin Role và User.**

*Endpoint:* `POST /api/Auth/register`

- **Request Payload:**
```json
{
  "username": "admin_test",
  "password": "SecurePassword123!",
  "companyName": "Công ty TNHH Dịch Vụ CofferBank",
  "email": "admin@cofferbank.local"
}
```
- **Response: `200 OK`**
```json
{
  "message": "Đăng ký thành công",
  "data": {
    "userId": "936081bb-6bd0-47e1-bbfd-eac4f971ba48",
    "username": "admin_test",
    "companyName": "Công ty TNHH Dịch Vụ CofferBank",
    "email": "admin@cofferbank.local",
    "role": "Admin",
    "tenantId": "e0e29202-a16f-47dc-98df-82db2f0bdf2a"
  }
}
```

### 3.2 Login
*Endpoint:* `POST /api/Auth/login`

- **Request Payload:**
```json
{
  "username": "admin_test",
  "password": "SecurePassword123!"
}
```
- **Response: `200 OK`**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "companyName": "Công ty TNHH Dịch Vụ CofferBank",
  "role": "Admin",
  "tenantId": "e0e29202-a16f-47dc-98df-82db2f0bdf2a"
}
```

### 3.3 Create Transaction
**Giao dịch Chi tiêu (OUT). Hệ thống tự làm phép trừ ở số dư Fund trong Transaction scope.**

*Endpoint:* `POST /api/Transactions`

- **Request Payload:**
```json
{
  "voucherCode": "PT-001092",
  "transactionType": "OUT",
  "amount": 2500000.00,
  "fundId": "48baf77e-deed-4b2a-aecf-fb8a478b056e",
  "description": "Chi mua thiết bị văn phòng",
  "categoryName": "OPEX (Chi phí vận hành)"
}
```
- **Response: `200 OK`**
```json
{
  "message": "Transaction created successfully",
  "data": {
    "id": "2af744af-814d-46e3-80b1-3e440cd2faeb",
    "voucherCode": "PT-001092",
    "transactionType": "OUT",
    "amount": 2500000.00,
    "fundId": "48baf77e-deed-4b2a-aecf-fb8a478b056e",
    "fundName": "Tài khoản ACB Chi nhánh HCM",
    "description": "Chi mua thiết bị văn phòng",
    "categoryName": "OPEX (Chi phí vận hành)",
    "transactionDate": "2026-03-20T10:45:00Z"
  }
}
```

### 3.4 Get Dashboard Data (Donut Chart)
*Endpoint:* `GET /api/Transactions/donut-chart`

- **Response: `200 OK`**
```json
[
  {
    "categoryName": "OPEX (Chi phí vận hành)",
    "totalAmount": 2500000.00
  },
  {
    "categoryName": "Tiền lương",
    "totalAmount": 15000000.00
  }
]
```

---

## 4. Cơ chế Bảo mật & Truy vết

### 4.1 Cơ chế Authorization & Khóa API
- **JWT Bearer Token**: Mọi Rest API (Ngoại trừ Login & Register) đều được khóa dưới scope `[Authorize]`. JWT Claims chứa các thông tin như `TenantId`, `Role` và `UserId` ở dạng payload. `TenantService` sẽ chắt lọc Guid của Tenant thông qua JWT claim, đảm bảo môi trường sandbox cho người dùng.
- **Role-Based Access Control (RBAC)**: Với tính năng mở rộng quản lý, endpoint đặc quyền trên bộ API được khoanh vùng bởi `[Authorize(Roles = "Admin")]`.
- **Password Hashing**: Quá trình khởi tạo và kiểm chứng được băm dữ liệu thông qua thư viện `BCrypt.Net-Next` cho luồng Login và Register, phòng tránh lộ dữ liệu plain-text vào DB CSDL.

### 4.2 Truy vết toàn cục (Audit Logging)
Dựa vào hạt nhân EF Core, CSDL bắt sự kiện `SaveChanges/SaveChangesAsync` và trích xuất EntityEntry:
- Tự động detect Entity là: Inserted, Modified hay Deleted.
- Chỉ theo dấu các Object kế thừa `BaseTenantEntity` và miễn trừ bản thân bảng `AuditLog`.
- Khóa toàn bộ Snapshot (dưới dạng chuỗi JSON `OldValues` và `NewValues`). Qua đó chủ doanh nghiệp (Admin) có thể View Log về dòng CSDL cụ thể nào đang bị chỉnh sửa. 
