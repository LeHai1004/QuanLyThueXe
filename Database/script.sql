-- ============================================================
--  DATABASE: CAR RENTAL MANAGEMENT SYSTEM
--  Platform: SQL Server 2019+
--  Encoding: UTF-8
--  Gom: 16 tables + triggers + views + stored procedures + sample data
-- ============================================================

USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'QuanLyThueXe')
BEGIN
    ALTER DATABASE QuanLyThueXe SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QuanLyThueXe;
END
GO

CREATE DATABASE QuanLyThueXe
    COLLATE Vietnamese_CI_AS;
GO

USE QuanLyThueXe;
GO

SET QUOTED_IDENTIFIER ON;
GO

-- ============================================================
-- TABLE 1: VAITRO
-- ============================================================
CREATE TABLE Role (
    RoleId INT PRIMARY KEY IDENTITY(1,1),
    TenRole NVARCHAR(50) NOT NULL UNIQUE,
    Description NVARCHAR(200) NULL
);
GO

-- ============================================================
-- TABLE 2: TAIKHOAN
-- ============================================================
CREATE TABLE Account (
    AccountId INT PRIMARY KEY IDENTITY(1,1),
    RoleId INT NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Account_Role FOREIGN KEY (RoleId) REFERENCES Role(RoleId)
);
GO

-- ============================================================
-- TABLE 3: NGUOIDUNG (ĐÃ SỬA ĐỊA CHỈ)
-- ============================================================
CREATE TABLE UserProfile (
    UserProfileId INT PRIMARY KEY IDENTITY(1,1),
    AccountId INT NOT NULL UNIQUE,
    FullName NVARCHAR(150) NOT NULL,
    PhoneNumber NVARCHAR(15) NULL,
    Gender NVARCHAR(10) NULL,
    DateOfBirth DATE NULL,
    
    -- ĐỊA CHỈ ĐÃ TÁCH THEO 1NF
    StreetAddress NVARCHAR(200) NULL,
    Ward NVARCHAR(100) NULL,
    District NVARCHAR(100) NULL,
    City NVARCHAR(100) NULL,
    
    AvatarUrl NVARCHAR(300) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_UserProfile_Account FOREIGN KEY (AccountId) REFERENCES Account(AccountId)
);
GO

-- ============================================================
-- TABLE 4: KHACHHANG (mo rong tu UserProfile)
-- ============================================================
CREATE TABLE Customer (
    CustomerId INT PRIMARY KEY IDENTITY(1,1),
    UserProfileId INT NOT NULL UNIQUE,
    NationalId NVARCHAR(20) NULL,
    NationalIdFrontImg NVARCHAR(300) NULL,
    NationalIdBackImg NVARCHAR(300) NULL,
    LicenseNumber NVARCHAR(20) NULL,
    LicenseClass NVARCHAR(10) NULL,
    LicenseIssueDate DATE NULL,
    LicenseExpiryDate DATE NULL,
    TotalSpent DECIMAL(18,2) NOT NULL DEFAULT 0,
    TotalRentals INT NOT NULL DEFAULT 0,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Customer_UserProfile FOREIGN KEY (UserProfileId) REFERENCES UserProfile(UserProfileId)
);
GO

-- ============================================================
-- TABLE 5: NHANVIEN (mo rong tu UserProfile)
-- ============================================================
CREATE TABLE Staff (
    StaffId            INT             PRIMARY KEY IDENTITY(1,1),
    UserProfileId     INT             NOT NULL UNIQUE,
    StaffCode        NVARCHAR(20)    NOT NULL UNIQUE,  -- 'NV001', 'NV002'
    Position          NVARCHAR(100)   NULL,
    Department        NVARCHAR(100)   NULL,
    Branch        NVARCHAR(150)   NULL,
    HireDate      DATE            NULL,
    IsActive        BIT             NOT NULL DEFAULT 1,
    CONSTRAINT FK_Staff_UserProfile FOREIGN KEY (UserProfileId)
        REFERENCES UserProfile(UserProfileId)
);
GO

-- ============================================================
-- TABLE 6: LOAIXE (danh muc loai xe)
-- ============================================================
CREATE TABLE VehicleCategory (
    CategoryId          INT             PRIMARY KEY IDENTITY(1,1),
    CategoryName         NVARCHAR(100)   NOT NULL,   -- 'Sedan', 'SUV', 'Pickup', 'Van'
    Description            NVARCHAR(300)   NULL,
    IsActive        BIT             NOT NULL DEFAULT 1
);
GO

