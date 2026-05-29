# Kịch bản Clip Báo Cáo Nhóm — Car Rental System

> Mục tiêu: Mỗi thành viên nói trực tiếp (voice-over) về các chức năng mình đã làm, chỉ share màn hình (không xuất hiện mặt), trình diễn nhanh trên app.
    
---

## Hướng dẫn chung cho nhóm
- Tổng thời lượng: 6–8 phút (không kéo dài).
- Hướng dẫn quay: chỉ quay màn hình (screen capture), không bật webcam; mỗi thành viên thu âm giọng nói khi trình bày phần của mình.
- Tên file nộp: `Presentation_YourGroupName.mp4`.
- Thứ tự trình bày: 1) Mở đầu (10–20s) — Giới thiệu nhóm và mục tiêu; 2) Admin (1:40–2:30); 3) Staff (1:40–2:30); 4) Customer (1:40–2:30); 5) Kết thúc (10–20s).
- Quy ước: khi một thành viên trình bày thì những thành viên khác tắt micro; chỉ hiện màn hình của app (trình duyệt ở http://localhost:5023).

---

## 1) Mở đầu (Người dẫn nhóm) — 15–20s
- Màn hình: mở trang chủ `http://localhost:5023`.
- Nội dung nói (ví dụ):
  - "Chào mọi người, chúng tôi là nhóm [tên nhóm]. Trong video này, mỗi thành viên sẽ trình bày các tính năng họ đã phát triển cho ứng dụng Car Rental System: vai trò Admin, Staff và Customer."
- Hành động trên màn hình: di chuột sơ qua header, menu để tạo ấn tượng.

---

## 2) Phần Admin (Người A) — 1:40–2:30
- Màn hình: đăng nhập bằng tài khoản Admin (vd: `admin@thuexe.vn`).
- Hành động mở: vào `Dashboard` -> `Quản lý tài khoản/xe/đặt xe` -> `Báo cáo doanh thu`.

### Các chức năng cần trình bày (thứ tự & gợi ý lời nói)
1) Dashboard (30s)
+ Màn hình: `Dashboard` (hiển thị tổng số đơn, tổng khách, biểu đồ doanh thu tháng, top xe thuê).
+ Nói mẫu: "Dashboard là điểm tập trung thông tin cho ban quản trị — nó hiển thị các chỉ số chính như tổng số đơn hàng trong kỳ, tổng số khách đã sử dụng dịch vụ, biểu đồ doanh thu theo tháng và danh sách top xe được thuê nhiều nhất. Trong phần này tôi sẽ minh họa cách đọc các chỉ số, lọc theo khoảng thời gian và giải thích cách các số liệu này phản ánh hiệu suất kinh doanh. Tôi cũng sẽ nêu ngắn gọn cách dữ liệu được tổng hợp từ các view như `VwMonthlyRevenue` và `VwTopRentedVehicle`."
+ Demo: phóng to biểu đồ, chọn khoảng thời gian để lọc.
2. Quản lý tài khoản (30s)
   - Màn hình: `Account Management` — danh sách user, chức năng tạo/sửa/xoá.
   - Lời nói mẫu: "Tôi đã làm CRUD cho `Account` và phân quyền theo `Role`. Tính năng bao gồm tạo tài khoản mới, đặt lại mật khẩu (cập nhật `PasswordHash`), và kích hoạt/khóa tài khoản."
   - Hành động: demo tạo user mẫu (chỉ thao tác UI), sửa trạng thái IsActive.
