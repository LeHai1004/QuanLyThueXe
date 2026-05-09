-- ============================================================
--  DATABASE: QUAN LY THUE XE O TO
--  Phan mem: SQL Server 2019+
--  Encoding: UTF-8
--  Gom: 12 bang cot loi + du lieu mau
-- ============================================================

USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'QuanLyThueXe')
    DROP DATABASE QuanLyThueXe;
GO

CREATE DATABASE QuanLyThueXe
    COLLATE Vietnamese_CI_AS;
GO

USE QuanLyThueXe;
GO

-- ============================================================
-- BANG 1: VAITRO
-- ============================================================
CREATE TABLE VaiTro (
    MaVaiTro INT PRIMARY KEY IDENTITY(1,1),
    TenVaiTro NVARCHAR(50) NOT NULL UNIQUE,
    MoTa NVARCHAR(200) NULL
);
GO

-- ============================================================
-- BANG 2: TAIKHOAN
-- ============================================================
CREATE TABLE TaiKhoan (
    MaTaiKhoan INT PRIMARY KEY IDENTITY(1,1),
    MaVaiTro INT NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    MatKhauHash NVARCHAR(255) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    NgayTao DATETIME NOT NULL DEFAULT GETDATE(),
    NgayCapNhat DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_TaiKhoan_VaiTro FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro)
);
GO

-- ============================================================
-- BANG 3: NGUOIDUNG (ĐÃ SỬA ĐỊA CHỈ)
-- ============================================================
CREATE TABLE NguoiDung (
    MaNguoiDung INT PRIMARY KEY IDENTITY(1,1),
    MaTaiKhoan INT NOT NULL UNIQUE,
    HoTen NVARCHAR(150) NOT NULL,
    SoDienThoai NVARCHAR(15) NULL,
    GioiTinh NVARCHAR(10) NULL,
    NgaySinh DATE NULL,
    
    -- ĐỊA CHỈ ĐÃ TÁCH THEO 1NF
    SoNha_Duong NVARCHAR(200) NULL,
    Phuong_Xa NVARCHAR(100) NULL,
    Quan_Huyen NVARCHAR(100) NULL,
    Tinh_ThanhPho NVARCHAR(100) NULL,
    
    AvatarUrl NVARCHAR(300) NULL,
    NgayTao DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_NguoiDung_TaiKhoan FOREIGN KEY (MaTaiKhoan) REFERENCES TaiKhoan(MaTaiKhoan)
);
GO

-- ============================================================
-- BANG 4: KHACHHANG (mo rong tu NguoiDung)
-- ============================================================
CREATE TABLE KhachHang (
    MaKH INT PRIMARY KEY IDENTITY(1,1),
    MaNguoiDung INT NOT NULL UNIQUE,
    SoCCCD NVARCHAR(20) NULL,
    AnhCCCDTruoc NVARCHAR(300) NULL,
    AnhCCCDSau NVARCHAR(300) NULL,
    SoBangLai NVARCHAR(20) NULL,
    HangBangLai NVARCHAR(10) NULL,
    NgayCapBL DATE NULL,
    NgayHetHanBL DATE NULL,
    TongChiTieu DECIMAL(18,2) NOT NULL DEFAULT 0,
    TongLanThue INT NOT NULL DEFAULT 0,
    NgayTao DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_KhachHang_NguoiDung FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung)
);
GO

-- ============================================================
-- BANG 5: NHANVIEN (mo rong tu NguoiDung)
-- ============================================================
CREATE TABLE NhanVien (
    MaNV            INT             PRIMARY KEY IDENTITY(1,1),
    MaNguoiDung     INT             NOT NULL UNIQUE,
    MaNVCode        NVARCHAR(20)    NOT NULL UNIQUE,  -- 'NV001', 'NV002'
    ChucVu          NVARCHAR(100)   NULL,
    PhongBan        NVARCHAR(100)   NULL,
    ChiNhanh        NVARCHAR(150)   NULL,
    NgayVaoLam      DATE            NULL,
    IsActive        BIT             NOT NULL DEFAULT 1,
    CONSTRAINT FK_NhanVien_NguoiDung FOREIGN KEY (MaNguoiDung)
        REFERENCES NguoiDung(MaNguoiDung)
);
GO

-- ============================================================
-- BANG 6: LOAIXE (danh muc loai xe)
-- ============================================================
CREATE TABLE LoaiXe (
    MaLoai          INT             PRIMARY KEY IDENTITY(1,1),
    TenLoai         NVARCHAR(100)   NOT NULL,   -- 'Sedan', 'SUV', 'Pickup', 'Van'
    MoTa            NVARCHAR(300)   NULL,
    IsActive        BIT             NOT NULL DEFAULT 1
);
GO