-- ============================================================
-- TABLE 7: XE (thong tin xe cho thue)
-- ============================================================
CREATE TABLE Vehicle (
    VehicleId            INT             PRIMARY KEY IDENTITY(1,1),
    CategoryId          INT             NOT NULL,
    LicensePlate          NVARCHAR(15)    NOT NULL UNIQUE,
    VehicleName           NVARCHAR(150)   NOT NULL,
    Brand            NVARCHAR(100)   NOT NULL,   -- 'Toyota', 'Honda', 'Mercedes'
    Model           NVARCHAR(100)   NOT NULL,
    ManufactureYear      INT             NOT NULL,
    Color          NVARCHAR(50)    NULL,
    FuelType       NVARCHAR(30)    NULL,       -- 'Xang', 'Dau', 'Dien', 'Hybrid'
    Transmission           NVARCHAR(30)    NULL,       -- 'So tu dong', 'So san'
    Seats           INT             NOT NULL DEFAULT 5,
    PricePerDay     DECIMAL(18,2)   NOT NULL,
    VehicleDesc          NVARCHAR(500)   NULL,
    HinhAnh         NVARCHAR(300)   NULL,
    Status       NVARCHAR(30)    NOT NULL DEFAULT N'San sang',
        -- N'San sang', N'Dang thue', N'Bao duong', N'Ngung hoat dong'
    AverageRating       DECIMAL(3,2)    NOT NULL DEFAULT 0,
    CreatedAt         DATETIME        NOT NULL DEFAULT GETDATE(),
    UpdatedAt     DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Xe_VehicleCategory FOREIGN KEY (CategoryId)
        REFERENCES VehicleCategory(CategoryId),
    CONSTRAINT CK_Xe_Seats      CHECK (Seats BETWEEN 2 AND 50),
    CONSTRAINT CK_Vehicle_Price    CHECK (PricePerDay > 0),
    CONSTRAINT CK_Xe_Review    CHECK (AverageRating BETWEEN 0 AND 5)
);
GO

-- ============================================================
-- TABLE 8: NHACUNGCAP
-- ============================================================
CREATE TABLE Supplier (
    SupplierId           INT             PRIMARY KEY IDENTITY(1,1),
    SupplierName          NVARCHAR(200)   NOT NULL,
    Address          NVARCHAR(300)   NULL,
    PhoneNumber     NVARCHAR(15)    NULL,
    Email           NVARCHAR(150)   NULL,
    ContactPerson     NVARCHAR(150)   NULL,
    TaxCode        NVARCHAR(20)    NULL,
    IsActive        BIT             NOT NULL DEFAULT 1,
    CreatedAt         DATETIME        NOT NULL DEFAULT GETDATE()
);
GO

-- ============================================================
-- TABLE 9: PHIEUNHAP (phieu nhap xe - master)
-- ============================================================
CREATE TABLE ImportReceipt (
    ImportReceiptId     INT             PRIMARY KEY IDENTITY(1,1),
    SoImportReceipt     NVARCHAR(20)    NOT NULL UNIQUE,  -- 'PN2024001'
    SupplierId           INT             NOT NULL,
    StaffId            INT             NOT NULL,   -- NV lap phieu
    ApprovedByStaffId       INT             NULL,       -- Admin/NV duyet
    ImportDate        DATETIME        NOT NULL DEFAULT GETDATE(),
    ApprovalDate       DATETIME        NULL,
    TotalAmount        DECIMAL(18,2)   NOT NULL DEFAULT 0,
    Status       NVARCHAR(30)    NOT NULL DEFAULT N'Cho duyet',
        -- N'Cho duyet', N'Da duyet', N'Tu choi'
    Note          NVARCHAR(500)   NULL,
    CreatedAt         DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_ImportReceipt_NCC      FOREIGN KEY (SupplierId)
        REFERENCES Supplier(SupplierId),
    CONSTRAINT FK_ImportReceipt_NV       FOREIGN KEY (StaffId)
        REFERENCES Staff(StaffId),
    CONSTRAINT FK_ImportReceipt_NVDuyet  FOREIGN KEY (ApprovedByStaffId)
        REFERENCES Staff(StaffId)
);
GO

-- ============================================================
-- TABLE 10: CT_PHIEUNHAP (chi tiet phieu nhap - detail)
-- ============================================================
CREATE TABLE ImportReceiptDetail (
    ImportDetailId          INT             PRIMARY KEY IDENTITY(1,1),
    ImportReceiptId     INT             NOT NULL,
    VehicleId            INT             NOT NULL,
    Quantity         INT             NOT NULL DEFAULT 1,
    UnitPrice          DECIMAL(18,2)   NOT NULL,
    LineTotal       AS (Quantity * UnitPrice) PERSISTED,  -- tu dong tinh
    VehicleCondition     NVARCHAR(100)   NULL,   -- 'Moi 100%', 'Da qua su dung - Tot'
    CurrentKm     INT             NOT NULL DEFAULT 0,
    Note          NVARCHAR(300)   NULL,
    CONSTRAINT FK_CTPN_ImportReceipt FOREIGN KEY (ImportReceiptId)
        REFERENCES ImportReceipt(ImportReceiptId),
    CONSTRAINT FK_ImportReceiptDetail_Vehicle        FOREIGN KEY (VehicleId)
        REFERENCES Vehicle(VehicleId),
    CONSTRAINT CK_CTPN_Quantity  CHECK (Quantity > 0),
    CONSTRAINT CK_CTPN_UnitPrice   CHECK (UnitPrice > 0)
);
GO

