# 🎬 KỊCH BẢN THUYẾT TRÌNH THI CUỐI KỲ — HỆ THỐNG QUẢN LÝ THUÊ XE Ô TÔ

> **Môn**: Lập trình C#
> **Nhóm**: 3 thành viên
> **Thời lượng ước tính**: 12–15 phút
> **Cách trình bày**: Quay màn hình chạy web + tự nói giải thích + chỉ lên code

---

## 📋 PHÂN CÔNG THUYẾT TRÌNH

| STT | Thành viên | Phần thuyết trình | Thời lượng |
|-----|-----------|-------------------|------------|
| 1 | **Lê Hoàng Hải** | Giao diện **Khách hàng** (đăng ký, đặt xe, lịch sử, hóa đơn, đánh giá) | ~5 phút |
| 2 | **Lê Thế Duy** | Giao diện **Nhân viên** + Code Backend (**Business, Extensions**) | ~4.5 phút |
| 3 | **Đặng Thái Nguyên** | Giao diện **Admin** + Code Backend (**Constants, Helpers, Validation**) + Tổng kết | ~4.5 phút |

---

---

# 👤 PHẦN 1 — LÊ HOÀNG HẢI (Khách hàng)

---

## 🎬 MỞ ĐẦU (30 giây)

**[Mở trình duyệt → Vào trang chủ `https://localhost:7266`]**

> "Xin chào thầy/cô và các bạn. Tên em là Lê Hoàng Hải — nhóm trưởng. Em xin phép mở đầu phần demo với giao diện **Khách hàng** của hệ thống thuê xe ô tô."
>
> "Đây là **trang chủ** của hệ thống, sử dụng Layout `_Layout.cshtml` — chung cho toàn bộ giao diện khách hàng. Phần header có các link điều hướng: Trang chủ, Đặt xe, Dịch vụ, Về chúng tôi, Liên hệ."

---

## 🔐 1.1 — ĐĂNG KÝ TÀI KHOẢN

**[Bấm nút "Đăng ký" ở header → Hiện form đăng ký]**

> "Đầu tiên em sẽ demo chức năng **đăng ký tài khoản**."
>
> "Khi người dùng nhấn Đăng ký, hệ thống yêu cầu: Họ tên, Email, Số điện thoại, CCCD và Mật khẩu."

**[Nhập thông tin mẫu và bấm Đăng ký]**

> "Phía **code backend**, form này gọi đến `AccountController` → action `Register` (phương thức POST). Hệ thống sẽ:"
> - "Kiểm tra **mật khẩu xác nhận**"
> - "Kiểm tra **email đã tồn tại** chưa bằng LINQ `_db.Accounts.AnyAsync(a => a.Email == email)`"
> - "Nếu hợp lệ → **mã hóa mật khẩu** bằng `BCrypt.HashPassword(password)` — bảo mật tuyệt đối, không lưu mật khẩu thô"
> - "Tạo lần lượt 3 bản ghi: `Account` → `UserProfile` → `Customer`"
> - "Set `TempData["SuccessMessage"]` và chuyển về trang Login"

**[Sau khi đăng ký thành công → Trang Login hiện thông báo xanh "Đăng ký thành công"]**

> "Thông báo thành công hiện ra nhờ `TempData` — dữ liệu truyền tạm thời giữa 2 request trong ASP.NET."

---

## 🔑 1.2 — ĐĂNG NHẬP

**[Nhập email + mật khẩu vừa đăng ký → Bấm Đăng nhập]**

> "Ở action `Login` (POST) trong `AccountController`:"
> - "Hệ thống xác minh mật khẩu bằng `BCrypt.Verify(password, account.PasswordHash)`"
> - "Kiểm tra tình trạng khóa tài khoản (`IsActive`)"
> - "Lưu `AccountId`, `RoleName`, `FullName` vào **Session** để giữ phiên đăng nhập"