-- ============================================================
-- BANG 7: XE (thong tin xe cho thue)
-- ============================================================
CREATE TABLE Xe (
    MaXe            INT             PRIMARY KEY IDENTITY(1,1),
    MaLoai          INT             NOT NULL,
    BienSo          NVARCHAR(15)    NOT NULL UNIQUE,
    TenXe           NVARCHAR(150)   NOT NULL,
    Hang            NVARCHAR(100)   NOT NULL,   -- 'Toyota', 'Honda', 'Mercedes'
    Model           NVARCHAR(100)   NOT NULL,
    NamSanXuat      INT             NOT NULL,
    MauSac          NVARCHAR(50)    NULL,
    NhienLieu       NVARCHAR(30)    NULL,       -- 'Xang', 'Dau', 'Dien', 'Hybrid'
    HopSo           NVARCHAR(30)    NULL,       -- 'So tu dong', 'So san'
    SoGhe           INT             NOT NULL DEFAULT 5,
    GiaThueNgay     DECIMAL(18,2)   NOT NULL,
    MoTaXe          NVARCHAR(500)   NULL,
    HinhAnh         NVARCHAR(300)   NULL,
    TrangThai       NVARCHAR(30)    NOT NULL DEFAULT N'San sang',
        -- N'San sang', N'Dang thue', N'Bao duong', N'Ngung hoat dong'
    DanhGiaTB       DECIMAL(3,2)    NOT NULL DEFAULT 0,
    NgayTao         DATETIME        NOT NULL DEFAULT GETDATE(),
    NgayCapNhat     DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Xe_LoaiXe FOREIGN KEY (MaLoai)
        REFERENCES LoaiXe(MaLoai),
    CONSTRAINT CK_Xe_SoGhe      CHECK (SoGhe BETWEEN 2 AND 50),
    CONSTRAINT CK_Xe_GiaThue    CHECK (GiaThueNgay > 0),
    CONSTRAINT CK_Xe_DanhGia    CHECK (DanhGiaTB BETWEEN 0 AND 5)
);
GO

-- ============================================================
-- BANG 8: NHACUNGCAP
-- ============================================================
CREATE TABLE NhaCungCap (
    MaNCC           INT             PRIMARY KEY IDENTITY(1,1),
    TenNCC          NVARCHAR(200)   NOT NULL,
    DiaChi          NVARCHAR(300)   NULL,
    SoDienThoai     NVARCHAR(15)    NULL,
    Email           NVARCHAR(150)   NULL,
    NguoiLienHe     NVARCHAR(150)   NULL,
    MaSoThue        NVARCHAR(20)    NULL,
    IsActive        BIT             NOT NULL DEFAULT 1,
    NgayTao         DATETIME        NOT NULL DEFAULT GETDATE()
);
GO

-- ============================================================
-- BANG 9: PHIEUNHAP (phieu nhap xe - master)
-- ============================================================
CREATE TABLE PhieuNhap (
    MaPhieuNhap     INT             PRIMARY KEY IDENTITY(1,1),
    SoPhieuNhap     NVARCHAR(20)    NOT NULL UNIQUE,  -- 'PN2024001'
    MaNCC           INT             NOT NULL,
    MaNV            INT             NOT NULL,   -- NV lap phieu
    MaNVDuyet       INT             NULL,       -- Admin/NV duyet
    NgayNhap        DATETIME        NOT NULL DEFAULT GETDATE(),
    NgayDuyet       DATETIME        NULL,
    TongTien        DECIMAL(18,2)   NOT NULL DEFAULT 0,
    TrangThai       NVARCHAR(30)    NOT NULL DEFAULT N'Cho duyet',
        -- N'Cho duyet', N'Da duyet', N'Tu choi'
    GhiChu          NVARCHAR(500)   NULL,
    NgayTao         DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_PhieuNhap_NCC      FOREIGN KEY (MaNCC)
        REFERENCES NhaCungCap(MaNCC),
    CONSTRAINT FK_PhieuNhap_NV       FOREIGN KEY (MaNV)
        REFERENCES NhanVien(MaNV),
    CONSTRAINT FK_PhieuNhap_NVDuyet  FOREIGN KEY (MaNVDuyet)
        REFERENCES NhanVien(MaNV)
);
GO

-- ============================================================
-- BANG 10: CT_PHIEUNHAP (chi tiet phieu nhap - detail)
-- ============================================================
CREATE TABLE CT_PhieuNhap (
    MaCTPN          INT             PRIMARY KEY IDENTITY(1,1),
    MaPhieuNhap     INT             NOT NULL,
    MaXe            INT             NOT NULL,
    SoLuong         INT             NOT NULL DEFAULT 1,
    DonGia          DECIMAL(18,2)   NOT NULL,
    ThanhTien       AS (SoLuong * DonGia) PERSISTED,  -- tu dong tinh
    TinhTrangXe     NVARCHAR(100)   NULL,   -- 'Moi 100%', 'Da qua su dung - Tot'
    SoKmHienTai     INT             NOT NULL DEFAULT 0,
    GhiChu          NVARCHAR(300)   NULL,
    CONSTRAINT FK_CTPN_PhieuNhap FOREIGN KEY (MaPhieuNhap)
        REFERENCES PhieuNhap(MaPhieuNhap),
    CONSTRAINT FK_CTPN_Xe        FOREIGN KEY (MaXe)
        REFERENCES Xe(MaXe),
    CONSTRAINT CK_CTPN_SoLuong  CHECK (SoLuong > 0),
    CONSTRAINT CK_CTPN_DonGia   CHECK (DonGia > 0)
);
GO