-- ============================================================
-- TABLE 11: DATXE (don dat xe cua khach)
-- ============================================================
CREATE TABLE Booking (
    BookingId         INT             PRIMARY KEY IDENTITY(1,1),
    CustomerId            INT             NOT NULL,
    VehicleId            INT             NOT NULL,
    StaffId            INT             NULL,   -- NV tiep nhan (NULL = dat online)
    PickupLocation     NVARCHAR(300)   NOT NULL,
    ReturnLocation      NVARCHAR(300)   NOT NULL,
    PickupDateTime    DATETIME        NOT NULL,
    ReturnDateTime     DATETIME        NOT NULL,
    RentalDays      AS (DATEDIFF(DAY, PickupDateTime, ReturnDateTime)) PERSISTED,
    BasePrice        DECIMAL(18,2)   NOT NULL,
    DiscountAmount        DECIMAL(18,2)   NOT NULL DEFAULT 0,
    ExtraFee     DECIMAL(18,2)   NOT NULL DEFAULT 0,
    TotalAmount        DECIMAL(18,2)   NOT NULL,
    Status       NVARCHAR(30)    NOT NULL DEFAULT N'Cho xac nhan',
        -- N'Cho xac nhan', N'Da xac nhan', N'Dang thue', N'Hoan thanh', N'Da huy'
    BookingChannel         NVARCHAR(20)    NOT NULL DEFAULT N'Online', -- N'Online', N'Tai quay'
    Note          NVARCHAR(500)   NULL,
    CreatedAt         DATETIME        NOT NULL DEFAULT GETDATE(),
    UpdatedAt     DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Booking_Customer FOREIGN KEY (CustomerId)
        REFERENCES Customer(CustomerId),
    CONSTRAINT FK_Booking_Xe        FOREIGN KEY (VehicleId)
        REFERENCES Vehicle(VehicleId),
    CONSTRAINT FK_Booking_Staff  FOREIGN KEY (StaffId)
        REFERENCES Staff(StaffId),
    CONSTRAINT CK_Booking_ThoiGian CHECK (ReturnDateTime > PickupDateTime),
    CONSTRAINT CK_Booking_TotalAmount CHECK (TotalAmount >= 0)
);
GO

-- ============================================================
-- TABLE 12: HOADON (hoa don - master)
-- ============================================================
CREATE TABLE Invoice (
    InvoiceId        INT             PRIMARY KEY IDENTITY(1,1),
    InvoiceNumber        NVARCHAR(20)    NOT NULL UNIQUE,  -- 'HD2024001'
    BookingId         INT             NOT NULL UNIQUE,
    CustomerId            INT             NOT NULL,
    StaffId            INT             NULL,   -- NV lap hoa don
    LoaiInvoice      NVARCHAR(30)    NOT NULL DEFAULT N'Thue xe',
        -- N'Thue xe', N'Dat coc', N'Phat'
    SubTotal   DECIMAL(18,2)   NOT NULL,
    TaxRate        DECIMAL(5,2)    NOT NULL DEFAULT 10, -- 10%
    TaxAmount        AS (ROUND(SubTotal * TaxRate / 100, 0)) PERSISTED,
    DiscountAmount        DECIMAL(18,2)   NOT NULL DEFAULT 0,
    GrandTotal        DECIMAL(18,2)   NOT NULL,
    Status       NVARCHAR(30)    NOT NULL DEFAULT N'Chua thanh toan',
        -- N'Chua thanh toan', N'Da thanh toan', N'Da huy'
    Note          NVARCHAR(500)   NULL,
    IssueDate         DATETIME        NOT NULL DEFAULT GETDATE(),
    DueDate    DATETIME        NULL,
    PaidDate   DATETIME        NULL,
    CONSTRAINT FK_Invoice_Booking FOREIGN KEY (BookingId)
        REFERENCES Booking(BookingId),
    CONSTRAINT FK_Invoice_KH    FOREIGN KEY (CustomerId)
        REFERENCES Customer(CustomerId),
    CONSTRAINT FK_Invoice_NV    FOREIGN KEY (StaffId)
        REFERENCES Staff(StaffId)
);
GO

-- ============================================================
-- TABLE 13: CT_HOADON (chi tiet hoa don - detail)
-- ============================================================
CREATE TABLE InvoiceDetail (
    InvoiceDetailId          INT             PRIMARY KEY IDENTITY(1,1),
    InvoiceId        INT             NOT NULL,
    ItemType       NVARCHAR(50)    NOT NULL,
        -- N'Tien thue xe', N'Phi bao hiem', N'Phi ve sinh',
        -- N'Phi qua gio', N'Phi xang', N'Tien phat'
    Description            NVARCHAR(200)   NOT NULL,
    Quantity         INT             NOT NULL DEFAULT 1,
    UnitPrice          DECIMAL(18,2)   NOT NULL,
    DiscountPercent       DECIMAL(5,2)    NOT NULL DEFAULT 0,
    LineTotal       AS (ROUND(Quantity * UnitPrice * (1 - DiscountPercent / 100), 0)) PERSISTED,
    Note          NVARCHAR(200)   NULL,
    CONSTRAINT FK_CTHD_Invoice FOREIGN KEY (InvoiceId)
        REFERENCES Invoice(InvoiceId),
    CONSTRAINT CK_CTHD_Quantity  CHECK (Quantity > 0),
    CONSTRAINT CK_CTHD_UnitPrice   CHECK (UnitPrice >= 0),
    CONSTRAINT CK_CTHD_DiscountPercent CHECK (DiscountPercent BETWEEN 0 AND 100)
);
GO

