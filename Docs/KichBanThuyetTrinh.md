# 🎬 KỊCH BẢN THUYẾT TRÌNH THI CUỐI KỲ — HỆ THỐNG QUẢN LÝ THUÊ XE Ô TÔ

> **Môn**: Lập trình C#
> **Nhóm**: 3 thành viên
> **Thời lượng ước tính**: 12–15 phút
> **Cách trình bày**: Quay màn hình chạy web + tự nói giải thích + chỉ lên code

---

## 📋 PHÂN CÔNG THUYẾT TRÌNH

| STT | Thành viên | Phần thuyết trình | Thời lượng |
|-----|-----------|-------------------|------------|
| 1 | **Đặng Thái Nguyên** | Giao diện **Khách hàng** (đăng ký, đặt xe, lịch sử, hóa đơn, đánh giá) | ~5 phút |
| 2 | **Lê Thế Duy** | Giao diện **Nhân viên + Admin** (dashboard, quản lý xe, đơn đặt, bảo dưỡng) | ~5 phút |
| 3 | **Lê Hoàng Hải** | **Business Logic, Validation, Constants, Helpers, Extensions** + tổng kết | ~4 phút |

---

---

# 👤 PHẦN 1 — ĐẶNG THÁI NGUYÊN (Khách hàng)

---

## 🎬 MỞ ĐẦU (30 giây)

**[Mở trình duyệt → Vào trang chủ `https://localhost:7266`]**

> "Xin chào thầy/cô và các bạn. Tên em là Đặng Thái Nguyên, hôm nay em sẽ demo phần giao diện **Khách hàng** của hệ thống thuê xe ô tô."
>
> "Đây là **trang chủ** của hệ thống, sử dụng Layout `_Layout.cshtml` — chung cho toàn bộ giao diện khách hàng. Phần header có các link điều hướng: Trang chủ, Đặt xe, Dịch vụ, Về chúng tôi, Liên hệ."

---

## 🔐 1.1 — ĐĂNG KÝ TÀI KHOẢN

**[Bấm nút "Đăng ký" ở header → Hiện form đăng ký]**

> "Đầu tiên em sẽ demo chức năng **đăng ký tài khoản**."
>
> "Khi người dùng nhấn Đăng ký, hệ thống hiển thị form yêu cầu nhập: Họ tên, Email, Số điện thoại, CCCD và Mật khẩu."

**[Nhập thông tin mẫu và bấm Đăng ký]**

> "Phía **code backend**, form này gọi đến `AccountController` → action `Register` (phương thức POST)."
>
> "Trong action này, hệ thống thực hiện tuần tự:"
> - "Kiểm tra **mật khẩu xác nhận** có trùng không"
> - "Kiểm tra **email đã tồn tại** trong database chưa bằng LINQ `_db.Accounts.AnyAsync(a => a.Email == email)`"
> - "Nếu hợp lệ → **mã hóa mật khẩu** bằng thư viện `BCrypt.Net` với hàm `BCrypt.HashPassword(password)` — không lưu mật khẩu thô"
> - "Tạo 3 bản ghi liên tiếp: `Account` → `UserProfile` → `Customer`"
> - "Cuối cùng set `TempData["SuccessMessage"]` và redirect về trang Login"

**[Sau khi đăng ký thành công → Trang Login hiện thông báo xanh "Đăng ký thành công"]**

> "Ở đây mọi người thấy thông báo thành công được hiển thị nhờ `TempData` — đây là cơ chế truyền dữ liệu giữa 2 request trong ASP.NET Core, dữ liệu chỉ tồn tại đến khi được đọc."

---

## 🔑 1.2 — ĐĂNG NHẬP

**[Nhập email + mật khẩu vừa đăng ký → Bấm Đăng nhập]**