**[Đăng nhập thành công → Về trang chủ, header hiện tên user]**

> "Đăng nhập xong, header lấy tên user từ `Session`. Bấm vào tên sẽ xổ ra menu: Hồ sơ, Lịch sử thuê, Hóa đơn, Đánh giá, Đổi mật khẩu."

---

## 🚙 1.3 — TÌM KIẾM VÀ ĐẶT XE

**[Bấm "Đặt xe" trên header → Trang danh sách xe]**

> "Đây là trang **danh sách xe**, action `CustomerList` trong `VehicleController`."
>
> "Hệ thống **lọc** cực kỳ linh hoạt:"
> - "Theo **loại xe** (Sedan, SUV, Pickup...) — tham số `categoryId`"
> - "Theo **giá tối đa** — tham số `maxPrice`"
> - "Theo **số ghế** — tham số `seats`"
> - "**Sắp xếp** giá tăng/giảm hoặc đánh giá cao nhất"
>
> "Chỉ những xe có trạng thái `Available` mới hiện. Hệ thống cũng xử lý **phân trang** mỗi trang 6 xe."

**[Chọn loại xe SUV → Kết quả thay đổi → Bấm vào 1 xe]**

> "Đây là trang **chi tiết xe**, hiện đầy đủ ảnh, thông số hộp số, nhiên liệu, giá/ngày."

**[Chọn ngày nhận và ngày trả → Bấm "Đặt xe ngay"]**

> "Khi Đặt xe, form gọi `BookingController` → action `Create` (POST). Thông tin sẽ được tính toán qua lớp Business Logic và lưu tạm vào **Session** (chưa lưu DB) trước khi redirect sang trang **Thanh toán**."

---

## 💳 1.4 — THANH TOÁN

**[Trang thanh toán hiện lên]**

> "Khách có 2 lựa chọn thanh toán. Nếu chọn **Tiền mặt** → hệ thống gọi hàm `ProcessBookingToDB()` lưu Booking vào DB ngay."
>
> "Nếu chọn **Chuyển khoản** → sẽ chuyển sang trang mã QR xác nhận."

**[Chọn Tiền mặt → Trang "Đặt xe thành công"]**

> "Đặt xe thành công! Booking mới sinh ra có trạng thái `Pending` (Chờ xác nhận)."

---

## 📋 1.5 — LỊCH SỬ THUÊ XE & ĐÁNH GIÁ

**[Bấm menu → Lịch sử thuê xe]**

> "Trang lịch sử, action `History`. Sử dụng JavaScript để chuyển tab trạng thái: Tất cả, Chờ xác nhận, Đã hoàn thành..."
>
> "Với đơn **Chờ xác nhận**, khách có thể Hủy. Với đơn **Đã hoàn thành**, khách có thể **Thuê lại** hoặc **Đánh giá**."

**[Bấm nút "Đánh giá" trên 1 đơn hoàn thành → Đánh giá 5 sao → Gửi]**

> "Action `Review` tạo review vào database. Sau đó redirect về lịch sử và hiện ngay thông báo cảm ơn."

---

## 🧾 1.6 — HÓA ĐƠN & HỒ SƠ

**[Bấm menu → Hóa đơn của tôi]**

> "Trang `MyInvoices`. Liệt kê hóa đơn, phân loại Đã thanh toán / Chưa thanh toán. Hóa đơn được tự động tạo khi hoàn thành chuyến đi."

**[Bấm menu → Hồ sơ cá nhân]**

> "Trang hồ sơ cho phép sửa thông tin cá nhân. Điểm đặc biệt: nút **Lưu thay đổi** ban đầu bị vô hiệu hóa, chỉ sáng lên khi người dùng thực sự nhập thay đổi thông tin (nhờ bắt sự kiện JavaScript)."

> "Phần giao diện khách hàng của em xin kết thúc. Em nhường lời cho bạn Duy trình bày giao diện Nhân viên và cấu trúc code bên dưới."