-- ============================================================
-- BANG 11: DATXE (don dat xe cua khach)
-- ============================================================
CREATE TABLE DatXe (
    MaDatXe         INT             PRIMARY KEY IDENTITY(1,1),
    MaKH            INT             NOT NULL,
    MaXe            INT             NOT NULL,
    MaNV            INT             NULL,   -- NV tiep nhan (NULL = dat online)
    DiaDiemNhan     NVARCHAR(300)   NOT NULL,
    DiaDiemTra      NVARCHAR(300)   NOT NULL,
    ThoiGianNhan    DATETIME        NOT NULL,
    ThoiGianTra     DATETIME        NOT NULL,
    SoNgayThue      AS (DATEDIFF(DAY, ThoiGianNhan, ThoiGianTra)) PERSISTED,
    GiaCoBan        DECIMAL(18,2)   NOT NULL,
    TienGiam        DECIMAL(18,2)   NOT NULL DEFAULT 0,
    PhiPhatSinh     DECIMAL(18,2)   NOT NULL DEFAULT 0,
    TongTien        DECIMAL(18,2)   NOT NULL,
    TrangThai       NVARCHAR(30)    NOT NULL DEFAULT N'Cho xac nhan',
        -- N'Cho xac nhan', N'Da xac nhan', N'Dang thue', N'Hoan thanh', N'Da huy'
    KenhDat         NVARCHAR(20)    NOT NULL DEFAULT N'Online', -- N'Online', N'Tai quay'
    GhiChu          NVARCHAR(500)   NULL,
    NgayTao         DATETIME        NOT NULL DEFAULT GETDATE(),
    NgayCapNhat     DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_DatXe_KhachHang FOREIGN KEY (MaKH)
        REFERENCES KhachHang(MaKH),
    CONSTRAINT FK_DatXe_Xe        FOREIGN KEY (MaXe)
        REFERENCES Xe(MaXe),
    CONSTRAINT FK_DatXe_NhanVien  FOREIGN KEY (MaNV)
        REFERENCES NhanVien(MaNV),
    CONSTRAINT CK_DatXe_ThoiGian CHECK (ThoiGianTra > ThoiGianNhan),
    CONSTRAINT CK_DatXe_TongTien CHECK (TongTien >= 0)
);
GO

-- ============================================================
-- BANG 12: HOADON (hoa don - master)
-- ============================================================
CREATE TABLE HoaDon (
    MaHoaDon        INT             PRIMARY KEY IDENTITY(1,1),
    SoHoaDon        NVARCHAR(20)    NOT NULL UNIQUE,  -- 'HD2024001'
    MaDatXe         INT             NOT NULL UNIQUE,
    MaKH            INT             NOT NULL,
    MaNV            INT             NULL,   -- NV lap hoa don
    LoaiHoaDon      NVARCHAR(30)    NOT NULL DEFAULT N'Thue xe',
        -- N'Thue xe', N'Dat coc', N'Phat'
    TienTruocThue   DECIMAL(18,2)   NOT NULL,
    ThueSuat        DECIMAL(5,2)    NOT NULL DEFAULT 10, -- 10%
    TienThue        AS (ROUND(TienTruocThue * ThueSuat / 100, 0)) PERSISTED,
    TienGiam        DECIMAL(18,2)   NOT NULL DEFAULT 0,
    TongCong        DECIMAL(18,2)   NOT NULL,
    TrangThai       NVARCHAR(30)    NOT NULL DEFAULT N'Chua thanh toan',
        -- N'Chua thanh toan', N'Da thanh toan', N'Da huy'
    GhiChu          NVARCHAR(500)   NULL,
    NgayLap         DATETIME        NOT NULL DEFAULT GETDATE(),
    HanThanhToan    DATETIME        NULL,
    NgayThanhToan   DATETIME        NULL,
    CONSTRAINT FK_HoaDon_DatXe FOREIGN KEY (MaDatXe)
        REFERENCES DatXe(MaDatXe),
    CONSTRAINT FK_HoaDon_KH    FOREIGN KEY (MaKH)
        REFERENCES KhachHang(MaKH),
    CONSTRAINT FK_HoaDon_NV    FOREIGN KEY (MaNV)
        REFERENCES NhanVien(MaNV)
);
GO