> "Tiếp theo là chức năng **đăng nhập**. Action `Login` (POST) trong `AccountController` sẽ:"
> - "Tìm Account theo email bằng `FirstOrDefaultAsync` kèm Include Role và UserProfile"
> - "Xác minh mật khẩu bằng `BCrypt.Verify(password, account.PasswordHash)` — so sánh hash, không giải mã ngược"
> - "Kiểm tra tài khoản có bị khóa không (`IsActive`)"
> - "Nếu OK → lưu `AccountId`, `RoleName`, `FullName` vào **Session** bằng `HttpContext.Session.SetString()`"
> - "Nếu role là Admin/Staff → redirect sang Dashboard. Nếu Customer → redirect sang Trang chủ"

**[Đăng nhập thành công → Về trang chủ, header hiện tên user và avatar]**

> "Sau khi đăng nhập, header hiển thị tên người dùng đọc từ `Session.GetString("FullName")`, nhấn vào sẽ mở dropdown menu với các link đến: Hồ sơ, Lịch sử thuê, Hóa đơn, Đánh giá, Đổi mật khẩu, Đăng xuất."

---

## 🚙 1.3 — TÌM KIẾM VÀ ĐẶT XE

**[Bấm "Đặt xe" trên header → Trang danh sách xe]**

> "Đây là trang **danh sách xe** cho khách hàng. Action `CustomerList` trong `VehicleController`."
>
> "Trang này có hệ thống **lọc** rất đầy đủ:"
> - "Lọc theo **loại xe** (Sedan, SUV, Pickup...) — dùng tham số `categoryId`"
> - "Lọc theo **giá tối đa** — dùng tham số `maxPrice`"
> - "Lọc theo **số ghế** — dùng tham số `seats`"
> - "**Sắp xếp** theo giá tăng/giảm hoặc đánh giá — dùng switch expression `sortOrder switch { ... }`"
>
> "Chỉ xe nào có trạng thái `VehicleStatus.Available` mới hiện ra. Hệ thống cũng có **phân trang** (pagination) mỗi trang 6 xe."

**[Dùng bộ lọc chọn loại xe SUV → Kết quả thay đổi]**

**[Bấm vào 1 xe → Trang chi tiết xe]**

> "Đây là trang **chi tiết xe**, action `CustomerDetails`. Hiện đầy đủ thông tin xe: tên, biển số, loại xe, hộp số, nhiên liệu, số ghế, giá/ngày."

**[Chọn ngày nhận xe và ngày trả xe → Bấm "Đặt xe ngay"]**

> "Khi bấm Đặt xe, form gọi `BookingController` → action `Create` (POST). Ở đây hệ thống sử dụng lớp Business Logic:"
> - "`VehicleAvailabilityBusiness.IsVehicleAvailable()` — kiểm tra xe có bị trùng lịch không bằng cách query bảng Booking tìm đơn nào chồng ngày"
> - "`BookingBusiness.CalculateRentalDays()` — tính số ngày thuê bằng `Math.Ceiling((returnDate - pickupDate).TotalDays)`"
> - "`BookingBusiness.CalculateBasePrice()` — tính tiền = số ngày × giá/ngày"
> - "Lưu thông tin đặt xe tạm vào **Session** (chưa lưu DB) rồi redirect sang trang **Thanh toán**"

---

## 💳 1.4 — THANH TOÁN

**[Trang thanh toán hiện lên — 2 phương thức: Tiền mặt và Chuyển khoản]**

> "Trang thanh toán có 2 lựa chọn. Nếu chọn **Tiền mặt** → gọi `PaymentProcessing(method: 'Cash')` → lưu Booking vào DB ngay qua hàm `ProcessBookingToDB()`."
>
> "Nếu chọn **Chuyển khoản** → redirect sang trang QR Code, sau khi xác nhận mới lưu."

**[Chọn Tiền mặt → Trang "Đặt xe thành công"]**

> "Đặt xe thành công! Hệ thống tạo 1 bản ghi `Booking` với trạng thái `BookingStatus.Pending` (Chờ xác nhận) và dọn dẹp Session tạm."

---

## 📋 1.5 — LỊCH SỬ THUÊ XE

**[Bấm menu → Lịch sử thuê xe]**