---

---

# 👔 PHẦN 2 — LÊ THẾ DUY (Nhân viên + Business Logic + Extensions)

---

## 🎬 MỞ ĐẦU (20 giây)

**[Đăng xuất → Đăng nhập bằng tài khoản Nhân viên: `nhanvien01` / `Staff@123`]**

> "Chào thầy/cô, em là Lê Thế Duy. Tiếp nối bạn Hải, em sẽ demo luồng làm việc của **Nhân viên** và giải thích một số kiến trúc code Backend quan trọng."
>
> "Khi đăng nhập bằng tài khoản Nhân viên, hệ thống redirect sang Dashboard và dùng layout `_AdminLayout.cshtml` với thanh sidebar."

---

## 📝 2.1 — QUẢN LÝ ĐƠN ĐẶT XE (Luồng xử lý)

**[Bấm sidebar → Quản lý đặt xe]**

> "Trang danh sách đơn đặt, action `Index` trong `BookingController`."
>
> "Nhiệm vụ chính của nhân viên là xử lý luồng trạng thái đơn hàng:"

```
Chờ xác nhận  ──[Approve]──►  Đã xác nhận  ──[HandoverVehicle]──►  Đang thuê  ──[ReturnVehicle]──►  Hoàn thành
```

**[Bấm "Xem chi tiết" 1 đơn Chờ xác nhận → Bấm "Duyệt đơn"]**

> "Khi duyệt đơn (`Approve`), hệ thống đổi status booking thành `Confirmed`, đồng thời khóa xe lại (chuyển xe thành `Rented`) để không ai đặt trùng."

**[Bấm "Bàn giao xe" → Trạng thái thành Đang thuê]**

**[Bấm "Trả xe"]**

> "Khi khách trả xe (`ReturnVehicle`), hệ thống đổi booking thành `Completed`, mở khóa xe lại `Available`. Và đặc biệt: **tự động tạo hóa đơn** (`Invoice`) với trạng thái `Unpaid` (Chưa thanh toán)."

---

## 🔧 2.2 — NHIỆM VỤ BẢO DƯỠNG

**[Bấm sidebar → Nhiệm vụ của tôi]**

> "Trang nhiệm vụ bảo dưỡng, action `MyTasks` trong `MaintenanceController`."
>
> "Hiển thị danh sách các phiếu bảo dưỡng xe được giao cho nhân viên này. Khi sửa xong xe, nhân viên bấm **Đã hoàn thành**."

**[Bấm nút "Đã hoàn thành" → Hiện popup modal]**

> "Hệ thống sẽ hiện **popup modal Tailwind** xác nhận thay vì alert xấu xí của trình duyệt. Nếu xác nhận, action `CompleteTask` cập nhật trạng thái phiếu và tự động mở lại trạng thái xe thành `Available`."

---

## 🧠 2.3 — BUSINESS LOGIC (Tầng nghiệp vụ)

**[Mở Visual Studio → File `Business/BookingBusiness.cs`]**

> "Về phần kiến trúc Code Backend, em phụ trách tầng **Business Logic**."
>
> "Thay vì nhồi nhét công thức tính toán vào Controller, em tách toàn bộ ra thư mục `Business`. Ví dụ trong `BookingBusiness`:"

```csharp
public int CalculateRentalDays(DateTime pickupDate, DateTime returnDate)
{
    int days = (int)Math.Ceiling((returnDate - pickupDate).TotalDays);
    return days <= 0 ? 1 : days;  // Tối thiểu 1 ngày
}
```

> "Hàm này tính số ngày thuê: dùng `Math.Ceiling` để làm tròn lên, ví dụ thuê 1.5 ngày tính 2 ngày. Và luôn tối thiểu 1 ngày."

**[Mở file `Business/VehicleAvailabilityBusiness.cs`]**

> "Lớp `VehicleAvailabilityBusiness` kiểm tra **xe có trống** không:"

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