-- ============================================================
-- BANG 13: CT_HOADON (chi tiet hoa don - detail)
-- ============================================================
CREATE TABLE CT_HoaDon (
    MaCTHD          INT             PRIMARY KEY IDENTITY(1,1),
    MaHoaDon        INT             NOT NULL,
    LoaiKhoan       NVARCHAR(50)    NOT NULL,
        -- N'Tien thue xe', N'Phi bao hiem', N'Phi ve sinh',
        -- N'Phi qua gio', N'Phi xang', N'Tien phat'
    MoTa            NVARCHAR(200)   NOT NULL,
    SoLuong         INT             NOT NULL DEFAULT 1,
    DonGia          DECIMAL(18,2)   NOT NULL,
    ChietKhau       DECIMAL(5,2)    NOT NULL DEFAULT 0,
    ThanhTien       AS (ROUND(SoLuong * DonGia * (1 - ChietKhau / 100), 0)) PERSISTED,
    GhiChu          NVARCHAR(200)   NULL,
    CONSTRAINT FK_CTHD_HoaDon FOREIGN KEY (MaHoaDon)
        REFERENCES HoaDon(MaHoaDon),
    CONSTRAINT CK_CTHD_SoLuong  CHECK (SoLuong > 0),
    CONSTRAINT CK_CTHD_DonGia   CHECK (DonGia >= 0),
    CONSTRAINT CK_CTHD_ChietKhau CHECK (ChietKhau BETWEEN 0 AND 100)
);
GO

-- ============================================================
-- BANG 14: THANHTOAN
-- ============================================================
CREATE TABLE ThanhToan (
    MaThanhToan     INT             PRIMARY KEY IDENTITY(1,1),
    MaHoaDon        INT             NOT NULL,
    MaNV            INT             NULL,   -- NULL = thanh toan online tu dong
    PhuongThuc      NVARCHAR(50)    NOT NULL,
        -- N'Tien mat', N'Chuyen khoan', N'VNPay', N'Momo', N'The ngan hang'
    MaGiaoDich      NVARCHAR(100)   NULL,   -- Ma tu cong thanh toan
    SoTien          DECIMAL(18,2)   NOT NULL,
    TrangThai       NVARCHAR(20)    NOT NULL DEFAULT N'Thanh cong',
        -- N'Thanh cong', N'That bai', N'Hoan tien'
    GhiChu          NVARCHAR(300)   NULL,
    ThoiGianTT      DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_ThanhToan_HoaDon FOREIGN KEY (MaHoaDon)
        REFERENCES HoaDon(MaHoaDon),
    CONSTRAINT FK_ThanhToan_NV     FOREIGN KEY (MaNV)
        REFERENCES NhanVien(MaNV),
    CONSTRAINT CK_ThanhToan_SoTien CHECK (SoTien > 0)
);
GO

-- ============================================================
-- BANG 15: DANHGIA
-- ============================================================
CREATE TABLE DanhGia (
    MaDanhGia       INT             PRIMARY KEY IDENTITY(1,1),
    MaDatXe         INT             NOT NULL UNIQUE, -- 1 don chi duoc danh gia 1 lan
    MaKH            INT             NOT NULL,
    MaXe            INT             NOT NULL,
    DiemSao         INT             NOT NULL,
    NoiDung         NVARCHAR(500)   NULL,
    NgayDanhGia     DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_DanhGia_DatXe FOREIGN KEY (MaDatXe)
        REFERENCES DatXe(MaDatXe),
    CONSTRAINT FK_DanhGia_KH    FOREIGN KEY (MaKH)
        REFERENCES KhachHang(MaKH),
    CONSTRAINT FK_DanhGia_Xe    FOREIGN KEY (MaXe)
        REFERENCES Xe(MaXe),
    CONSTRAINT CK_DanhGia_Diem  CHECK (DiemSao BETWEEN 1 AND 5)
);
GO

-- ============================================================
-- BANG 16: LICHSUBAODUONG
-- ============================================================
CREATE TABLE LichSuBaoDuong (
    MaBaoDuong      INT             PRIMARY KEY IDENTITY(1,1),
    MaXe            INT             NOT NULL,
    MaNV            INT             NULL,
    LoaiBaoDuong    NVARCHAR(100)   NOT NULL,   -- N'Dinh ky', N'Sua chua', N'Kiem tra'
    MoTa            NVARCHAR(500)   NULL,
    ChiPhi          DECIMAL(18,2)   NOT NULL DEFAULT 0,
    NgayBaoDuong    DATE            NOT NULL,
    NgayBaoDuongTiep DATE           NULL,
    TrangThai       NVARCHAR(30)    NOT NULL DEFAULT N'Hoan thanh',
    CONSTRAINT FK_BaoDuong_Xe  FOREIGN KEY (MaXe)
        REFERENCES Xe(MaXe),
    CONSTRAINT FK_BaoDuong_NV  FOREIGN KEY (MaNV)
        REFERENCES NhanVien(MaNV)
);
GO

