# 🚗 LuxeDrive — Hệ Thống Quản Lý Thuê Xe Ô Tô

<div align="center">

![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core_MVC-10.0-512BD4?style=for-the-badge&logo=dotnet)
![SQL Server](https://img.shields.io/badge/SQL_Server-2022-CC2927?style=for-the-badge&logo=microsoftsqlserver)
![EF Core](https://img.shields.io/badge/Entity_Framework_Core-10.0-512BD4?style=for-the-badge)
![TailwindCSS](https://img.shields.io/badge/Tailwind_CSS-CDN-06B6D4?style=for-the-badge&logo=tailwindcss)

**Đồ án môn Lập trình C# — Nhóm 3 thành viên**

[📄 Báo cáo](https://docs.google.com/document/d/1t3y4F3mz7IRLiuNVkHeHpkU_BsD48VAT5FWPjmF8ZC4/edit?usp=sharing) · [🎨 Figma](https://www.figma.com/design/iJJsyOlmU2e2YX6h9Aqvva) · [📋 Trello](https://trello.com/b/932nuUD1/quan-ly-thue-xe-o-to) · [💻 GitHub](https://github.com/LeHai1004/QuanLyThueXe)

</div>

---

## 📖 Giới thiệu

**LuxeDrive** là ứng dụng web quản lý dịch vụ thuê xe ô tô được xây dựng bằng **ASP.NET Core MVC**. Hệ thống hỗ trợ đầy đủ vòng đời của một đơn thuê xe — từ lúc khách hàng tìm kiếm và đặt xe trực tuyến, nhân viên tiếp nhận và bàn giao xe, đến khi hoàn tất chuyến đi, xuất hóa đơn và khách hàng để lại đánh giá.

### ✨ Tính năng nổi bật
- 🔐 **Xác thực phiên** (Session-based) với phân quyền 3 vai trò: Admin, Nhân viên, Khách hàng
- 🚙 **Đặt xe online** với kiểm tra lịch trống theo thời gian thực
- 💳 **Thanh toán** tiền mặt hoặc chuyển khoản (QR Code)
- 🧾 **Hóa đơn tự động** được tạo ngay khi hoàn thành chuyến đi
- 🔧 **Quản lý bảo dưỡng** xe với phân công nhân viên kỹ thuật
- 📊 **Dashboard** thống kê doanh thu, xe được thuê nhiều nhất
- ⭐ **Đánh giá & nhận xét** của khách hàng sau chuyến đi
- 📦 **Quản lý nhập xe** từ nhà cung cấp với phiếu nhập kho

---

## 👥 Thành viên nhóm

| MSSV | Họ tên | Vai trò | Phụ trách |
|------|--------|---------|-----------|
| 2415053122213 | **Lê Hoàng Hải** | Nhóm trưởng / Integration | Thiết kế DB, ERD, Business Logic, Tổng hợp tài liệu |
| 2415053122208 | **Lê Thế Duy** | Thành viên | Giao diện Admin, Giao diện Nhân viên, Use Case Admin+NV |
| 2415053122224 | **Đặng Thái Nguyên** | Thành viên | Giao diện Khách hàng, Use Case KH, Sơ đồ nghiệp vụ |

---

## 🛠️ Công nghệ sử dụng

| Công nghệ | Phiên bản | Mục đích |
|-----------|-----------|----------|
| ASP.NET Core MVC | .NET 10.0 | Web framework chính |
| Entity Framework Core | 10.0.8 | ORM — truy cập cơ sở dữ liệu |
| SQL Server | 2022 | Hệ quản trị cơ sở dữ liệu |
| BCrypt.Net-Next | 4.2.0 | Mã hóa mật khẩu |
| Tailwind CSS | CDN | Giao diện người dùng |
| Google Material Icons | CDN | Icon bộ |

---

## 📁 Cấu trúc thư mục

```
QuanLyThueXe/
├── CarRentalSystem/
│   └── CarRentalSystem/
│       ├── Business/             # Business logic (tính giá, kiểm tra lịch trống...)
│       ├── Constants/            # Hằng số trạng thái (BookingStatus, InvoiceStatus...)
│       ├── Controllers/          # Xử lý request HTTP
│       │   ├── AccountController.cs      # Đăng nhập, đăng ký, hồ sơ, đổi mật khẩu
│       │   ├── BookingController.cs      # Đặt xe, lịch sử, hóa đơn, đánh giá
│       │   ├── VehicleController.cs      # Quản lý xe (admin/staff) và tra cứu (khách)
│       │   ├── MaintenanceController.cs  # Quản lý bảo dưỡng, phân công kỹ thuật
│       │   ├── ImportReceiptController.cs# Quản lý nhập xe từ nhà cung cấp
│       │   ├── DashboardController.cs    # Thống kê & báo cáo
│       │   └── ...
│       ├── Data/                 # DbContext (Entity Framework)
│       ├── Helpers/              # Hàm tiện ích (format tiền, mã hóa, sinh mã...)
│       ├── Models/               # Các entity chính
│       ├── Validation/           # Custom validation attributes
│       ├── Views/                # Giao diện Razor (.cshtml)
│       │   ├── Account/          # Đăng nhập, hồ sơ cá nhân, đổi mật khẩu
│       │   ├── Booking/          # Đặt xe, lịch sử, hóa đơn, đánh giá
│       │   ├── Vehicle/          # Danh sách xe, chi tiết xe
│       │   ├── Maintenance/      # Quản lý bảo dưỡng
│       │   └── Shared/           # Layout dùng chung
│       └── Program.cs            # Cấu hình ứng dụng
├── Database/
│   └── script.sql               # Script tạo bảng + dữ liệu mẫu
├── Docs/
│   ├── ERD.png                  # Sơ đồ quan hệ thực thể
│   ├── SoDoUseCase_Admin_NV.jpg # Use case Admin & Nhân viên
│   ├── SoDoUseCase_KhachHang.jpg# Use case Khách hàng
│   ├── SoDoNghiepVu.jpg         # Sơ đồ nghiệp vụ
│   └── PhanCongTask.md          # Bảng phân công công việc
└── UI/                          # Ảnh giao diện thiết kế
```

---

## 🗃️ Mô hình dữ liệu

Hệ thống gồm **16+ bảng** chính:

| Bảng | Mô tả |
|------|-------|
| `Account` | Tài khoản đăng nhập (username, password hash) |
| `UserProfile` | Thông tin cá nhân (họ tên, SĐT, CCCD, bằng lái) |
| `Customer` | Khách hàng thuê xe |
| `Staff` | Nhân viên (có chuyên môn bảo dưỡng) |
| `Role` | Vai trò hệ thống (Admin, Staff, Customer) |
| `VehicleCategory` | Danh mục xe (Sedan, SUV, Pickup...) |
| `Vehicle` | Thông tin xe (BKS, giá/ngày, trạng thái, hình ảnh) |
| `Booking` | Đơn đặt thuê xe |
| `Invoice` | Hóa đơn thanh toán |
| `InvoiceDetail` | Chi tiết dòng hóa đơn |
| `Review` | Đánh giá của khách hàng |
| `MaintenanceLog` | Nhật ký bảo dưỡng xe |
| `Supplier` | Nhà cung cấp xe |
| `ImportReceipt` | Phiếu nhập xe |
| `ImportReceiptDetail` | Chi tiết phiếu nhập |
| `Payment` | Thông tin thanh toán |

---

## 🔄 Quy trình nghiệp vụ thuê xe

```
Khách hàng        Nhân viên / Admin
     │                    │
     ▼                    │
 Tìm kiếm xe              │
     │                    │
     ▼                    │
 Đặt xe (chọn            │
 ngày, thanh toán)        │
     │                    │
     ▼                    │
 Chờ xác nhận ────────► Duyệt đơn
     │                    │
     ▼                    ▼
 Đã xác nhận ────────► Bàn giao xe
     │                    │
     ▼                    │
 Đang thuê                │
     │                    ▼
     └──────────────► Nhận lại xe
                          │
                          ▼
                    Hoàn thành ──► Hóa đơn tự động
                          │
                          ▼
                    Khách đánh giá
```

---

## 🚀 Hướng dẫn cài đặt và chạy

### Yêu cầu hệ thống
- **.NET 10.0 SDK** — [Tải tại đây](https://dotnet.microsoft.com/download/dotnet/10.0)
- **SQL Server 2022** (hoặc SQL Server Express)
- **Visual Studio 2022** (hoặc VS Code + C# extension)

### Các bước thực hiện

**1. Clone repository**
```bash
git clone https://github.com/LeHai1004/QuanLyThueXe.git
cd QuanLyThueXe
```

**2. Tạo database**

Mở **SQL Server Management Studio** (SSMS), kết nối đến server local, sau đó chạy file:
```
Database/script.sql
```
File này sẽ tự động tạo database `QuanLyThueXe` với đầy đủ bảng và dữ liệu mẫu.

**3. Cấu hình Connection String**

Mở file `CarRentalSystem/CarRentalSystem/appsettings.json` và cập nhật thông tin kết nối:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TÊN_SERVER_CỦA_BẠN;Database=QuanLyThueXe;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```
> ⚠️ Thay `TÊN_SERVER_CỦA_BẠN` bằng tên SQL Server instance của bạn (ví dụ: `localhost`, `.\SQLEXPRESS`, `LAPTOP-ABC\SQLSERVER`)

**4. Chạy ứng dụng**

```bash
cd CarRentalSystem/CarRentalSystem
dotnet run
```

Hoặc mở file `CarRentalSystem.sln` bằng Visual Studio và nhấn **F5**.

Ứng dụng sẽ chạy tại: `https://localhost:7266`

---

## 👤 Tài khoản demo

| Vai trò | Tên đăng nhập | Mật khẩu |
|---------|--------------|----------|
| Admin | `admin` | `Admin@123` |
| Nhân viên | `nhanvien01` | `Staff@123` |
| Khách hàng | `khach001` | `Customer@123` |

---

## 🖼️ Giao diện chính

### Khách hàng
- **Trang chủ** — Banner, giới thiệu dịch vụ, xe nổi bật
- **Danh sách xe** — Tìm kiếm, lọc theo loại xe, giá/ngày
- **Chi tiết xe** — Thông tin xe, đánh giá, form đặt xe
- **Lịch sử đơn đặt** — Xem tất cả đơn, lọc theo trạng thái, đánh giá, thuê lại
- **Hóa đơn của tôi** — Danh sách hóa đơn, lọc đã/chưa thanh toán, xem chi tiết
- **Hồ sơ cá nhân** — Cập nhật thông tin, đổi mật khẩu

### Nhân viên
- **Quản lý đặt xe** — Duyệt đơn, bàn giao xe, nhận xe trả, xuất hóa đơn
- **Nhiệm vụ bảo dưỡng** — Danh sách xe cần bảo dưỡng được phân công

### Admin
- **Dashboard** — Doanh thu theo tháng, xe được thuê nhiều nhất, tổng quan hệ thống
- **Quản lý xe** — Thêm/sửa/xóa xe, phân loại, cập nhật trạng thái
- **Quản lý nhân viên** — Thêm/sửa thông tin nhân viên
- **Quản lý nhập kho** — Tạo phiếu nhập xe từ nhà cung cấp, duyệt phiếu
- **Quản lý nhà cung cấp** — Thêm/sửa/xóa nhà cung cấp

---

## 📅 Timeline phát triển

| Lần báo cáo | Ngày | Nội dung hoàn thành |
|-------------|------|---------------------|
| Lần 1 | 11/05/2026 | DB + ERD + Giao diện thiết kế + Phân task |
| Lần 2 | 18/05/2026 | Triển khai MVC, Views, dữ liệu mẫu |
| Lần 3 | 25/05/2026 | Business logic, Validation, Session, Helper, hoàn thiện tính năng |

---

## 🔗 Tài liệu liên quan

- 📄 [Báo cáo đồ án (Google Docs)](https://docs.google.com/document/d/1t3y4F3mz7IRLiuNVkHeHpkU_BsD48VAT5FWPjmF8ZC4/edit?usp=sharing)
- 🎨 [Thiết kế giao diện (Figma)](https://www.figma.com/design/iJJsyOlmU2e2YX6h9Aqvva)
- 📋 [Quản lý task (Trello)](https://trello.com/b/932nuUD1/quan-ly-thue-xe-o-to)
- 💻 [GitHub Repository](https://github.com/LeHai1004/QuanLyThueXe)

---

<div align="center">
  <sub>© 2026 · Đồ án Lập trình C# · Nhóm 3 thành viên</sub>
</div>