> "Hàm truy vấn DB kiểm tra chồng lịch: Ngày trả cũ > Ngày nhận mới VÀ Ngày nhận cũ < Ngày trả mới. Controller chỉ việc gọi hàm này."

**[Mở file `Business/DashboardBusiness.cs`]**

> "Lớp `DashboardBusiness` xử lý toán học cho báo cáo, ví dụ tính Tỷ lệ lấp đầy xe và % tăng trưởng doanh thu."

---

## 🧩 2.4 — EXTENSIONS (Hàm mở rộng)

**[Mở thư mục `Extensions` → File `SessionExtensions.cs`]**

> "Để code gọn gàng hơn, em viết các **Extension methods**. Ví dụ cho Session:"

```csharp
public static string GetFullName(this ISession session)
{
    return session.GetString("FullName") ?? "Người dùng"; 
}
```

> "Giúp gọi `HttpContext.Session.GetFullName()` nhanh chóng, tránh lỗi NullReference."

**[Mở file `StringExtensions.cs`]**

> "Hàm `MaskNationalId()` giúp ẩn số CCCD trên giao diện: đổi `"123456789012"` thành `"123******012"` để bảo mật thông tin khách hàng."

> "Phần trình bày của em xong. Em xin nhường lời cho bạn Nguyên trình bày giao diện Admin và các thành phần kỹ thuật khác."

---

---

# 🛡️ PHẦN 3 — ĐẶNG THÁI NGUYÊN (Admin + Constants + Helpers + Validation)

---

## 🎬 MỞ ĐẦU (20 giây)

**[Đăng xuất → Đăng nhập bằng tài khoản Admin: `admin` / `Admin@123`]**

> "Chào thầy/cô, em là Đặng Thái Nguyên. Khép lại bài thuyết trình, em sẽ demo tính năng quản trị tối cao của **Admin** và các cấu trúc **Validation, Constants** mà nhóm áp dụng."

---

## 📊 3.1 — DASHBOARD VÀ BÁO CÁO (Admin)

**[Trang Dashboard Admin hiện lên]**

> "Vừa đăng nhập, Admin thấy **Dashboard** tổng quan: đếm tổng đơn, xe đang thuê, xe sẵn sàng và tổng doanh thu. Bên dưới là danh sách 5 đơn hàng mới nhất."

**[Bấm sidebar → Báo cáo]**

> "Trang **Báo cáo doanh thu**, action `Report` trong `DashboardController`. Hiển thị: Tổng doanh thu, Lợi nhuận ròng, Tỷ lệ lấp đầy xe và doanh thu chia theo chi nhánh (kèm % tăng trưởng tháng trước)."
>
> "Ngoài ra, Admin có thể tải file CSV báo cáo bằng chức năng **Xuất báo cáo**. File CSV được tạo bằng `StringBuilder` và dùng UTF-8 BOM để không bị lỗi font tiếng Việt khi mở bằng Excel."

---

## 🚗 3.2 — QUẢN LÝ XE & NHẬP XE

**[Bấm sidebar → Quản lý xe]**

> "Trang quản lý xe, action `Index` trong `VehicleController`. Admin thấy danh sách toàn bộ xe (không phân trang), trong khi nhân viên bị giới hạn phân trang. Admin được quyền Thêm, Sửa, Xóa xe. Khi thêm xe sẽ kiểm tra **biển kiểm soát** không được trùng lặp."

**[Bấm sidebar → Phiếu nhập xe]**

> "Chức năng `ImportReceiptController` cho phép quản lý việc nhập thêm xe từ **Nhà cung cấp**. Có trạng thái duyệt phiếu rõ ràng."

**[Bấm sidebar → Yêu cầu bảo dưỡng]**

> "Khi nhân viên đề xuất sửa xe, yêu cầu hiện lên đây. Admin quyết định **Duyệt** (chuyển việc cho nhân viên + đổi trạng thái xe thành bảo dưỡng) hoặc **Từ chối**."