-- ============================================================
--  TRIGGER: Tu dong cap nhat TongTien PhieuNhap
--  khi them/sua/xoa CT_PhieuNhap
-- ============================================================
CREATE TRIGGER trg_CapNhat_TongTienPhieuNhap
ON CT_PhieuNhap
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @MaPhieuNhap INT;

    -- Lay MaPhieuNhap tu ban ghi vua thay doi
    SELECT @MaPhieuNhap = COALESCE(
        (SELECT TOP 1 MaPhieuNhap FROM inserted),
        (SELECT TOP 1 MaPhieuNhap FROM deleted)
    );

    UPDATE PhieuNhap
    SET TongTien = (
        SELECT ISNULL(SUM(ThanhTien), 0)
        FROM CT_PhieuNhap
        WHERE MaPhieuNhap = @MaPhieuNhap
    )
    WHERE MaPhieuNhap = @MaPhieuNhap;
END;
GO

-- ============================================================
--  TRIGGER: Tu dong cap nhat DanhGiaTB cua Xe
--  khi them danh gia moi
-- ============================================================
CREATE TRIGGER trg_CapNhat_DanhGiaTB
ON DanhGia
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @MaXe INT;

    SELECT @MaXe = COALESCE(
        (SELECT TOP 1 MaXe FROM inserted),
        (SELECT TOP 1 MaXe FROM deleted)
    );

    UPDATE Xe
    SET DanhGiaTB = (
        SELECT ISNULL(AVG(CAST(DiemSao AS DECIMAL(3,2))), 0)
        FROM DanhGia
        WHERE MaXe = @MaXe
    )
    WHERE MaXe = @MaXe;
END;
GO

-- ============================================================
--  DU LIEU MAU
-- ============================================================

-- VaiTro
INSERT INTO VaiTro (TenVaiTro, MoTa) VALUES
(N'Admin',      N'Quan tri vien, toan quyen he thong'),
(N'NhanVien',   N'Nhan vien xu ly nghiep vu hang ngay'),
(N'KhachHang',  N'Khach hang su dung dich vu thue xe');
GO

-- TaiKhoan (mat khau mau: "123456" - trong thuc te phai hash)
INSERT INTO TaiKhoan (MaVaiTro, Email, MatKhauHash) VALUES
(1, N'admin@thuexe.vn',    N'$2a$10$hashedpassword_admin'),
(2, N'nv001@thuexe.vn',    N'$2a$10$hashedpassword_nv001'),
(2, N'nv002@thuexe.vn',    N'$2a$10$hashedpassword_nv002'),
(3, N'khach001@gmail.com', N'$2a$10$hashedpassword_kh001'),
(3, N'khach002@gmail.com', N'$2a$10$hashedpassword_kh002'),
(3, N'khach003@gmail.com', N'$2a$10$hashedpassword_kh003');
GO

-- NguoiDung - Dữ liệu mẫu đã tách địa chỉ
INSERT INTO NguoiDung (MaTaiKhoan, HoTen, SoDienThoai, GioiTinh, NgaySinh, 
                       SoNha_Duong, Phuong_Xa, Quan_Huyen, Tinh_ThanhPho) 
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

-- NhanVien
INSERT INTO NhanVien (MaNguoiDung, MaNVCode, ChucVu, PhongBan, ChiNhanh, NgayVaoLam) VALUES
(2, N'NV001', N'Nhan vien kinh doanh', N'Kinh doanh', N'Chi nhanh Da Nang', '2022-01-10'),
(3, N'NV002', N'Truong phong',         N'Kinh doanh', N'Chi nhanh Da Nang', '2020-06-15');
GO

-- KhachHang
INSERT INTO KhachHang (MaNguoiDung, SoCCCD, SoBangLai, HangBangLai, NgayCapBL, NgayHetHanBL) VALUES
(4, N'123456789012', N'BL001234', N'B2', '2020-05-01', '2030-05-01'),
(5, N'234567890123', N'BL005678', N'B2', '2019-08-15', '2029-08-15'),
(6, N'345678901234', N'BL009012', N'B1', '2021-03-20', '2031-03-20');
GO

-- LoaiXe
INSERT INTO LoaiXe (TenLoai, MoTa) VALUES
(N'Sedan',      N'Xe con 4-5 cho, thiet ke sang trong, tiet kiem nhien lieu'),
(N'SUV',        N'Xe gam cao da dung, khoang noi that rong, phu hop gia dinh'),
(N'Pickup',     N'Xe ban tai, co thung sau, chiu tai tot, phu hop dia hinh'),
(N'Van',        N'Xe nhieu cho 7-9 cho, phu hop dua don tap the'),
(N'Hatchback',  N'Xe co nho gon, de di chuyen thanh pho, tiet kiem xang');
GO