> "Đây là trang **lịch sử đơn đặt**, action `History` trong `BookingController`."
>
> "Trang có hệ thống **tab lọc** bằng JavaScript: Tất cả, Chờ xác nhận, Đã xác nhận, Đang thuê, Đã hoàn thành. Mỗi card xe hiển thị mã đơn, tên xe, ngày nhận/trả, tổng tiền."
>
> "Các nút thao tác tùy theo trạng thái:"
> - "Đơn **Chờ xác nhận** → có nút Hủy đơn, Thay đổi lịch trình"
> - "Đơn **Đã hoàn thành** → có nút **Đánh giá** (link sang `/Booking/Review/{id}`) và **Thuê lại** (link sang `/Vehicle/CustomerDetails/{vehicleId}`)"
> - "Nút Đánh giá chỉ hiện khi chưa đánh giá — kiểm tra qua `ViewBag.ReviewedBookingIds` (một HashSet)"

---

## ⭐ 1.6 — ĐÁNH GIÁ CHUYẾN ĐI

**[Bấm nút "Đánh giá" trên 1 đơn đã hoàn thành]**

> "Trang đánh giá gọi action `Review` (GET) — kiểm tra booking phải ở trạng thái `Completed` và chưa có review."

**[Chọn sao + nhập nội dung → Bấm Gửi đánh giá]**

> "Action `Review` (POST) tạo bản ghi `Review` với StarRating, Content, NgayReview. Sau đó set `TempData["SuccessMessage"]` và redirect về trang lịch sử — thông báo thành công hiện ngay tại đây, không bị dính sang trang khác."

---

## 🧾 1.7 — HÓA ĐƠN CỦA TÔI

**[Bấm menu → Hóa đơn của tôi]**

> "Trang hóa đơn, action `MyInvoices`. Liệt kê tất cả hóa đơn của khách."
>
> "Có 3 tab lọc bằng JavaScript: **Tất cả**, **Đã thanh toán**, **Chưa thanh toán**. Mỗi hóa đơn hiện mã số, ngày lập, tên xe, tổng tiền, trạng thái."
>
> "Hóa đơn được **tự động tạo** khi nhân viên xác nhận trả xe (trạng thái Completed) — logic này nằm trong action `ReturnVehicle`."

**[Bấm tab "Chưa thanh toán" → Chỉ hiện đơn chưa thanh toán]**

**[Bấm "Xem chi tiết" → Trang chi tiết hóa đơn]**

---

## 👤 1.8 — HỒ SƠ CÁ NHÂN + ĐỔI MẬT KHẨU

**[Bấm avatar → Hồ sơ cá nhân]**

> "Trang hồ sơ, action `Profile`. Hiển thị thông tin cá nhân từ bảng `Customer` JOIN `UserProfile`."
>
> "Đặc biệt: nút **Lưu thay đổi** ban đầu bị **mờ và vô hiệu hóa**. Chỉ khi người dùng thực sự sửa thông tin trên form thì nút mới sáng lên — logic này xử lý bằng JavaScript lắng nghe sự kiện `input` trên các ô."

**[Sửa số điện thoại → Nút Lưu sáng lên → Bấm Lưu]**

> "Action `UpdateProfile` (POST) cập nhật `UserProfile` và `Customer`, đồng thời cập nhật lại tên mới vào Session."

**[Bấm menu → Đổi mật khẩu]**

> "Action `DoiMatKhau` (POST): Kiểm tra mật khẩu cũ bằng `BCrypt.Verify`, hash mật khẩu mới bằng `BCrypt.HashPassword`, rồi xóa toàn bộ Session bắt buộc đăng nhập lại."

> "Phần giao diện khách hàng của em đến đây là xong. Em xin nhường lời cho bạn Duy phần Admin và Nhân viên."

---

---

# 🛡️ PHẦN 2 — LÊ THẾ DUY (Admin + Nhân viên)

---

## 🎬 MỞ ĐẦU (20 giây)

**[Đăng xuất → Đăng nhập bằng tài khoản Admin: `admin` / `Admin@123`]**

