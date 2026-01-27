```markdown
# CashFlow Pro - Hệ thống Quản lý Dòng tiền Doanh nghiệp (SaaS)

![.NET 8](https://img.shields.io/badge/.NET-8.0-purple)
![SQL Server](https://img.shields.io/badge/Database-SQL%20Server-red)
![Architecture](https://img.shields.io/badge/Architecture-Client%2FServer-blue)

**CashFlow Pro** là giải pháp phần mềm quản lý thu chi tiền mặt tập trung dành cho các doanh nghiệp vừa và nhỏ có nhiều chi nhánh. Hệ thống được xây dựng theo mô hình **SaaS (Multi-tenant)**, đảm bảo tính bảo mật, cô lập dữ liệu và hiệu năng cao.

## 🚀 Tính năng nổi bật

* **Multi-tenancy:** Hỗ trợ nhiều doanh nghiệp sử dụng trên cùng một hệ thống nhưng dữ liệu độc lập tuyệt đối.
* **Quản lý Thu/Chi:** Lập phiếu thu, phiếu chi, tự động cập nhật số dư các quỹ (Tiền mặt, Ngân hàng).
* **Phân quyền động (RBAC):** Quản lý quyền hạn chi tiết đến từng chức năng (Matrix Permission).
* **Bảo mật cao:** Xác thực qua **JWT Token**, mật khẩu mã hóa MD5/SHA.
* **Audit Logging:** Truy vết mọi thay đổi dữ liệu (Ai làm? Sửa cái gì? Lúc nào?) để chống gian lận.
* **Báo cáo:** Thống kê dòng tiền, tồn quỹ theo thời gian thực.

## 🛠️ Công nghệ sử dụng

### Backend (Server)
* **Framework:** ASP.NET Core Web API (.NET 8.0)
* **Database Access:** ADO.NET & Dapper (High Performance)
* **Authentication:** JWT Bearer Token
* **Documentation:** Swagger / OpenAPI

### Frontend (Client)
* **Framework:** Windows Forms (.NET 8.0)
* **Communication:** HttpClient (RESTful API interaction)

### Database
* **System:** Microsoft SQL Server
* **Architecture:** Shared Database, Shared Schema (TenantId Isolation)

## 📂 Cấu trúc dự án

```text
CashFlow.Solution/
├── CashFlow.API/           # Backend: Xử lý Logic, Auth, kết nối DB
│   ├── Controllers/        # API Endpoints
│   ├── Services/           # ADO.NET/Dapper Logic
│   └── appsettings.json    # Cấu hình DB & JWT
├── CashFlow.WinForms/      # Frontend: Giao diện người dùng
│   ├── Forms/              # Các màn hình (Login, Dashboard...)
│   └── Services/           # Gọi API từ Client
└── CashFlow.Shared/        # Class dùng chung (DTOs, Enums)

```

## ⚙️ Hướng dẫn cài đặt & Chạy dự án

### 1. Cấu hình Cơ sở dữ liệu

1. Mở SQL Server Management Studio (SSMS).
2. Chạy file script `Database.sql` (nằm trong thư mục `docs/` hoặc root) để tạo Database và các bảng.
3. Script đã bao gồm dữ liệu mẫu (Seed Data):
* **User:** `admin`
* **Pass:** `123456`



### 2. Cấu hình Backend (API)

1. Mở `CashFlow.Solution` bằng Visual Studio 2022.
2. Mở file `CashFlow.API/appsettings.json`.
3. Cập nhật **ConnectionStrings** cho phù hợp với máy của bạn:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=CashFlowDB;Trusted_Connection=True;TrustServerCertificate=True;"
}

```


4. Chuột phải vào project `CashFlow.API` -> chọn **Set as Startup Project**.
5. Nhấn **F5** để chạy. Trình duyệt sẽ mở trang **Swagger UI** (`https://localhost:xxxx/swagger`).

### 3. Cấu hình Frontend (WinForms)

*Lưu ý: Đảm bảo API đang chạy trước khi bật WinForms.*

1. Mở file cấu hình trong `CashFlow.WinForms` (thường là `Program.cs` hoặc file Constant).
2. Sửa đường dẫn `BaseUrl` trỏ về địa chỉ API đang chạy (ví dụ `https://localhost:7152`).
3. Chuột phải vào Solution -> **Properties** -> **Multiple startup projects**:
* API: **Start**
* WinForms: **Start**



## 🧪 Hướng dẫn Test API (Swagger)

1. Truy cập API `/api/Auth/login`.
2. Nhập thông tin mẫu:
```json
{
  "username": "admin",
  "password": "123456"
}

```


3. Copy chuỗi **Token** trả về.
4. Nhấn nút **Authorize** (ổ khóa) trên Swagger -> Nhập: `Bearer <dán_token_vào_đây>`.
5. Thử nghiệm các API khác như `GET /api/Transaction`.

## 📝 Nhật ký thay đổi (Changelog)

### Giai đoạn 1 (Hiện tại)

* [x] Khởi tạo cấu trúc Solution 3-Tier.
* [x] Thiết kế Database Multi-tenant (Tenants, Users, Transactions...).
* [x] Xây dựng API Login & JWT Authentication.
* [x] Tích hợp Swagger & CORS.

### Giai đoạn 2 (Đang triển khai)

* [ ] API CRUD nghiệp vụ Thu/Chi (Transaction).
* [ ] Xử lý SQL Transaction cập nhật số dư quỹ.
* [ ] Giao diện WinForms: Login, Danh sách phiếu.

## 🤝 Đóng góp

Dự án được phát triển phục vụ mục đích học tập môn Lập trình Quản lý.

## 📞 Liên hệ

* **Developer:** Phi Hùng
* **Email:** (Email của bạn)

```

---

### Mẹo nhỏ để README đẹp hơn trên Github/Gitlab:
Nếu sau này bạn đẩy code lên Git, bạn có thể chụp ảnh màn hình giao diện Swagger hoặc sơ đồ ERD Database rồi chèn vào file README bằng cú pháp:

```markdown
![Sơ đồ Database](./docs/erd-diagram.png)

```