-- Xe
INSERT INTO Xe (MaLoai, BienSo, TenXe, Hang, Model, NamSanXuat, MauSac, NhienLieu, HopSo, SoGhe, GiaThueNgay, MoTaXe) VALUES
(1, N'43A-12345', N'Toyota Camry 2022',    N'Toyota',  N'Camry',       2022, N'Trang',  N'Xang',   N'So tu dong', 5, 1200000, N'Xe sang trong, tiet kiem, phu hop di cong tac'),
(1, N'43B-23456', N'Honda Accord 2021',    N'Honda',   N'Accord',      2021, N'Den',    N'Xang',   N'So tu dong', 5, 1100000, N'Dong co manh me, noi that cao cap'),
(2, N'43C-34567', N'Toyota Fortuner 2023', N'Toyota',  N'Fortuner',    2023, N'Bac',    N'Dau',    N'So tu dong', 7, 1800000, N'SUV gam cao, phu hop di phuot'),
(2, N'43D-45678', N'Hyundai SantaFe 2022', N'Hyundai', N'SantaFe',     2022, N'Xanh',   N'Xang',   N'So tu dong', 7, 1600000, N'Thiet ke hien dai, nhieu tien nghi'),
(3, N'43E-56789', N'Ford Ranger 2023',     N'Ford',    N'Ranger',      2023, N'Trang',  N'Dau',    N'So tu dong', 5, 1400000, N'Xe ban tai manh, phu hop dia hinh khó'),
(4, N'43F-67890', N'Toyota Innova 2022',   N'Toyota',  N'Innova',      2022, N'Bac',    N'Xang',   N'So tu dong', 7, 1000000, N'Xe gia dinh tiet kiem, khong gian rong'),
(5, N'43G-78901', N'Kia Morning 2023',     N'Kia',     N'Morning',     2023, N'Do',     N'Xang',   N'So tu dong', 5,  500000, N'Xe nho gon, de do xe, phu hop noi thanh'),
(1, N'43H-89012', N'Mazda 6 2022',         N'Mazda',   N'Mazda 6',     2022, N'Xam',    N'Xang',   N'So tu dong', 5, 1300000, N'Thiet ke the thao, lai dep');
GO

-- NhaCungCap
INSERT INTO NhaCungCap (TenNCC, DiaChi, SoDienThoai, Email, NguoiLienHe, MaSoThue) VALUES
(N'Dai ly Toyota Da Nang',    N'234 Dien Bien Phu, Da Nang',  N'02363123456', N'toyota.danang@gmail.com',  N'Nguyen Van An',  N'0400123456'),
(N'Dai ly Honda Mien Trung',  N'78 Le Van Hien, Da Nang',     N'02363234567', N'honda.mientrung@gmail.com',N'Tran Thi Binh',  N'0400234567'),
(N'Cong ty Xe Hoang Gia',     N'45 Nguyen Tat Thanh, Da Nang',N'02363345678', N'hoanggia.xe@gmail.com',    N'Le Minh Cuong',  N'0400345678');
GO

-- PhieuNhap
INSERT INTO PhieuNhap (SoPhieuNhap, MaNCC, MaNV, MaNVDuyet, NgayNhap, NgayDuyet, TrangThai, GhiChu) VALUES
(N'PN2024001', 1, 1, 2, '2024-01-15', '2024-01-16', N'Da duyet', N'Nhap lo xe dau nam 2024'),
(N'PN2024002', 2, 1, 2, '2024-03-20', '2024-03-21', N'Da duyet', N'Bo sung them xe gia dinh'),
(N'PN2024003', 3, 1, NULL, '2024-06-10', NULL,        N'Cho duyet', N'Nhap xe ban tai');
GO

-- CT_PhieuNhap (ThanhTien tu dong tinh qua computed column)
INSERT INTO CT_PhieuNhap (MaPhieuNhap, MaXe, SoLuong, DonGia, TinhTrangXe, SoKmHienTai) VALUES
(1, 1, 1, 750000000, N'Moi 100%', 0),
(1, 2, 1, 700000000, N'Moi 100%', 0),
(2, 6, 1, 580000000, N'Moi 100%', 0),
(2, 7, 1, 380000000, N'Moi 100%', 0),
(3, 5, 1, 820000000, N'Moi 100%', 0);
GO

-- DatXe
INSERT INTO DatXe (MaKH, MaXe, MaNV, DiaDiemNhan, DiaDiemTra, ThoiGianNhan, ThoiGianTra, GiaCoBan, TienGiam, TongTien, TrangThai, KenhDat) VALUES
(1, 1, 1, N'15 Tran Phu, Da Nang', N'15 Tran Phu, Da Nang', '2024-05-01 08:00', '2024-05-04 08:00', 3600000, 0,       3600000, N'Hoan thanh', N'Online'),
(2, 3, 1, N'San bay Da Nang',       N'San bay Da Nang',       '2024-05-10 09:00', '2024-05-13 09:00', 5400000, 500000, 4900000, N'Hoan thanh', N'Tai quay'),
(3, 7, 2, N'32 Le Loi, Da Nang',   N'32 Le Loi, Da Nang',   '2024-06-01 07:00', '2024-06-02 07:00',  500000, 0,        500000, N'Hoan thanh', N'Online'),
(1, 4, 1, N'Khach san Novotel',     N'Khach san Novotel',     '2024-07-15 10:00', '2024-07-18 10:00', 4800000, 0,       4800000, N'Da xac nhan',N'Online'),
(2, 6, 2, N'22 Hung Vuong, Da Nang',N'22 Hung Vuong, Da Nang','2024-08-01 08:00', '2024-08-03 08:00', 2000000, 200000, 1800000, N'Cho xac nhan',N'Online');
GO