> "Xin chào, em là Lê Thế Duy, em sẽ trình bày phần giao diện **Admin** và **Nhân viên**."
>
> "Khi đăng nhập với role Admin hoặc Staff, hệ thống redirect sang `DashboardController → Index` thay vì trang chủ khách hàng. Giao diện admin dùng layout riêng `_AdminLayout.cshtml` với sidebar bên trái."

---

## 📊 2.1 — DASHBOARD

**[Trang Dashboard Admin hiện lên]**

> "Đây là **Dashboard** — trang tổng quan. Action `Index` trong `DashboardController`."
>
> "Hiển thị 4 ô thống kê chính đọc trực tiếp từ database:"
> - "`TotalOrders` = `_context.Bookings.Count()` — tổng đơn đặt"
> - "`RentedCars` = đếm booking có status `BookingStatus.Active` — xe đang thuê"
> - "`AvailableCars` = đếm vehicle có status `VehicleStatus.Available` — xe sẵn sàng"
> - "`Revenue` = tổng `TotalAmount` của booking `Completed` — doanh thu"
>
> "Bên dưới là danh sách **5 đơn đặt gần nhất**, lấy bằng LINQ `.OrderByDescending(b => b.CreatedAt).Take(5)`."

---

## 📈 2.2 — BÁO CÁO DOANH THU (Admin)

**[Bấm sidebar → Báo cáo]**

> "Trang **Báo cáo**, action `Report`. Ở đây sử dụng lớp `DashboardBusiness` để tính toán:"
> - "`CalculateOccupancyRate(totalVehicles, rentedVehicles)` — tỷ lệ lấp đầy = xe đang thuê / tổng xe × 100"
> - "`CalculateGrowthRate(thisMonthRev, lastMonthRev)` — tỷ lệ tăng trưởng doanh thu so với tháng trước"
>
> "Có chức năng **xuất báo cáo CSV** bằng action `ExportReport` — tạo file CSV bằng `StringBuilder`, encode UTF-8 BOM để Excel đọc tiếng Việt đúng, trả về dạng `File()` để tải."

---

## 🚗 2.3 — QUẢN LÝ XE

**[Bấm sidebar → Quản lý xe]**

> "Trang quản lý xe, action `Index` trong `VehicleController`."
>
> "Phía trên có 4 ô thống kê: Tổng xe, Xe sẵn sàng, Đang thuê, Bảo dưỡng — mỗi ô đếm theo trạng thái từ Constants."
>
> "Admin thấy toàn bộ xe (không phân trang), Staff thì có phân trang mỗi trang 6 xe — logic phân quyền nằm trong controller: `if (role == RoleConstants.Admin) { ... }`."

**[Bấm "Thêm xe mới"]**

> "Form thêm xe gọi action `Create` (POST). Controller kiểm tra:"
> - "Tên xe và biển số không được trống"
> - "Biển kiểm soát không được trùng: `_context.Vehicles.FirstOrDefaultAsync(v => v.LicensePlate == vehicle.LicensePlate)`"
> - "Nếu không nhập hình ảnh → dùng ảnh mặc định"
> - "Tự động gán `Status = Available`, `CreatedAt = DateTime.Now`"

**[Bấm "Sửa" trên 1 xe → Form sửa thông tin]**

> "Action `Edit` (POST) tương tự nhưng thêm kiểm tra biển số trùng với xe **khác** (loại trừ chính nó): `v.LicensePlate == vehicle.LicensePlate && v.VehicleId != id`."

---

## 📝 2.4 — QUẢN LÝ ĐƠN ĐẶT XE

**[Bấm sidebar → Quản lý đặt xe]**

> "Trang danh sách đơn đặt, action `Index` trong `BookingController`."
>
> "Có thanh **tìm kiếm** (theo mã đơn hoặc tên khách) và **lọc** theo trạng thái, ngày."
>
> "Luồng xử lý trạng thái của một đơn đặt xe đi qua 4 bước — mỗi bước là 1 action riêng:"

