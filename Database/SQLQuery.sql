CREATE DATABASE CashFlowDB;
GO
USE CashFlowDB;
GO

-- 1. Bảng Doanh nghiệp (Tenant) - Khách hàng thuê phần mềm
CREATE TABLE Tenants (
    TenantId INT PRIMARY KEY IDENTITY(1,1),
    TenantCode VARCHAR(50) NOT NULL UNIQUE, -- Mã cty (VD: FPT, VIN)
    TenantName NVARCHAR(255) NOT NULL,
    TaxCode VARCHAR(50),
    PhoneNumber VARCHAR(20),
    IsActive BIT DEFAULT 1,                 -- 1: Hoạt động, 0: Bị khóa
    ExpireDate DATETIME,                    -- Ngày hết hạn gói cước
    CreatedDate DATETIME DEFAULT GETDATE()
);

-- 2. Bảng Chi nhánh (Store/Branch)
CREATE TABLE Branches (
    BranchId INT PRIMARY KEY IDENTITY(1,1),
    TenantId INT NOT NULL,                  -- Thuộc công ty nào
    BranchName NVARCHAR(255) NOT NULL,
    Address NVARCHAR(500),
    Phone VARCHAR(20),
    IsActive BIT DEFAULT 1,
    FOREIGN KEY (TenantId) REFERENCES Tenants(TenantId)
);

-- 3. Bảng Nhóm quyền (Roles)
CREATE TABLE Roles (
    RoleId INT PRIMARY KEY IDENTITY(1,1),
    TenantId INT NOT NULL,
    RoleName NVARCHAR(100) NOT NULL,        -- VD: Kế toán trưởng, Thu ngân
    Description NVARCHAR(255),
    FOREIGN KEY (TenantId) REFERENCES Tenants(TenantId)
);

-- 4. Bảng Người dùng (Users)
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    TenantId INT NOT NULL,
    BranchId INT NULL,                      -- Null nếu là Giám đốc (xem all)
    RoleId INT NOT NULL,
    Username VARCHAR(50) NOT NULL,
    PasswordHash VARCHAR(500) NOT NULL,     -- Lưu mật khẩu đã mã hóa
    FullName NVARCHAR(100),
    Email VARCHAR(100),
    IsActive BIT DEFAULT 1,
    FOREIGN KEY (TenantId) REFERENCES Tenants(TenantId),
    FOREIGN KEY (BranchId) REFERENCES Branches(BranchId),
    FOREIGN KEY (RoleId) REFERENCES Roles(RoleId)
);

-- 5. Bảng Danh sách Chức năng (Permissions - Static Data)
CREATE TABLE Permissions (
    PermissionCode VARCHAR(50) PRIMARY KEY, -- VD: TRANS_VIEW, TRANS_CREATE
    PermissionName NVARCHAR(100),
    GroupName NVARCHAR(50)                  -- VD: Quản lý Thu Chi
);

-- 6. Bảng Phân quyền (Role-Permission Matrix)
CREATE TABLE RolePermissions (
    RoleId INT NOT NULL,
    PermissionCode VARCHAR(50) NOT NULL,
    PRIMARY KEY (RoleId, PermissionCode),
    FOREIGN KEY (RoleId) REFERENCES Roles(RoleId),
    FOREIGN KEY (PermissionCode) REFERENCES Permissions(PermissionCode)
);

-- 7. Bảng Quỹ tiền (CashFunds) - Ví dụ: Két sắt, Vietcombank...
CREATE TABLE CashFunds (
    FundId INT PRIMARY KEY IDENTITY(1,1),
    TenantId INT NOT NULL,
    BranchId INT NOT NULL,
    FundName NVARCHAR(100) NOT NULL,
    AccountNumber VARCHAR(50),              -- Số tài khoản ngân hàng (nếu có)
    Balance DECIMAL(18, 2) DEFAULT 0,       -- Số dư hiện tại (Quan trọng)
    FOREIGN KEY (TenantId) REFERENCES Tenants(TenantId),
    FOREIGN KEY (BranchId) REFERENCES Branches(BranchId)
);

-- 8. Bảng Danh mục Thu/Chi (Categories)
CREATE TABLE TransactionCategories (
    CategoryId INT PRIMARY KEY IDENTITY(1,1),
    TenantId INT NOT NULL,
    CategoryName NVARCHAR(100) NOT NULL,    -- VD: Tiền điện, Tiền hàng
    Type VARCHAR(10) NOT NULL,              -- 'IN' (Thu) hoặc 'OUT' (Chi)
    FOREIGN KEY (TenantId) REFERENCES Tenants(TenantId)
);

-- 9. Bảng Đối tượng (Partners) - Khách hàng, NCC
CREATE TABLE Partners (
    PartnerId INT PRIMARY KEY IDENTITY(1,1),
    TenantId INT NOT NULL,
    PartnerName NVARCHAR(255) NOT NULL,
    Phone VARCHAR(20),
    Address NVARCHAR(500),
    Type VARCHAR(20),                       -- 'CUSTOMER', 'SUPPLIER', 'EMPLOYEE'
    InitialDebt DECIMAL(18, 2) DEFAULT 0,   -- Nợ đầu kỳ
    FOREIGN KEY (TenantId) REFERENCES Tenants(TenantId)
);