-- HoaDon
INSERT INTO HoaDon (SoHoaDon, MaDatXe, MaKH, MaNV, LoaiHoaDon, TienTruocThue, ThueSuat, TienGiam, TongCong, TrangThai, NgayLap, NgayThanhToan) VALUES
(N'HD2024001', 1, 1, 1, N'Thue xe', 3600000,  10, 0,       3960000,  N'Da thanh toan', '2024-05-01', '2024-05-01'),
(N'HD2024002', 2, 2, 1, N'Thue xe', 4900000,  10, 500000,  4890000,  N'Da thanh toan', '2024-05-10', '2024-05-10'),
(N'HD2024003', 3, 3, 2, N'Thue xe',  500000,  10, 0,        550000,  N'Da thanh toan', '2024-06-01', '2024-06-01');
GO

-- CT_HoaDon
INSERT INTO CT_HoaDon (MaHoaDon, LoaiKhoan, MoTa, SoLuong, DonGia, ChietKhau) VALUES
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

-- ThanhToan
INSERT INTO ThanhToan (MaHoaDon, MaNV, PhuongThuc, MaGiaoDich, SoTien, TrangThai, ThoiGianTT) VALUES
(1, 1,    N'VNPay',          N'VNP20240501001', 3960000, N'Thanh cong', '2024-05-01 08:30'),
(2, 1,    N'Tien mat',       NULL,              4890000, N'Thanh cong', '2024-05-10 09:15'),
(3, NULL, N'Chuyen khoan',   N'BIDV20240601001',  550000, N'Thanh cong', '2024-06-01 07:45');
GO

-- DanhGia
INSERT INTO DanhGia (MaDatXe, MaKH, MaXe, DiemSao, NoiDung) VALUES
(1, 1, 1, 5, N'Xe dep, sach se, lai thoai mai. Nhan vien nhiet tinh. Rat hai long!'),
(2, 2, 3, 4, N'Xe tot, khong gian rong. Se thue lai lan sau.'),
(3, 3, 7, 5, N'Xe nho gon, phu hop di trong thanh pho. Gia ca hop ly.');
GO

-- LichSuBaoDuong
INSERT INTO LichSuBaoDuong (MaXe, MaNV, LoaiBaoDuong, MoTa, ChiPhi, NgayBaoDuong, NgayBaoDuongTiep, TrangThai) VALUES
(1, 2, N'Dinh ky',  N'Thay dau dong co, loc gio, kiem tra phanh',     2500000, '2024-04-01', '2024-10-01', N'Hoan thanh'),
(3, 2, N'Sua chua', N'Thay lop xe truoc phai, can chinh vo lang',      3800000, '2024-04-15', '2025-04-15', N'Hoan thanh'),
(7, 1, N'Dinh ky',  N'Thay dau dong co, kiem tra he thong dien',       1800000, '2024-05-20', '2024-11-20', N'Hoan thanh'),
(4, 2, N'Kiem tra', N'Kiem tra truoc mua he: dieu hoa, lop, phanh',     500000, '2024-06-01', NULL,          N'Hoan thanh');
GO

-- ============================================================
--  CAC VIEW HUU ICH (dung cho bao cao)
-- ============================================================

-- View: Thong tin day du cua khach hang
CREATE VIEW vw_KhachHang AS
SELECT
    kh.MaKH,
    nd.HoTen,
    tk.Email,
    nd.SoDienThoai,
    -- Địa chỉ mới
    nd.SoNha_Duong,
    nd.Phuong_Xa,
    nd.Quan_Huyen,
    nd.Tinh_ThanhPho,
    kh.SoCCCD,
    kh.SoBangLai,
    kh.HangBangLai,
    kh.TongLanThue,
    kh.TongChiTieu,
    kh.NgayTao
FROM KhachHang kh
JOIN NguoiDung nd ON kh.MaNguoiDung = nd.MaNguoiDung
JOIN TaiKhoan tk ON nd.MaTaiKhoan = tk.MaTaiKhoan;
GO

-- View: Danh sach don dat xe day du
CREATE VIEW vw_DatXe AS
SELECT
    dx.MaDatXe,
    nd.HoTen        AS TenKhachHang,
    tk.Email,
    nd.SoDienThoai,
    x.BienSo,
    x.TenXe,
    lx.TenLoai      AS LoaiXe,
    dx.DiaDiemNhan,
    dx.DiaDiemTra,
    dx.ThoiGianNhan,
    dx.ThoiGianTra,
    dx.SoNgayThue,
    dx.GiaCoBan,
    dx.TienGiam,
    dx.PhiPhatSinh,
    dx.TongTien,
    dx.TrangThai,
    dx.KenhDat,
    dx.NgayTao