-- ============================================================
-- TABLE 14: THANHTOAN
-- ============================================================
CREATE TABLE Payment (
    PaymentId     INT             PRIMARY KEY IDENTITY(1,1),
    InvoiceId        INT             NOT NULL,
    StaffId            INT             NULL,   -- NULL = thanh toan online tu dong
    PaymentMethod      NVARCHAR(50)    NOT NULL,
        -- N'Tien mat', N'Chuyen khoan', N'VNPay', N'Momo', N'The ngan hang'
    TransactionCode      NVARCHAR(100)   NULL,   -- Ma tu cong thanh toan
    Amount          DECIMAL(18,2)   NOT NULL,
    Status       NVARCHAR(20)    NOT NULL DEFAULT N'Thanh cong',
        -- N'Thanh cong', N'That bai', N'Hoan tien'
    Note          NVARCHAR(300)   NULL,
    PaymentTime      DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Payment_Invoice FOREIGN KEY (InvoiceId)
        REFERENCES Invoice(InvoiceId),
    CONSTRAINT FK_Payment_NV     FOREIGN KEY (StaffId)
        REFERENCES Staff(StaffId),
    CONSTRAINT CK_Payment_Amount CHECK (Amount > 0)
);
GO

-- ============================================================
-- TABLE 15: DANHGIA
-- ============================================================
CREATE TABLE Review (
    ReviewId       INT             PRIMARY KEY IDENTITY(1,1),
    BookingId         INT             NOT NULL UNIQUE, -- 1 don chi duoc danh gia 1 lan
    CustomerId            INT             NOT NULL,
    VehicleId            INT             NOT NULL,
    StarRating         INT             NOT NULL,
    Content         NVARCHAR(500)   NULL,
    NgayReview     DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Review_Booking FOREIGN KEY (BookingId)
        REFERENCES Booking(BookingId),
    CONSTRAINT FK_Review_KH    FOREIGN KEY (CustomerId)
        REFERENCES Customer(CustomerId),
    CONSTRAINT FK_Review_Xe    FOREIGN KEY (VehicleId)
        REFERENCES Vehicle(VehicleId),
    CONSTRAINT CK_Review_Diem  CHECK (StarRating BETWEEN 1 AND 5)
);
GO

-- ============================================================
-- TABLE 16: LICHSUBAODUONG
-- ============================================================
CREATE TABLE MaintenanceLog (
    MaintenanceId      INT             PRIMARY KEY IDENTITY(1,1),
    VehicleId            INT             NOT NULL,
    StaffId            INT             NULL,
    MaintenanceType    NVARCHAR(100)   NOT NULL,   -- N'Dinh ky', N'Sua chua', N'Kiem tra'
    Description            NVARCHAR(500)   NULL,
    Cost          DECIMAL(18,2)   NOT NULL DEFAULT 0,
    MaintenanceDate    DATE            NOT NULL,
    MaintenanceDateTiep DATE           NULL,
    Status       NVARCHAR(30)    NOT NULL DEFAULT N'Hoan thanh',
    CONSTRAINT FK_MaintenanceLog_Vehicle  FOREIGN KEY (VehicleId)
        REFERENCES Vehicle(VehicleId),
    CONSTRAINT FK_MaintenanceLog_Staff  FOREIGN KEY (StaffId)
        REFERENCES Staff(StaffId)
);
GO

-- ============================================================
--  TRIGGER: Tu dong cap nhat TotalAmount ImportReceipt
--  khi them/sua/xoa ImportReceiptDetail
-- ============================================================
CREATE TRIGGER trg_UpdateImportReceiptTotal
ON ImportReceiptDetail
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ImportReceiptId INT;

    -- Lay ImportReceiptId tu ban ghi vua thay doi
    SELECT @ImportReceiptId = COALESCE(
        (SELECT TOP 1 ImportReceiptId FROM inserted),
        (SELECT TOP 1 ImportReceiptId FROM deleted)
    );

    UPDATE ImportReceipt
    SET TotalAmount = (
        SELECT ISNULL(SUM(LineTotal), 0)
        FROM ImportReceiptDetail
        WHERE ImportReceiptId = @ImportReceiptId
    )
    WHERE ImportReceiptId = @ImportReceiptId;
END;
GO

-- ============================================================
--  TRIGGER: Tu dong cap nhat AverageRating cua Vehicle
--  khi them danh gia moi
-- ============================================================
CREATE TRIGGER trg_UpdateVehicleAverageRating
ON Review
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @VehicleId INT;

    SELECT @VehicleId = COALESCE(
        (SELECT TOP 1 VehicleId FROM inserted),
        (SELECT TOP 1 VehicleId FROM deleted)
    );

    UPDATE Vehicle
    SET AverageRating = (
        SELECT ISNULL(AVG(CAST(StarRating AS DECIMAL(3,2))), 0)
        FROM Review
        WHERE VehicleId = @VehicleId
    )
    WHERE VehicleId = @VehicleId;
END;
GO

-- ============================================================
--  DU LIEU MAU
-- ============================================================

-- Role
INSERT INTO Role (TenRole, Description) VALUES
(N'Admin',      N'Quan tri vien, toan quyen he thong'),
(N'Staff',   N'Nhan vien xu ly nghiep vu hang ngay'),
(N'Customer',  N'Khach hang su dung dich vu thue xe');
GO

