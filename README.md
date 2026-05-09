\# 🚗 Hệ Thống Quản Lý Thuê Xe Ô Tô



> Đồ án môn Lập trình C# — Nhóm 3 thành viên



\---



\## 👥 Thành viên nhóm



| MSSV | Họ tên | Vai trò | Phụ trách |

|------|--------|---------|-----------|

| 2415053122213 | \[TÊN BẠN] | Nhóm trưởng / Integration | Thiết kế DB, ERD, Tổng hợp tài liệu |

| 2415053122208 | \[TÊN DUY] | Thành viên | Giao diện Admin, Giao diện Nhân viên |

| 2415053122224 | \[TÊN NGUYÊN] | Thành viên | Giao diện Khách hàng, Sơ đồ nghiệp vụ |



\---



\## 📌 Mô tả đề tài



Hệ thống quản lý thuê xe ô tô hỗ trợ 3 loại người dùng:



\- \*\*Admin\*\* — quản trị tài khoản, duyệt phiếu nhập xe, xem báo cáo thống kê

\- \*\*Nhân viên\*\* — tiếp nhận đặt xe, lập hóa đơn, ghi nhận bảo dưỡng

\- \*\*Khách hàng\*\* — tìm kiếm xe, đặt xe online, xem lịch sử, tải hóa đơn



\---



\## 🛠️ Công nghệ sử dụng



\- ASP.NET MVC 5 / C#

\- SQL Server 2022

\- Entity Framework 10

\- Bootstrap 5



\---



\## 📁 Cấu trúc thư mục



```

QuanLyThueXe/

├── Database/

│   └── QuanLyThueXe.sql     # Script tạo 16 bảng + dữ liệu mẫu

├── Docs/

│   ├── ERD.png               # Sơ đồ ERD

│   └── PhanCongTask.md       # Bảng phân công công việc

|    **└──** usecase\_Admin\_Nv.png  # Sơ đồ usecase Admin và nhân viên

|    └── usecase\_Customer.png  # Sơ đồ usecase khách hàng

|    └── Activity Diagram.png  # Sô đồ nghiệp vụ

└── UI/

&#x20;   ├── KhachHang/            # Ảnh giao diện khách hàng

&#x20;   ├── NhanVien/             # Ảnh giao diện nhân viên

&#x20;   └── Admin/                # Ảnh giao diện admin

```



\---



\## 🚀 Hướng dẫn chạy (cập nhật ở lần báo cáo 2)



> Giai đoạn 1: Tài liệu, thiết kế DB và giao diện.

> Code C# sẽ được thêm vào ở giai đoạn 2 (18/5).



\---



\## 📅 Timeline dự án



| Lần báo cáo | Ngày | Nội dung |

|-------------|------|---------|

| Lần 1 | 11/05 | DB + ERD + Giao diện + Phân task |

| Lần 2 | 18/05 | Chạy project, View, Hard code |

| Lần 3 | 25/05 | Business logic, Validation, Helper |



\---



\## 🔗 Tài liệu tham khảo



\- 📄 \[Báo cáo Google Docs] https://docs.google.com/document/d/1t3y4F3mz7IRLiuNVkHeHpkU\_BsD48VAT5FWPjmF8ZC4/edit?usp=sharing

\- 🎨 \[Figma — Giao diện] https://www.figma.com/design/iJJsyOlmU2e2YX6h9Aqvva/Qu%E1%BA%A3n-l%C3%AD-thu%C3%AA-xe-%C3%B4-t%C3%B4?node-id=0-1\&p=f\&t=SxVVMuKCD3DrIzUT-0

\- 📋 \[Trello — Phân task] https://trello.com/b/932nuUD1/qu%E1%BA%A3n-ly-thue-xe-o-to