-- 10. Bảng Giao dịch Thu/Chi (Transactions - Bảng Core)
CREATE TABLE Transactions (
    TransId BIGINT PRIMARY KEY IDENTITY(1,1),
    TenantId INT NOT NULL,
    BranchId INT NOT NULL,
    FundId INT NOT NULL,                    -- Tiền ra/vào quỹ nào
    CategoryId INT NOT NULL,                -- Lý do gì
    PartnerId INT NULL,                     -- Giao dịch với ai (Có thể Null)
    
    TransDate DATETIME DEFAULT GETDATE(),
    Amount DECIMAL(18, 2) NOT NULL,         -- Số tiền
    Description NVARCHAR(500),              -- Diễn giải
    TransType VARCHAR(10) NOT NULL,         -- 'IN' hoặc 'OUT'
    RefNo VARCHAR(50),                      -- Số hóa đơn gốc
    
    CreatedBy INT NOT NULL,                 -- User tạo phiếu
    Status VARCHAR(20) DEFAULT 'COMPLETED', -- 'PENDING', 'COMPLETED', 'REJECTED'
    
    FOREIGN KEY (TenantId) REFERENCES Tenants(TenantId),
    FOREIGN KEY (BranchId) REFERENCES Branches(BranchId),
    FOREIGN KEY (FundId) REFERENCES CashFunds(FundId),
    FOREIGN KEY (CategoryId) REFERENCES TransactionCategories(CategoryId),
    FOREIGN KEY (PartnerId) REFERENCES Partners(PartnerId),
    FOREIGN KEY (CreatedBy) REFERENCES Users(UserId)
);

-- 11. Bảng Chứng từ đính kèm
CREATE TABLE TransactionAttachments (
    AttachmentId BIGINT PRIMARY KEY IDENTITY(1,1),
    TransId BIGINT NOT NULL,
    FileName NVARCHAR(255),
    FilePath NVARCHAR(MAX),                 -- Đường dẫn file ảnh
    UploadedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (TransId) REFERENCES Transactions(TransId) ON DELETE CASCADE
);

-- 12. Bảng Nhật ký truy vết (AuditLogs)
CREATE TABLE AuditLogs (
    LogId BIGINT PRIMARY KEY IDENTITY(1,1),
    TenantId INT NOT NULL,
    UserId INT,
    UserName NVARCHAR(100),
    ActionType VARCHAR(20),                 -- 'INSERT', 'UPDATE', 'DELETE'
    TableName VARCHAR(50),
    RecordId VARCHAR(50),
    OldValues NVARCHAR(MAX),                -- JSON dữ liệu cũ
    NewValues NVARCHAR(MAX),                -- JSON dữ liệu mới
    ActionDate DATETIME DEFAULT GETDATE()
);

CREATE INDEX IDX_Transactions_Tenant ON Transactions(TenantId, TransDate);
CREATE INDEX IDX_CashFunds_Tenant ON CashFunds(TenantId);
CREATE INDEX IDX_Partners_Tenant ON Partners(TenantId);
CREATE INDEX IDX_AuditLogs_Tenant ON AuditLogs(TenantId, ActionDate);

-- 1. Tạo 1 Công ty mẫu
INSERT INTO Tenants (TenantCode, TenantName, IsActive) 
VALUES ('DEMO', N'Công ty Demo CashFlow', 1);

DECLARE @TenantId INT = SCOPE_IDENTITY(); -- Lấy ID vừa tạo

-- 2. Tạo 1 Chi nhánh
INSERT INTO Branches (TenantId, BranchName, Address) 
VALUES (@TenantId, N'Chi nhánh Hà Nội', N'123 Cầu Giấy');

DECLARE @BranchId INT = SCOPE_IDENTITY();

-- 3. Tạo Role Admin
INSERT INTO Roles (TenantId, RoleName) VALUES (@TenantId, 'Admin');
DECLARE @RoleId INT = SCOPE_IDENTITY();

-- 4. Tạo User Admin (Pass: 123456 - Hash demo)
INSERT INTO Users (TenantId, BranchId, RoleId, Username, PasswordHash, FullName)
VALUES (@TenantId, @BranchId, @RoleId, 'admin', 'e10adc3949ba59abbe56e057f20f883e', N'Nguyễn Văn A');

-- 5. Tạo Quỹ tiền mặt
INSERT INTO CashFunds (TenantId, BranchId, FundName, Balance)
VALUES (@TenantId, @BranchId, N'Tiền mặt tại két', 10000000);

-- 6. Tạo Danh mục
INSERT INTO TransactionCategories (TenantId, CategoryName, Type) VALUES (@TenantId, N'Thu bán hàng', 'IN');
INSERT INTO TransactionCategories (TenantId, CategoryName, Type) VALUES (@TenantId, N'Tiền điện nước', 'OUT');

GO
CREATE TRIGGER trg_Audit_Transactions_Auto
ON Transactions
AFTER UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ActionType VARCHAR(20);
    
    IF EXISTS (SELECT * FROM inserted) SET @ActionType = 'UPDATE';
    ELSE SET @ActionType = 'DELETE';

    INSERT INTO AuditLogs (TenantId, UserId, UserName, ActionType, TableName, RecordId, OldValues, NewValues, ActionDate)
    SELECT 
        d.TenantId,
        d.CreatedBy, 
        'System Trigger',
        @ActionType,
        'Transactions',
        CAST(d.TransId AS VARCHAR(50)),
        (SELECT d.Amount, d.Description, d.TransDate, d.Status FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), -- Cũ
        CASE WHEN @ActionType = 'UPDATE' 
             THEN (SELECT i.Amount, i.Description, i.TransDate, i.Status FROM inserted i WHERE i.TransId = d.TransId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
             ELSE NULL END, -- Mới
        GETDATE()
    FROM deleted d;
END;
GO