3. Quản lý xe & danh mục (30s)
   - Màn hình: `Vehicle` & `VehicleCategory` pages.
   - Lời nói mẫu: "Phần này xử lý danh mục loại xe và thông tin chi tiết xe — giá, số ghế, trạng thái. Tôi đảm bảo các ràng buộc dữ liệu và các computed column hoạt động đúng."
   # Kịch bản Clip Báo Cáo Nhóm — Car Rental System

   Mục tiêu: Mỗi thành viên trình bày trực tiếp (voice-over) về các chức năng mình đã làm, chỉ quay màn hình ứng dụng (không hiện mặt), trình diễn thao tác thực tế theo các mục trên thanh sidebar.

   Tổng thời lượng đề xuất: 6–8 phút.

   Thứ tự & thời lượng (gợi ý):
   + Mở đầu — 20s (Người dẫn)
   + Admin — 2:10 (Người A)
   + Staff — 2:00 (Người B)
   + Customer — 2:00 (Người C)
   + Kết thúc — 10–15s (Người dẫn)

   Hướng dẫn quay: chỉ ghi màn hình của cửa sổ trình duyệt (http://localhost:5023). Tắt webcam; tắt thông báo hệ thống; kiểm tra micro; đọc trước kịch bản.

   ---

   ## Mở đầu (Người dẫn) — 15–20s
   + Màn hình: mở `http://localhost:5023` (homepage).
   + Lời nói (bản mẫu): "Chào mọi người, chúng tôi là nhóm [Tên nhóm]. Video này giới thiệu các tính năng chính của hệ thống quản lý thuê xe, theo ba vai trò: Admin, Staff và Customer. Mỗi thành viên sẽ demo trực tiếp phần mình phát triển."
   + Hành động: di chuyển chuột qua header/menu để giới thiệu nhanh giao diện.

   ---

   ## Phần A — Admin (Người A) — 2:10
   Mục tiêu: Trình bày toàn bộ những mục có trên sidebar dành cho Admin, thao tác demo cụ thể và lời nói mẫu.

   1) Dashboard (30s)
   + Màn hình: `Dashboard` (hiển thị tổng số đơn, tổng khách, biểu đồ doanh thu tháng, top xe thuê).
   + Nói mẫu: "Dashboard cung cấp cái nhìn tổng quan về hoạt động: số đơn, doanh thu theo tháng, và top xe được thuê. Tôi xây dựng các view báo cáo (`VwMonthlyRevenue`, `VwTopRentedVehicle`) và hiển thị dữ liệu dạng biểu đồ/ô số."
   + Demo: phóng to biểu đồ, chọn khoảng thời gian để lọc.

   2) Quản lý Tài khoản (Accounts) (25s)
   + Màn hình: `Accounts` — danh sách user, search, phân trang.
   + Nói mẫu: "Phần quản lý tài khoản cho phép admin kiểm soát toàn bộ người dùng hệ thống: từ việc tạo tài khoản mới, gán role phù hợp, cho đến việc khóa tài khoản khi cần thiết. Tôi đã thực hiện logic xác thực các trường, lưu `PasswordHash` an toàn và cung cấp chức năng reset mật khẩu cũng như kích hoạt/tạm ngưng tài khoản. Trong demo này tôi sẽ trình diễn quy trình tạo một user test, thay đổi role và thực hiện thao tác disable/enable để minh họa các thao tác quản trị."
   + Demo: tạo tài khoản test, đổi role, bật/tắt `IsActive`.

   3) Quản lý Vai trò (Roles) (15s)
   + Màn hình: `Roles` — thêm/sửa quyền.
   + Nói mẫu: "Mục Roles cho phép admin định nghĩa các nhóm quyền và mô tả phạm vi truy cập cho từng vai trò. Tôi sẽ trình bày cách tạo vai trò mới, gán quyền và liên kết role đó với tài khoản để điều chỉnh truy cập tới các module như báo cáo, quản lý đơn hay cấu hình."

   4) Quản lý Xe & Danh mục (Vehicles & Vehicle Categories) (30s)
   + Màn hình: `Vehicles` và `Vehicle Categories` — danh sách, form thêm/sửa, upload ảnh, cập nhật trạng thái.
   + Nói mẫu: "Mục quản lý xe là nơi lưu trữ toàn bộ thông tin về fleet: biển số, tên xe, model, năm sản xuất, cấu hình ghế, giá thuê, hình ảnh và trạng thái hoạt động. Tôi đảm bảo có các ràng buộc dữ liệu cần thiết (ví dụ giá phải > 0) và tính năng thay đổi trạng thái khi xe vào bảo dưỡng hoặc được cho thuê. Danh mục xe giúp phân loại để khách dễ dàng tìm kiếm. Trong demo, tôi sẽ cập nhật một xe để minh họa quy trình sửa thông tin và thay đổi trạng thái."
   + Demo: chỉnh trạng thái xe từ 'San sang' thành 'Bao duong' rồi lưu.

   5) Quản lý Đặt xe (Bookings) (20s)
   + Màn hình: `Bookings` — filter theo trạng thái, gán nhân viên, hủy/hoàn thành.
   + Nói mẫu: "Phần Booking cho phép admin xem toàn bộ đơn đặt theo trạng thái (Chờ xác nhận, Đang thuê, Hoàn thành...) và thực hiện các can thiệp cần thiết như gán nhân viên, điều chỉnh ngày giờ hoặc hủy đơn. Tôi sẽ minh họa cách lọc danh sách theo trạng thái và cập nhật một đơn để chuyển sang trạng thái tiếp theo."
   + Demo: filter `Cho xac nhan`, mở 1 đơn và thay đổi trạng thái.

   6) Import Receipts & Suppliers (20s)
   + Màn hình: `Import Receipts`, `Suppliers` — tạo phiếu, quản lý NCC.
   + Nói mẫu: "Module phiếu nhập ghi nhận các xe được nhập từ nhà cung cấp, gồm chi tiết số lượng, đơn giá và tổng tiền. Hệ thống có trigger tự động cập nhật tổng tiền khi thêm/sửa chi tiết. Trong phần demo, tôi sẽ tạo một phiếu nhập giả lập, thêm chi tiết và giải thích cách hệ thống tính toán tổng tiền và cập nhật tồn kho."

   7) Invoicing & Payments (15s)
   + Màn hình: `Invoices`/`Payments` — xem hóa đơn, đánh dấu đã thu, xuất hóa đơn.
   + Nói mẫu: "Phần hóa đơn cho phép admin kiểm tra và xác nhận các khoản phải thu từ khách hàng, theo dõi trạng thái thanh toán và xuất biên lai. Tôi sẽ trình diễn cách truy xuất một hóa đơn, kiểm tra chi tiết các mục tính tiền và đánh dấu là đã thanh toán."

   8) Reports & Exports (25s)
   + Màn hình: `Reports` — doanh thu theo tháng, theo xe, số đơn, export CSV/Excel.
   + Nói mẫu: "Phần Reports cung cấp các báo cáo phân tích để hỗ trợ ra quyết định: doanh thu theo khoảng thời gian, so sánh giữa các tháng, tổng doanh thu theo từng xe hay theo chi nhánh. Ngoài ra, tôi đã tích hợp tính năng xuất dữ liệu ra CSV/Excel để phục vụ báo cáo offline hoặc nhập vào công cụ phân tích. Trong demo, tôi sẽ chạy báo cáo tháng hiện tại và xuất file mẫu để trình bày luồng xuất dữ liệu."
   + Demo: chạy report tháng hiện tại, click Export → lưu file (mô tả ngắn).

   Kết thúc phần Admin (10s): tóm tắt vai trò Admin trong 1 câu.

   ---

   ## Phần B — Staff (Người B) — 2:00
   Mục tiêu: Trình bày các mục sidebar của Staff và demo quy trình nghiệp vụ.

   1) Booking Management (40s)
   + Màn hình: `Bookings` dành cho Staff — danh sách, chi tiết đơn, cập nhật trạng thái.
   + Nói mẫu: "Phần quản lý đặt xe cho staff là trung tâm xử lý các yêu cầu khách hàng: từ việc nhận yêu cầu, liên hệ để xác nhận thông tin, tới điều phối xe và nhân viên giao nhận. Tôi đã triển khai các chức năng để staff có thể xem lịch sử đơn, thêm ghi chú, và cập nhật trạng thái theo tiến trình thực tế. Trong demo, tôi sẽ minh họa một kịch bản xử lý đơn: từ chờ xác nhận đến gán xe và hoàn tất."
   + Demo: chọn đơn `Cho xac nhan` → xác nhận và ghi chú xử lý.

   2) Import Receipts / Inventory (25s)
   + Màn hình: tạo phiếu nhập, thêm chi tiết, kiểm tra tồn kho.
   + Nói mẫu: "Khi nhận xe mới từ nhà cung cấp, staff sẽ tạo phiếu nhập, nhập chi tiết từng xe cùng đơn giá và số lượng. Hệ thống tính toán tổng tiền tự động và cập nhật tồn kho; điều này giúp kiểm soát chính xác nguồn cung. Tôi sẽ tạo một phiếu nhập mẫu và giải thích từng bước thao tác để đảm bảo dữ liệu nhập vào là đầy đủ và chính xác."
   + Demo: tạo phiếu nhập mẫu, thêm 1 chi tiết, lưu.

   3) Maintenance (30s)
   + Màn hình: `Maintenance` — tạo bản ghi, lịch bảo dưỡng, chi phí.
   + Nói mẫu: "Module bảo dưỡng cho phép staff ghi nhận mọi hoạt động sửa chữa, bảo trì: mô tả công việc, chi phí, ngày thực hiện và nhân viên thực hiện. Khi một xe vào bảo dưỡng, trạng thái của xe sẽ được cập nhật và loại trừ khỏi danh sách xe có sẵn cho thuê. Trong demo, tôi sẽ tạo một bản ghi bảo dưỡng, trình bày cách nhập chi phí và xem hiệu ứng lên trạng thái xe."
   + Demo: tạo log bảo dưỡng, mở chi tiết xe để hiển thị trạng thái.

   4) Customer Support & Payments (25s)
   + Màn hình: xem thông tin khách, xử lý yêu cầu, kiểm tra thanh toán.
   + Nói mẫu: "Phần hỗ trợ khách và thanh toán là nơi staff xử lý các tình huống phát sinh: phản hồi khiếu nại, hỗ trợ huỷ đổi lịch, và kiểm tra xác nhận thanh toán. Tôi đã tích hợp các thao tác để staff có thể gửi thông báo cho khách và ghi chú lịch sử tương tác nhằm thuận tiện cho việc đối soát. Trong demo, tôi sẽ mô tả quy trình kiểm tra thanh toán và xử lý một yêu cầu hoàn/hủy."

   Kết thúc Staff (10s): tóm tắt công việc chính.

   ---

   ## Phần C — Customer (Người C) — 2:00
   Mục tiêu: Trình diễn trải nghiệm người dùng từ tìm xe đến đặt xe và quản lý đơn.

   1) Đăng ký / Đăng nhập (20s)
   + Màn hình: `Account/Register` và `Account/Login`.
   + Nói mẫu: "Phần đăng ký và đăng nhập là bước đầu tiên để khách sử dụng dịch vụ: khách có thể tạo tài khoản bằng email hoặc số điện thoại, điền thông tin cá nhân và tải lên giấy tờ tùy thân nếu cần. Hệ thống thực hiện kiểm tra định dạng, tránh trùng email và lưu trữ mật khẩu ở dạng băm an toàn. Trong phần demo, tôi sẽ minh họa quy trình đăng ký ngắn gọn và cách đăng nhập bằng tài khoản mẫu để tiếp tục flow đặt xe."
   + Demo: mở form Register (hoặc demo login bằng `khach001@gmail.com`).

   2) Browse & Search (25s)
   + Màn hình: trang danh sách xe, filters (category, price range).
   + Nói mẫu: "Tính năng tìm kiếm và lọc giúp khách tiết kiệm thời gian: họ có thể tìm theo tên xe, loại, khoảng giá, số ghế hoặc tiện nghi. Tôi đã tối ưu giao diện bộ lọc để phản hồi nhanh và hiển thị kết quả cập nhật ngay lập tức. Trong demo, tôi sẽ áp dụng vài bộ lọc để minh họa cách khách có thể thu hẹp kết quả và chọn ra những xe phù hợp."
   + Demo: dùng bộ lọc để tìm 1 xe.

   3) Vehicle Details (20s)
   + Màn hình: trang chi tiết xe (ảnh, thông số, giá/ngày).
   + Nói mẫu: "Trang chi tiết cung cấp mọi thông tin cần thiết để khách đưa ra quyết định: gallery ảnh, thông số kỹ thuật, điều kiện thuê, giá theo từng ngày và các chính sách liên quan. Tôi đã bố trí thông tin sao cho rõ ràng và dễ đọc, kèm nút đặt xe nổi bật để khách có thể bắt đầu flow đặt ngay từ trang này."

   4) Booking Flow (45s)
   + Màn hình: chọn ngày nhận/trả, địa điểm, xem tổng tiền, gửi yêu cầu.
   + Nói mẫu: "Flow đặt xe được thiết kế để đơn giản và minh bạch: khách chọn ngày giờ nhận và trả, địa điểm, hệ thống tự động tính toán số ngày thuê và tổng chi phí (bao gồm phụ phí, giảm giá nếu có). Nếu có phương thức thanh toán trực tuyến, khách có thể thực hiện; nếu không, họ có thể đặt giữ chỗ và hoàn tất thanh toán sau. Sau khi gửi yêu cầu, trạng thái đơn sẽ phản ánh tại `My Bookings` để khách theo dõi. Tôi sẽ thực hiện một booking mẫu để trình bày từng bước."
   + Demo: thực hiện booking mẫu đến màn hình xác nhận (không cần hoàn tất thanh toán nếu test). 

   5) My Account / My Bookings (10s)
   + Màn hình: `Customerprofile` và `My Bookings`.
   + Nói mẫu: "Khách kiểm tra lịch sử đặt, trạng thái đơn, và cập nhật thông tin cá nhân."

   Kết thúc Customer (10s): nhắc lợi ích chính cho người dùng.

   ---

   ## Kết thúc (Người dẫn) — 10–15s
   + Màn hình: trở về `Dashboard` hoặc trang chủ.
   + Lời nói mẫu: "Cảm ơn đã xem. Tóm tắt: Admin quản lý toàn bộ hệ thống, Staff xử lý nghiệp vụ và hỗ trợ khách, Customer trải nghiệm đặt xe và quản lý hồ sơ. Nếu cần demo chi tiết nào nữa, nhóm chúng tôi sẵn sàng."

   ---

   ## Checklist quay & kỹ thuật
   + Server: `dotnet run` (http://localhost:5023) — đảm bảo server đang chạy.
   + Độ dài mỗi phần theo kịch bản; không quá 8 phút tổng.
   + Ghi chú âm thanh: tắt micro khi không trình bày; ghi lại từng phần nếu muốn cắt ghép.
   + Công cụ quay: OBS / Loom / Xbox Game Bar — chỉ capture cửa sổ trình duyệt.

   ---

   ## Phân công và file nộp (gợi ý)
   + Người dẫn: Giới thiệu + kết thúc (20s + 10s)
   + Người A (Admin): phần Admin (2:10)
   + Người B (Staff): phần Staff (2:00)
   + Người C (Customer): phần Customer (2:00)

   File nộp: `Presentation_YourGroupName.mp4`.

   ---

   Muốn tôi chuyển nội dung này thành checklist ngắn hoặc slide PowerPoint tự động không? Bạn muốn phân công tên cụ thể cho từng thành viên không? 