-- Account (mat khau mau: "123456" - trong thuc te phai hash)
INSERT INTO Account (RoleId, Email, PasswordHash) VALUES
(1, N'admin@thuexe.vn',    N'$2a$11$qX0VB7q/tbOQxAk2Kqozku/mxtMxkEPVbShjOrfyqdbaaPvbDC7qS'),
(2, N'nv001@thuexe.vn',    N'$2a$11$qX0VB7q/tbOQxAk2Kqozku/mxtMxkEPVbShjOrfyqdbaaPvbDC7qS'),
(2, N'nv002@thuexe.vn',    N'$2a$11$qX0VB7q/tbOQxAk2Kqozku/mxtMxkEPVbShjOrfyqdbaaPvbDC7qS'),
(3, N'khach001@gmail.com', N'$2a$11$qX0VB7q/tbOQxAk2Kqozku/mxtMxkEPVbShjOrfyqdbaaPvbDC7qS'),
(3, N'khach002@gmail.com', N'$2a$11$qX0VB7q/tbOQxAk2Kqozku/mxtMxkEPVbShjOrfyqdbaaPvbDC7qS'),
(3, N'khach003@gmail.com', N'$2a$11$qX0VB7q/tbOQxAk2Kqozku/mxtMxkEPVbShjOrfyqdbaaPvbDC7qS');
GO

-- UserProfile - Dữ liệu mẫu đã tách địa chỉ
INSERT INTO UserProfile (AccountId, FullName, PhoneNumber, Gender, DateOfBirth, 
                       StreetAddress, Ward, District, City) 
VALUES
(1, N'Nguyen Van Admin', N'0901000001', N'Nam', '1985-03-10', 
    N'1 Hùng Vương', N'Phường Hải Châu 1', N'Quận Hải Châu', N'TP. Đà Nẵng'),

(2, N'Tran Thi Lan', N'0901000002', N'Nu', '1995-07-22', 
    N'12 Lê Duẩn', N'Phường Thạch Thang', N'Quận Hải Châu', N'TP. Đà Nẵng'),

(3, N'Le Minh Duc', N'0901000003', N'Nam', '1993-11-05', 
    N'45 Trần Phú', N'Phường Thạch Thang', N'Quận Hải Châu', N'TP. Đà Nẵng'),

(4, N'Pham Thi Hoa', N'0912345678', N'Nu', '1998-04-15', 
    N'78 Nguyễn Văn Linh', N'Phường Hòa Hiệp Nam', N'Quận Liên Chiểu', N'TP. Đà Nẵng'),

(5, N'Vo Quoc Bao', N'0923456789', N'Nam', '1990-08-30', 
    N'23 Bạch Đằng', N'Phường Hải Châu 2', N'Quận Hải Châu', N'TP. Đà Nẵng'),

(6, N'Nguyen Thi Mai', N'0934567890', N'Nu', '2000-12-01', 
    N'56 Hoàng Diệu', N'Phường Nam Dương', N'Quận Hải Châu', N'TP. Đà Nẵng');
GO

-- Staff
INSERT INTO Staff (UserProfileId, StaffCode, Position, Department, Branch, HireDate) VALUES
(2, N'NV001', N'Nhan vien kinh doanh', N'Kinh doanh', N'Chi nhanh Da Nang', '2022-01-10'),
(3, N'NV002', N'Truong phong',         N'Kinh doanh', N'Chi nhanh Da Nang', '2020-06-15');
GO

-- Customer
INSERT INTO Customer (UserProfileId, NationalId, LicenseNumber, LicenseClass, LicenseIssueDate, LicenseExpiryDate) VALUES
(4, N'123456789012', N'BL001234', N'B2', '2020-05-01', '2030-05-01'),
(5, N'234567890123', N'BL005678', N'B2', '2019-08-15', '2029-08-15'),
(6, N'345678901234', N'BL009012', N'B1', '2021-03-20', '2031-03-20');
GO

-- VehicleCategory
INSERT INTO VehicleCategory (CategoryName, Description) VALUES
(N'Sedan',      N'Vehicle con 4-5 cho, thiet ke sang trong, tiet kiem nhien lieu'),
(N'SUV',        N'Vehicle gam cao da dung, khoang noi that rong, phu hop gia dinh'),
(N'Pickup',     N'Vehicle ban tai, co thung sau, chiu tai tot, phu hop dia hinh'),
(N'Van',        N'Vehicle nhieu cho 7-9 cho, phu hop dua don tap the'),
(N'Hatchback',  N'Vehicle co nho gon, de di chuyen thanh pho, tiet kiem xang');
GO