```
Chờ xác nhận  ──[Approve]──►  Đã xác nhận  ──[HandoverVehicle]──►  Đang thuê  ──[ReturnVehicle]──►  Hoàn thành
       │
       └──[Reject]──► Đã hủy
```

**[Bấm "Xem chi tiết" 1 đơn → Trang chi tiết]**

> "Trang chi tiết đơn, action `Details`. Hiện đầy đủ thông tin khách hàng, xe, ngày thuê, tổng tiền."

**[Bấm nút "Duyệt đơn" trên 1 đơn Chờ xác nhận]**

> "Action `Approve` (POST): Đổi status thành `Confirmed`, đồng thời đổi trạng thái xe thành `VehicleStatus.Rented` — xe được giữ lại cho đơn này."

**[Bấm "Bàn giao xe" → Bấm "Trả xe"]**

> "Action `ReturnVehicle` (POST): Đổi status thành `Completed`, đổi xe về `Available`, và **tự động tạo hóa đơn** `Invoice` với trạng thái `InvoiceStatus.Unpaid`. Mã hóa đơn được sinh tự động bằng `CodeGeneratorHelper.GenerateInvoiceCode()`."

**[Bấm "Xuất hóa đơn" → Trang hóa đơn nhân viên]**

> "Action `Invoice` tạo hoặc lấy hóa đơn có sẵn cho đơn đặt này."

---

## 🔧 2.5 — QUẢN LÝ BẢO DƯỠNG

**[Bấm sidebar → Bảo dưỡng xe]**

> "Trang tạo phiếu bảo dưỡng, action `Index` trong `MaintenanceController`."
>
> "Admin tạo phiếu → chọn xe, chọn nhân viên phụ trách, loại bảo dưỡng, chi phí. Trạng thái phiếu là `Pending` (Chờ xử lý), đồng thời xe chuyển sang `VehicleStatus.Maintenance`."
>
> "Staff tạo phiếu → không chọn nhân viên (tự lấy ID của mình từ Session), trạng thái phiếu là `Requested` (Yêu cầu — cần Admin duyệt)."

**[Bấm sidebar → Yêu cầu bảo dưỡng (Admin)]**

> "Trang duyệt yêu cầu, action `Requests`. Admin thấy danh sách phiếu staff gửi lên. Có thể **Duyệt** (`ApproveRequest` → status thành `Pending` + xe chuyển sang Maintenance) hoặc **Từ chối** (`RejectRequest` → status thành `Rejected` + ghi lý do)."

---

### Chuyển sang Staff

**[Đăng xuất → Đăng nhập `nhanvien01` / `Staff@123`]**

> "Giờ em chuyển sang giao diện **Nhân viên**."

**[Bấm sidebar → Nhiệm vụ của tôi]**

> "Trang nhiệm vụ bảo dưỡng, action `MyTasks`. Hiện các phiếu được phân công cho nhân viên này."
>
> "Khi bấm **Đã hoàn thành** → hiện **popup modal xác nhận** (không phải alert mặc định). Nếu xác nhận → action `CompleteTask` đổi phiếu thành `Completed` và xe trở lại `Available`."

---

## 📦 2.6 — QUẢN LÝ NHẬP XE + NHÀ CUNG CẤP

**[Đăng nhập lại Admin → Sidebar → Quản lý nhà cung cấp]**

> "CRUD nhà cung cấp: Thêm, Sửa, Xóa. Dùng `SupplierController`."

**[Sidebar → Phiếu nhập xe]**

> "Phiếu nhập xe từ nhà cung cấp, `ImportReceiptController`. Mã phiếu sinh tự động bằng `CodeGeneratorHelper.GenerateReceiptCode()`. Phiếu có trạng thái: Chờ duyệt → Đã duyệt / Từ chối."

---

## 👥 2.7 — QUẢN LÝ NHÂN SỰ (Admin)

**[Sidebar → Quản lý nhân viên]**