---

## 📌 3.3 — CONSTANTS (Hằng số trạng thái)

**[Mở Visual Studio → File `Constants/SystemConstants.cs`]**

> "Về phần kiến trúc Code Backend, em phụ trách **Constants** (Hằng số) và Validation."
>
> "Thay vì gõ text trực tiếp (như `"Cho xac nhan"`), nhóm gom tất cả thành các static class (Enum):"

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

> "Điều này chống lỗi sai chính tả và dễ dàng bảo trì — muốn đổi tên trạng thái chỉ cần đổi ở 1 nơi."

---

## 🛠️ 3.4 — HELPERS (Hàm tiện ích)

**[Mở file `Helpers/CodeGeneratorHelper.cs`]**

> "Lớp `Helpers` chứa các hàm dùng chung độc lập. Ví dụ sinh mã phiếu nhập và mã hóa đơn tự động:"

```csharp
public static string GenerateInvoiceCode()
{
    return "HD" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString("D3");
}
```

> "Kết hợp Timestamp và Mili-giây đảm bảo mã hóa đơn **độc nhất vô nhị** (VD: HD20260529143025123)."

**[Mở file `Helpers/CurrencyHelper.cs`]**

> "Format tiền tệ: `amount.ToString("N0") + "đ"`"

---

## ✅ 3.5 — CUSTOM VALIDATION

**[Mở file `Validation/CccdValidationAttribute.cs`]**

> "Để chuẩn hóa dữ liệu, thay vì validation mặc định, em tạo các **Custom Validation Attribute**."

```csharp
public class CccdValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        string cccd = value.ToString();
        if (!Regex.IsMatch(cccd, @"^\d{12}$"))
            return new ValidationResult("CCCD bắt buộc phải đủ 12 chữ số.");
        return ValidationResult.Success;
    }
}
```

> "Sử dụng **Regex `^\d{12}$`** để bắt buộc CCCD phải là chuỗi 12 số."

**[Mở file `Validation/DateGreaterThanAttribute.cs`]**

> "Validation phức tạp nhất là so sánh 2 ngày — ví dụ **Ngày trả xe phải sau Ngày nhận xe**:"

```csharp
public class DateGreaterThanAttribute : ValidationAttribute
{
    // ...
    var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
    var comparisonValue = property.GetValue(validationContext.ObjectInstance);

    if (currentValue <= comparisonValue)
        return new ValidationResult("Ngày kết thúc phải lớn hơn ngày bắt đầu.");
}
```

> "Sử dụng **Reflection** (`GetProperty`) để đọc giá trị của trường khác trên cùng Model và so sánh, rất cơ động."

---

## 🎯 TỔNG KẾT (30 giây)

> "Để kết luận, dự án Hệ thống Quản lý Thuê xe của nhóm em có 3 điểm mạnh chính:"
> 1. **Nghiệp vụ đầy đủ**: Từ khách hàng tự đặt, thanh toán → nhân viên duyệt, trả xe, bảo dưỡng → admin quản lý, thống kê. Hóa đơn sinh tự động.
> 2. **Bảo mật**: Mật khẩu mã hóa **BCrypt**, chống CSRF token, Session-based auth, và Validation từ Backend.
> 3. **Kiến trúc Clean**: Tách rời Business, Constants, Helpers, Extensions giúp code tái sử dụng cao, không bị phình to ở Controller.
>
> "Cảm ơn thầy/cô đã theo dõi phần trình bày của nhóm 3!"

---

# 📝 LƯU Ý KHI QUAY VIDEO

1. **Quay màn hình** — dùng OBS Studio hoặc Xbox Game Bar (Win+G)
2. **Micro rõ** — nói to, rõ ràng, không quá nhanh
3. **Chỉ lên code** — khi giải thích logic, mở file code tương ứng và bôi đen dòng đang nói
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