-- Vehicle
INSERT INTO Vehicle (CategoryId, LicensePlate, VehicleName, Brand, Model, ManufactureYear, Color, FuelType, Transmission, Seats, PricePerDay, VehicleDesc) VALUES
(1, N'43A-12345', N'Toyota Camry 2022',    N'Toyota',  N'Camry',       2022, N'Trang',  N'Xang',   N'So tu dong', 5, 1200000, N'Vehicle sang trong, tiet kiem, phu hop di cong tac'),
(1, N'43B-23456', N'Honda Accord 2021',    N'Honda',   N'Accord',      2021, N'Den',    N'Xang',   N'So tu dong', 5, 1100000, N'Dong co manh me, noi that cao cap'),
(2, N'43C-34567', N'Toyota Fortuner 2023', N'Toyota',  N'Fortuner',    2023, N'Bac',    N'Dau',    N'So tu dong', 7, 1800000, N'SUV gam cao, phu hop di phuot'),
(2, N'43D-45678', N'Hyundai SantaFe 2022', N'Hyundai', N'SantaFe',     2022, N'Xanh',   N'Xang',   N'So tu dong', 7, 1600000, N'Thiet ke hien dai, nhieu tien nghi'),
(3, N'43E-56789', N'Ford Ranger 2023',     N'Ford',    N'Ranger',      2023, N'Trang',  N'Dau',    N'So tu dong', 5, 1400000, N'Vehicle ban tai manh, phu hop dia hinh khó'),
(4, N'43F-67890', N'Toyota Innova 2022',   N'Toyota',  N'Innova',      2022, N'Bac',    N'Xang',   N'So tu dong', 7, 1000000, N'Vehicle gia dinh tiet kiem, khong gian rong'),
(5, N'43G-78901', N'Kia Morning 2023',     N'Kia',     N'Morning',     2023, N'Do',     N'Xang',   N'So tu dong', 5,  500000, N'Vehicle nho gon, de do xe, phu hop noi thanh'),
(1, N'43H-89012', N'Mazda 6 2022',         N'Mazda',   N'Mazda 6',     2022, N'Xam',    N'Xang',   N'So tu dong', 5, 1300000, N'Thiet ke the thao, lai dep');
GO

-- Supplier
INSERT INTO Supplier (SupplierName, Address, PhoneNumber, Email, ContactPerson, TaxCode) VALUES
(N'Dai ly Toyota Da Nang',    N'234 Dien Bien Phu, Da Nang',  N'02363123456', N'toyota.danang@gmail.com',  N'Nguyen Van An',  N'0400123456'),
(N'Dai ly Honda Mien Trung',  N'78 Le Van Hien, Da Nang',     N'02363234567', N'honda.mientrung@gmail.com',N'Tran Thi Binh',  N'0400234567'),
(N'Cong ty Vehicle Hoang Gia',     N'45 Nguyen Tat Thanh, Da Nang',N'02363345678', N'hoanggia.xe@gmail.com',    N'Le Minh Cuong',  N'0400345678');
GO

-- ImportReceipt
INSERT INTO ImportReceipt (SoImportReceipt, SupplierId, StaffId, ApprovedByStaffId, ImportDate, ApprovalDate, Status, Note) VALUES
(N'PN2024001', 1, 1, 2, '2024-01-15', '2024-01-16', N'Da duyet', N'Nhap lo xe dau nam 2024'),
(N'PN2024002', 2, 1, 2, '2024-03-20', '2024-03-21', N'Da duyet', N'Bo sung them xe gia dinh'),
(N'PN2024003', 3, 1, NULL, '2024-06-10', NULL,        N'Cho duyet', N'Nhap xe ban tai');
GO

-- ImportReceiptDetail (LineTotal tu dong tinh qua computed column)
INSERT INTO ImportReceiptDetail (ImportReceiptId, VehicleId, Quantity, UnitPrice, VehicleCondition, CurrentKm) VALUES
(1, 1, 1, 750000000, N'Moi 100%', 0),
(1, 2, 1, 700000000, N'Moi 100%', 0),
(2, 6, 1, 580000000, N'Moi 100%', 0),
(2, 7, 1, 380000000, N'Moi 100%', 0),
(3, 5, 1, 820000000, N'Moi 100%', 0);
GO

-- Booking
INSERT INTO Booking (CustomerId, VehicleId, StaffId, PickupLocation, ReturnLocation, PickupDateTime, ReturnDateTime, BasePrice, DiscountAmount, TotalAmount, Status, BookingChannel) VALUES
(1, 1, 1, N'15 Tran Phu, Da Nang', N'15 Tran Phu, Da Nang', '2024-05-01 08:00', '2024-05-04 08:00', 3600000, 0,       3600000, N'Hoan thanh', N'Online'),
(2, 3, 1, N'San bay Da Nang',       N'San bay Da Nang',       '2024-05-10 09:00', '2024-05-13 09:00', 5400000, 500000, 4900000, N'Hoan thanh', N'Tai quay'),
(3, 7, 2, N'32 Le Loi, Da Nang',   N'32 Le Loi, Da Nang',   '2024-06-01 07:00', '2024-06-02 07:00',  500000, 0,        500000, N'Hoan thanh', N'Online'),
(1, 4, 1, N'Khach san Novotel',     N'Khach san Novotel',     '2024-07-15 10:00', '2024-07-18 10:00', 4800000, 0,       4800000, N'Da xac nhan',N'Online'),
(2, 6, 2, N'22 Hung Vuong, Da Nang',N'22 Hung Vuong, Da Nang','2024-08-01 08:00', '2024-08-03 08:00', 2000000, 200000, 1800000, N'Cho xac nhan',N'Online');
GO