> "Danh sách nhân viên, `StaffController`. Admin xem thông tin, chi tiết từng nhân viên."

**[Sidebar → Quản lý khách hàng]**

> "Danh sách khách hàng, `CustomerController`. Lọc theo hạng thành viên (VIP Gold, Silver, Bronze, Thường), trạng thái tài khoản."

> "Phần Admin và Nhân viên của em đến đây là xong. Em xin nhường lời cho bạn Hải — phần kiến trúc code."

---

---

# ⚙️ PHẦN 3 — LÊ HOÀNG HẢI (Business Logic, Validation, Constants, Helpers, Extensions)

---

## 🎬 MỞ ĐẦU (20 giây)

> "Xin chào, em là Lê Hoàng Hải — nhóm trưởng. Em sẽ trình bày phần **kiến trúc code phía sau** (backend) — gồm Business Logic, Validation, Constants, Helpers và Extensions."

---

## 🏗️ 3.1 — KIẾN TRÚC TỔNG QUAN

**[Mở Visual Studio → Show cây thư mục Solution Explorer]**

> "Dự án theo kiến trúc **ASP.NET Core MVC** (Model–View–Controller), tổ chức thành các thư mục rõ ràng:"

```
CarRentalSystem/
├── Business/       ← Logic nghiệp vụ tách riêng khỏi Controller
├── Constants/      ← Hằng số trạng thái, tránh dùng chuỗi "magic string"
├── Controllers/    ← Xử lý request HTTP
├── Data/           ← DbContext (Entity Framework Core)
├── Extensions/     ← Extension methods mở rộng các kiểu dữ liệu
├── Helpers/        ← Hàm tiện ích dùng chung (format tiền, sinh mã)
├── Models/         ← Entity / bảng trong database
├── Validation/     ← Custom validation attributes
└── Views/          ← Giao diện Razor (.cshtml)
```

> "Mục tiêu tách như vậy là tuân thủ nguyên tắc **Separation of Concerns** — mỗi thư mục một nhiệm vụ rõ ràng, dễ bảo trì và mở rộng."

---

## 📌 3.2 — CONSTANTS (Hằng số trạng thái)

**[Mở file `Constants/SystemConstants.cs` → Chỉ lên code]**

> "Đây là file **hằng số** của hệ thống. Thay vì viết chuỗi trực tiếp như `"Cho xac nhan"` rải rác khắp code, chúng em gom tất cả vào các **static class**:"

```csharp
public static class BookingStatus
{
    public const string Pending = "Cho xac nhan";
    public const string Confirmed = "Da xac nhan";
    public const string Active = "Dang thue";
    public const string Completed = "Hoan thanh";
    public const string Cancelled = "Da huy";
}
```

> "Lợi ích: Tránh sai chính tả, dễ refactor — nếu muốn đổi giá trị chỉ sửa **1 chỗ duy nhất**."
>
> "Tương tự có `VehicleStatus`, `InvoiceStatus`, `MaintenanceStatus`, `PaymentMethod`, `MaintenanceType`, `RoleConstants`."

---

## 🧠 3.3 — BUSINESS LOGIC (Tầng nghiệp vụ)

**[Mở file `Business/BookingBusiness.cs`]**

> "Đây là lớp **nghiệp vụ đặt xe** — tách logic tính toán ra khỏi Controller để Controller chỉ lo điều hướng."

```csharp
public int CalculateRentalDays(DateTime pickupDate, DateTime returnDate)
{
    int days = (int)Math.Ceiling((returnDate - pickupDate).TotalDays);
    return days <= 0 ? 1 : days;  // Tối thiểu 1 ngày
}

public decimal CalculateBasePrice(int days, decimal pricePerDay)
{
    return days * pricePerDay;
}
```

> "Hàm `CalculateRentalDays`: Dùng `Math.Ceiling` để làm tròn lên — ví dụ thuê 1.5 ngày thì tính 2 ngày. Tối thiểu luôn là 1 ngày."
>
> "Hàm `CalculateBasePrice`: Đơn giản nhân số ngày với giá/ngày."