FROM DatXe dx
JOIN KhachHang kh ON dx.MaKH   = kh.MaKH
JOIN NguoiDung nd ON kh.MaNguoiDung = nd.MaNguoiDung
JOIN TaiKhoan  tk ON nd.MaTaiKhoan  = tk.MaTaiKhoan
JOIN Xe        x  ON dx.MaXe   = x.MaXe
JOIN LoaiXe    lx ON x.MaLoai  = lx.MaLoai;
GO

-- View: Doanh thu theo thang
CREATE VIEW vw_DoanhThuTheoThang AS
SELECT
    YEAR(hd.NgayLap)    AS Nam,
    MONTH(hd.NgayLap)   AS Thang,
    COUNT(*)            AS SoHoaDon,
    SUM(hd.TongCong)    AS TongDoanhThu
FROM HoaDon hd
WHERE hd.TrangThai = N'Da thanh toan'
GROUP BY YEAR(hd.NgayLap), MONTH(hd.NgayLap);
GO

-- View: Xe duoc thue nhieu nhat
CREATE VIEW vw_XeThueNhieu AS
SELECT
    x.MaXe,
    x.BienSo,
    x.TenXe,
    x.Hang,
    x.GiaThueNgay,
    x.DanhGiaTB,
    COUNT(dx.MaDatXe)       AS SoLanThue,
    SUM(dx.TongTien)        AS TongDoanhThu
FROM Xe x
LEFT JOIN DatXe dx ON x.MaXe = dx.MaXe
    AND dx.TrangThai = N'Hoan thanh'
GROUP BY x.MaXe, x.BienSo, x.TenXe, x.Hang, x.GiaThueNgay, x.DanhGiaTB;
GO

-- ============================================================
--  STORED PROCEDURE
-- ============================================================

-- SP: Kiem tra xe co trong trong khoang thoi gian khong
CREATE PROCEDURE sp_KiemTraXeTrong
    @MaXe        INT,
    @ThoiGianNhan DATETIME,
    @ThoiGianTra  DATETIME
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*) AS SoDonTrung
    FROM DatXe
    WHERE MaXe = @MaXe
      AND TrangThai NOT IN (N'Da huy')
      AND NOT (ThoiGianTra <= @ThoiGianNhan OR ThoiGianNhan >= @ThoiGianTra);
END;
GO

-- SP: Lay danh sach xe con trong theo ngay
CREATE PROCEDURE sp_XeConTrong
    @ThoiGianNhan DATETIME,
    @ThoiGianTra  DATETIME,
    @MaLoai       INT = NULL   -- NULL = lay tat ca loai
AS
BEGIN
    SET NOCOUNT ON;
    SELECT x.*, lx.TenLoai
    FROM Xe x
    JOIN LoaiXe lx ON x.MaLoai = lx.MaLoai
    WHERE x.TrangThai = N'San sang'
      AND (@MaLoai IS NULL OR x.MaLoai = @MaLoai)
      AND x.MaXe NOT IN (
          SELECT MaXe FROM DatXe
          WHERE TrangThai NOT IN (N'Da huy')
            AND NOT (ThoiGianTra <= @ThoiGianNhan OR ThoiGianNhan >= @ThoiGianTra)
      );
END;
GO

-- ============================================================
--  KIEM TRA DU LIEU DA INSERT
-- ============================================================
SELECT N'=== KIEM TRA DATABASE ===' AS ThongBao;
SELECT 'VaiTro'          AS Bang, COUNT(*) AS SoBanGhi FROM VaiTro          UNION ALL
SELECT 'TaiKhoan',                COUNT(*)              FROM TaiKhoan        UNION ALL
SELECT 'NguoiDung',               COUNT(*)              FROM NguoiDung       UNION ALL
SELECT 'KhachHang',               COUNT(*)              FROM KhachHang       UNION ALL
SELECT 'NhanVien',                COUNT(*)              FROM NhanVien        UNION ALL
SELECT 'LoaiXe',                  COUNT(*)              FROM LoaiXe          UNION ALL
SELECT 'Xe',                      COUNT(*)              FROM Xe              UNION ALL
SELECT 'NhaCungCap',              COUNT(*)              FROM NhaCungCap      UNION ALL
SELECT 'PhieuNhap',               COUNT(*)              FROM PhieuNhap       UNION ALL
SELECT 'CT_PhieuNhap',            COUNT(*)              FROM CT_PhieuNhap    UNION ALL
SELECT 'DatXe',                   COUNT(*)              FROM DatXe           UNION ALL
SELECT 'HoaDon',                  COUNT(*)              FROM HoaDon          UNION ALL
SELECT 'CT_HoaDon',               COUNT(*)              FROM CT_HoaDon       UNION ALL
SELECT 'ThanhToan',               COUNT(*)              FROM ThanhToan       UNION ALL
SELECT 'DanhGia',                 COUNT(*)              FROM DanhGia         UNION ALL
SELECT 'LichSuBaoDuong',          COUNT(*)              FROM LichSuBaoDuong;

-- Thu procedure tim xe trong
EXEC sp_XeConTrong '2024-09-01 08:00', '2024-09-04 08:00', NULL;
GO