-- Invoice
INSERT INTO Invoice (InvoiceNumber, BookingId, CustomerId, StaffId, LoaiInvoice, SubTotal, TaxRate, DiscountAmount, GrandTotal, Status, IssueDate, PaidDate) VALUES
(N'HD2024001', 1, 1, 1, N'Thue xe', 3600000,  10, 0,       3960000,  N'Da thanh toan', '2024-05-01', '2024-05-01'),
(N'HD2024002', 2, 2, 1, N'Thue xe', 4900000,  10, 500000,  4890000,  N'Da thanh toan', '2024-05-10', '2024-05-10'),
(N'HD2024003', 3, 3, 2, N'Thue xe',  500000,  10, 0,        550000,  N'Da thanh toan', '2024-06-01', '2024-06-01');
GO

-- InvoiceDetail
INSERT INTO InvoiceDetail (InvoiceId, ItemType, Description, Quantity, UnitPrice, DiscountPercent) VALUES
-- HD001: Toyota Camry 3 ngay
(1, N'Tien thue xe', N'Toyota Camry - 3 ngay (01/05 - 04/05)', 3, 1200000, 0),
(1, N'Phi bao hiem', N'Bao hiem xe trong thoi gian thue',       1,  150000, 0),
(1, N'Phi ve sinh',  N'Ve sinh xe truoc khi giao',              1,   50000, 0),
-- HD002: Toyota Fortuner 3 ngay
(2, N'Tien thue xe', N'Toyota Fortuner - 3 ngay (10/05 - 13/05)',3, 1800000, 0),
(2, N'Phi bao hiem', N'Bao hiem xe trong thoi gian thue',        1,  200000, 0),
(2, N'Phi tai xe',   N'Thue tai xe trong 3 ngay',                3,  300000, 0),
-- HD003: Kia Morning 1 ngay
(3, N'Tien thue xe', N'Kia Morning - 1 ngay (01/06 - 02/06)',   1,  500000, 0),
(3, N'Phi ve sinh',  N'Ve sinh xe truoc khi giao',               1,   30000, 0);
GO

-- Payment
INSERT INTO Payment (InvoiceId, StaffId, PaymentMethod, TransactionCode, Amount, Status, PaymentTime) VALUES
(1, 1,    N'VNPay',          N'VNP20240501001', 3960000, N'Thanh cong', '2024-05-01 08:30'),
(2, 1,    N'Tien mat',       NULL,              4890000, N'Thanh cong', '2024-05-10 09:15'),
(3, NULL, N'Chuyen khoan',   N'BIDV20240601001',  550000, N'Thanh cong', '2024-06-01 07:45');
GO

-- Review
INSERT INTO Review (BookingId, CustomerId, VehicleId, StarRating, Content) VALUES
(1, 1, 1, 5, N'Vehicle dep, sach se, lai thoai mai. Nhan vien nhiet tinh. Rat hai long!'),
(2, 2, 3, 4, N'Vehicle tot, khong gian rong. Se thue lai lan sau.'),
(3, 3, 7, 5, N'Vehicle nho gon, phu hop di trong thanh pho. Gia ca hop ly.');
GO

-- MaintenanceLog
INSERT INTO MaintenanceLog (VehicleId, StaffId, MaintenanceType, Description, Cost, MaintenanceDate, MaintenanceDateTiep, Status) VALUES
(1, 2, N'Dinh ky',  N'Thay dau dong co, loc gio, kiem tra phanh',     2500000, '2024-04-01', '2024-10-01', N'Hoan thanh'),
(3, 2, N'Sua chua', N'Thay lop xe truoc phai, can chinh vo lang',      3800000, '2024-04-15', '2025-04-15', N'Hoan thanh'),
(7, 1, N'Dinh ky',  N'Thay dau dong co, kiem tra he thong dien',       1800000, '2024-05-20', '2024-11-20', N'Hoan thanh'),
(4, 2, N'Kiem tra', N'Kiem tra truoc mua he: dieu hoa, lop, phanh',     500000, '2024-06-01', NULL,          N'Hoan thanh');
GO

-- ============================================================
--  CAC VIEW HUU ICH (dung cho bao cao)
-- ============================================================

-- View: Thong tin day du cua khach hang
CREATE VIEW vw_Customer AS
SELECT
    kh.CustomerId,
    nd.FullName,
    tk.Email,
    nd.PhoneNumber,
    -- Địa chỉ mới
    nd.StreetAddress,
    nd.Ward,
    nd.District,
    nd.City,
    kh.NationalId,
    kh.LicenseNumber,
    kh.LicenseClass,
    kh.TotalRentals,
    kh.TotalSpent,
    kh.CreatedAt
FROM Customer kh
JOIN UserProfile nd ON kh.UserProfileId = nd.UserProfileId
JOIN Account tk ON nd.AccountId = tk.AccountId;
GO

-- View: Danh sach don dat xe day du
CREATE VIEW vw_Booking AS
SELECT
    dx.BookingId,
    nd.FullName        AS TenCustomer,
    tk.Email,
    nd.PhoneNumber,
    x.LicensePlate,
    x.VehicleName,
    lx.CategoryName      AS VehicleCategory,
    dx.PickupLocation,
    dx.ReturnLocation,
    dx.PickupDateTime,
    dx.ReturnDateTime,
    dx.RentalDays,
    dx.BasePrice,
    dx.DiscountAmount,
    dx.ExtraFee,
    dx.TotalAmount,
    dx.Status,
    dx.BookingChannel,
    dx.CreatedAt