**[Mở file `Business/VehicleAvailabilityBusiness.cs`]**

> "Lớp kiểm tra **xe có trống** trong khoảng thời gian khách chọn không."

```csharp
public bool IsVehicleAvailable(int vehicleId, DateTime pickup, DateTime returnDate)
{
    bool isConflict = _context.Bookings.Any(b =>
        b.VehicleId == vehicleId &&
        b.Status != "Da huy" &&
        !(b.ReturnDateTime <= pickup || b.PickupDateTime >= returnDate));
    return !isConflict;
}
```

> "Logic: Tìm trong bảng Booking xem có đơn nào **chồng lịch** không. Điều kiện chồng lịch là: ngày trả cũ > ngày nhận mới VÀ ngày nhận cũ < ngày trả mới. Bỏ qua đơn đã hủy."
>
> "Controller chỉ cần gọi `IsVehicleAvailable(vehicleId, pickup, returnDate)` — không cần biết bên trong query thế nào."

**[Mở file `Business/DashboardBusiness.cs`]**

> "Lớp nghiệp vụ cho **Dashboard**:"
> - "`CalculateGrowthRate`: Tính % tăng trưởng doanh thu so tháng trước. Xử lý edge case: nếu tháng trước = 0 và tháng này > 0 → trả về 100%."
> - "`CalculateOccupancyRate`: Tính tỷ lệ lấp đầy = xe đang thuê / tổng xe × 100."

---

## ✅ 3.4 — VALIDATION (Kiểm tra dữ liệu)

**[Mở file `Validation/CccdValidationAttribute.cs`]**

> "Đây là **Custom Validation Attribute** — viết riêng cho dữ liệu đặc thù của Việt Nam."

```csharp
public class CccdValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        string cccd = value.ToString();
        if (!Regex.IsMatch(cccd, @"^\d{12}$"))
        {
            return new ValidationResult("CCCD bắt buộc phải đủ 12 chữ số.");
        }
        return ValidationResult.Success;
    }
}
```

> "Kế thừa `ValidationAttribute` của .NET. Dùng **Regex `^\d{12}$`** kiểm tra CCCD phải đúng 12 chữ số. Gắn vào Model bằng annotation `[CccdValidation]`."

**[Mở file `Validation/FutureDateAttribute.cs`]**

> "Validate ngày không được ở **quá khứ** — dùng cho ngày nhận xe:"

```csharp
if (date.Date < DateTime.Now.Date)
{
    return new ValidationResult("Ngày không được ở trong quá khứ.");
}
```

> "So sánh `.Date` (bỏ phần giờ/phút) để tránh lỗi nếu user chọn cùng ngày nhưng giờ trước."

**[Mở file `Validation/DateGreaterThanAttribute.cs`]**

> "Validate ngày trả phải **sau** ngày nhận:"

```csharp
public class DateGreaterThanAttribute : ValidationAttribute
{
    private readonly string _comparisonProperty;  // Tên property để so sánh

    protected override ValidationResult IsValid(...)
    {
        // Dùng Reflection lấy giá trị property khác trên cùng object
        var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
        var comparisonValue = property.GetValue(validationContext.ObjectInstance);

        if (currentValue <= comparisonValue)
            return new ValidationResult("Ngày kết thúc phải lớn hơn ngày bắt đầu.");
    }
}
```

> "Đặc biệt dùng **Reflection** — `GetProperty()` để lấy giá trị của property khác cùng Model và so sánh. Cách dùng: `[DateGreaterThan("PickupDateTime")]` trên property `ReturnDateTime`."

---

## 🔧 3.5 — HELPERS (Hàm tiện ích)

**[Mở file `Helpers/CurrencyHelper.cs`]**

> "Format tiền tệ Việt Nam: `amount.ToString("N0") + "đ"` — hiển thị dạng `1,200,000đ`. Có 2 overload cho `decimal` và `double`."

**[Mở file `Helpers/CodeGeneratorHelper.cs`]**