FROM Booking dx
JOIN Customer kh ON dx.CustomerId   = kh.CustomerId
JOIN UserProfile nd ON kh.UserProfileId = nd.UserProfileId
JOIN Account  tk ON nd.AccountId  = tk.AccountId
JOIN Vehicle        x  ON dx.VehicleId   = x.VehicleId
JOIN VehicleCategory    lx ON x.CategoryId  = lx.CategoryId;
GO

-- View: Doanh thu theo thang
CREATE VIEW vw_MonthlyRevenue AS
SELECT
    YEAR(hd.IssueDate)    AS Nam,
    MONTH(hd.IssueDate)   AS Thang,
    COUNT(*)            AS InvoiceNumber,
    SUM(hd.GrandTotal)    AS TongDoanhThu
FROM Invoice hd
WHERE hd.Status = N'Da thanh toan'
GROUP BY YEAR(hd.IssueDate), MONTH(hd.IssueDate);
GO

-- View: Vehicle duoc thue nhieu nhat
CREATE VIEW vw_TopRentedVehicles AS
SELECT
    x.VehicleId,
    x.LicensePlate,
    x.VehicleName,
    x.Brand,
    x.PricePerDay,
    x.AverageRating,
    COUNT(dx.BookingId)       AS SoLanThue,
    SUM(dx.TotalAmount)        AS TongDoanhThu
FROM Vehicle x
LEFT JOIN Booking dx ON x.VehicleId = dx.VehicleId
    AND dx.Status = N'Hoan thanh'
GROUP BY x.VehicleId, x.LicensePlate, x.VehicleName, x.Brand, x.PricePerDay, x.AverageRating;
GO

-- ============================================================
--  STORED PROCEDURE
-- ============================================================

-- SP: Kiem tra xe co trong trong khoang thoi gian khong
CREATE PROCEDURE sp_CheckVehicleAvailability
    @VehicleId        INT,
    @PickupDateTime DATETIME,
    @ReturnDateTime  DATETIME
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*) AS SoDonTrung
    FROM Booking
    WHERE VehicleId = @VehicleId
      AND Status NOT IN (N'Da huy')
      AND NOT (ReturnDateTime <= @PickupDateTime OR PickupDateTime >= @ReturnDateTime);
END;
GO

-- SP: Lay danh sach xe con trong theo ngay
CREATE PROCEDURE sp_GetAvailableVehicles
    @PickupDateTime DATETIME,
    @ReturnDateTime  DATETIME,
    @CategoryId       INT = NULL   -- NULL = lay tat ca loai
AS
BEGIN
    SET NOCOUNT ON;
    SELECT x.*, lx.CategoryName
    FROM Vehicle x
    JOIN VehicleCategory lx ON x.CategoryId = lx.CategoryId
    WHERE x.Status = N'San sang'
      AND (@CategoryId IS NULL OR x.CategoryId = @CategoryId)
      AND x.VehicleId NOT IN (
          SELECT VehicleId FROM Booking
          WHERE Status NOT IN (N'Da huy')
            AND NOT (ReturnDateTime <= @PickupDateTime OR PickupDateTime >= @ReturnDateTime)
      );
END;
GO

-- ============================================================
--  KIEM TRA DU LIEU DA INSERT
-- ============================================================
SELECT N'=== KIEM TRA DATABASE ===' AS ThongBao;
SELECT 'Role'          AS Bang, COUNT(*) AS SoBanGhi FROM Role          UNION ALL
SELECT 'Account',                COUNT(*)              FROM Account        UNION ALL
SELECT 'UserProfile',               COUNT(*)              FROM UserProfile       UNION ALL
SELECT 'Customer',               COUNT(*)              FROM Customer       UNION ALL
SELECT 'Staff',                COUNT(*)              FROM Staff        UNION ALL
SELECT 'VehicleCategory',                  COUNT(*)              FROM VehicleCategory          UNION ALL
SELECT 'Vehicle',                      COUNT(*)              FROM Vehicle              UNION ALL
SELECT 'Supplier',              COUNT(*)              FROM Supplier      UNION ALL
SELECT 'ImportReceipt',               COUNT(*)              FROM ImportReceipt       UNION ALL
SELECT 'ImportReceiptDetail',            COUNT(*)              FROM ImportReceiptDetail    UNION ALL
SELECT 'Booking',                   COUNT(*)              FROM Booking           UNION ALL
SELECT 'Invoice',                  COUNT(*)              FROM Invoice          UNION ALL
SELECT 'InvoiceDetail',               COUNT(*)              FROM InvoiceDetail       UNION ALL
SELECT 'Payment',               COUNT(*)              FROM Payment       UNION ALL
SELECT 'Review',                 COUNT(*)              FROM Review         UNION ALL
SELECT 'MaintenanceLog',          COUNT(*)              FROM MaintenanceLog;

-- Thu procedure tim xe trong
EXEC sp_GetAvailableVehicles '2024-09-01 08:00', '2024-09-04 08:00', NULL;
GO