> "Sinh mã tự động cho hóa đơn và phiếu nhập:"

```csharp
public static string GenerateInvoiceCode()
{
    return "HD" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString("D3");
}
```

> "Format: `HD` + năm tháng ngày giờ phút giây + mili giây (3 chữ số) → đảm bảo **không trùng lặp**. Ví dụ: `HD20260529143025123`. Phiếu nhập tương tự nhưng prefix là `PN`."

---

## 🧩 3.6 — EXTENSIONS (Mở rộng)

**[Mở file `Extensions/SessionExtensions.cs`]**

> "Extension methods cho **Session** — thay vì viết `HttpContext.Session.GetString("FullName")` dài dòng, viết gọn `session.GetFullName()`."

```csharp
public static string GetFullName(this ISession session)
{
    return session.GetString("FullName") ?? "Người dùng";  // Giá trị mặc định nếu null
}
```

**[Mở file `Extensions/DateTimeExtensions.cs`]**

> "Format ngày tháng kiểu Việt Nam: `date.ToVnFormat()` → `"dd/MM/yyyy HH:mm"`."

**[Mở file `Extensions/StringExtensions.cs`]**

> "Ẩn CCCD để bảo mật: `"123456789012".MaskNationalId()` → `"123******012"` — chỉ hiện 3 số đầu và 3 số cuối."

---

## 🔒 3.7 — BẢO MẬT

> "Về bảo mật, hệ thống áp dụng:"
> - "**Mật khẩu hash** bằng BCrypt — không lưu mật khẩu thô"
> - "**`[ValidateAntiForgeryToken]`** trên tất cả form POST — chống tấn công CSRF (Cross-Site Request Forgery)"
> - "**Session-based authentication** — kiểm tra `Session.GetString("AccountId")` ở đầu mỗi action"
> - "**Phân quyền** bằng `RoleConstants` — Admin, Staff, Customer có quyền truy cập khác nhau"
> - "**Ẩn CCCD** trên giao diện bằng `MaskNationalId()` Extension"

---

## 🎯 TỔNG KẾT (30 giây)

> "Tóm lại, hệ thống của nhóm em gồm:"
> - "**16+ bảng** database, thiết kế chuẩn với quan hệ khóa ngoại"
> - "**11 Controller** xử lý toàn bộ nghiệp vụ"
> - "**3 lớp Business** tách logic tính toán ra khỏi Controller"
> - "**3 Custom Validation** kiểm tra CCCD, ngày tương lai, so sánh ngày"
> - "**2 Helper** sinh mã tự động và format tiền"
> - "**3 Extension** mở rộng Session, DateTime, String"
> - "**Constants** gom tất cả hằng số trạng thái"
>
> "Tất cả tuân thủ kiến trúc **MVC**, tách rõ ràng Model–View–Controller, sử dụng Entity Framework Core để làm ORM, BCrypt để hash mật khẩu, và Session để quản lý phiên đăng nhập."
>
> "Cảm ơn thầy/cô đã lắng nghe. Nhóm em xin kết thúc phần trình bày."

---

---

# 📝 LƯU Ý KHI QUAY VIDEO

1. **Quay màn hình** — dùng OBS Studio hoặc Xbox Game Bar (Win+G)
2. **Micro rõ** — nói to, rõ ràng, không quá nhanh
3. **Chỉ lên code** — khi giải thích logic, mở file code tương ứng và chỉ vào dòng đang nói
4. **Không cần slides** — chạy web trực tiếp + mở code bên cạnh là đủ
5. **Mỗi người tự nói phần mình** — cắt ghép hoặc quay liên tục đều được
6. **Demo có dữ liệu sẵn** — đảm bảo database có dữ liệu mẫu trước khi quay

---

# 🔑 TÀI KHOẢN DEMO

| Vai trò | Email / Username | Mật khẩu |
|---------|-----------------|----------|
| Admin | `admin` | `Admin@123` |
| Nhân viên | `nhanvien01` | `Staff@123` |
| Khách hàng | `khach001` | `Customer@123` |
