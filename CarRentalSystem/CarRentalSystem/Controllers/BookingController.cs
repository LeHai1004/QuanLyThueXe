using CarRentalSystem.Data;
using CarRentalSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CarRentalSystem.Constants;
using CarRentalSystem.Business;
using CarRentalSystem.Helpers;
using System;
using System.Threading.Tasks;

namespace CarRentalSystem.Controllers
{
    public class BookingController : Controller
    {
        private readonly CarRentalContext _context;

        public BookingController(CarRentalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create(int vehicleId)
        {
            if (HttpContext.Session.GetString("AccountId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var vehicle = _context.Vehicles.FirstOrDefault(v => v.VehicleId == vehicleId);
            if (vehicle == null)
            {
                return RedirectToAction("CustomerList", "Vehicle");
            }

            return View("CustomerCreate", vehicle);
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search, string status, string fromDate, string toDate)
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != RoleConstants.Admin && role != RoleConstants.Staff)
            {
                return RedirectToAction("Login", "Account");
            }

            // Khởi tạo truy vấn cơ bản
            var query = _context.Bookings
                .Include(b => b.Customer).ThenInclude(c => c.UserProfile)
                .Include(b => b.Vehicle)
                .AsQueryable();

            // Xử lý Tìm kiếm (Theo mã đơn hoặc Tên khách hàng)
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(b => b.BookingId.ToString().Contains(search)
                                      || b.Customer.UserProfile.FullName.Contains(search));
                ViewBag.SearchString = search;
            }

            // Xử lý Lọc theo Trạng thái
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(b => b.Status == status);
                ViewBag.CurrentStatus = status;
            }

            // Xử lý Lọc theo Ngày đặt (từ ngày)
            if (!string.IsNullOrEmpty(fromDate) && DateTime.TryParse(fromDate, out DateTime parsedFromDate))
            {
                query = query.Where(b => b.PickupDateTime.Date >= parsedFromDate.Date);
                ViewBag.FromDate = fromDate;
            }

            // Xử lý Lọc theo Ngày đặt (đến ngày)
            if (!string.IsNullOrEmpty(toDate) && DateTime.TryParse(toDate, out DateTime parsedToDate))
            {
                query = query.Where(b => b.PickupDateTime.Date <= parsedToDate.Date);
                ViewBag.ToDate = toDate;
            }

            // Thực thi truy vấn và sắp xếp
            var bookings = await query.OrderByDescending(b => b.CreatedAt).ToListAsync();

            // Điều hướng theo Role
            if (role == RoleConstants.Admin)
            {
                return View("AdminIndex", bookings);
            }

            // Dành cho Staff
            ViewBag.TotalBookings = bookings.Count;
            ViewBag.PendingBookings = bookings.Count(b => b.Status == BookingStatus.Pending);
            ViewBag.ActiveBookings = bookings.Count(b => b.Status == BookingStatus.Active);

            return View("StaffIndex", bookings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int vehicleId, DateTime pickupDateTime, DateTime returnDateTime)
        {
            if (HttpContext.Session.GetString("AccountId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var vehicle = _context.Vehicles.FirstOrDefault(v => v.VehicleId == vehicleId);
            if (vehicle == null)
            {
                return RedirectToAction("CustomerList", "Vehicle");
            }

            var availabilityBiz = new VehicleAvailabilityBusiness(_context);
            if (!availabilityBiz.IsVehicleAvailable(vehicleId, pickupDateTime, returnDateTime))
            {
                TempData["ErrorMessage"] = "Xe đã có lịch bận trong khoảng thời gian này.";
                return RedirectToAction("Create", new { vehicleId = vehicleId });
            }

            var bookingBiz = new BookingBusiness();
            int totalDays = bookingBiz.CalculateRentalDays(pickupDateTime, returnDateTime);
            decimal totalAmount = bookingBiz.CalculateBasePrice(totalDays, vehicle.PricePerDay);

            HttpContext.Session.SetString("TempVehicleId", vehicleId.ToString());
            HttpContext.Session.SetString("TempPickup", pickupDateTime.ToString("yyyy-MM-ddTHH:mm"));
            HttpContext.Session.SetString("TempReturn", returnDateTime.ToString("yyyy-MM-ddTHH:mm"));
            HttpContext.Session.SetString("TempTotal", totalAmount.ToString());

            return RedirectToAction("Payment");
        }

        [HttpGet]
        public IActionResult Payment()
        {
            if (HttpContext.Session.GetString("AccountId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.TotalAmount = HttpContext.Session.GetString("TempTotal");
            return View("CustomerPayment");
        }

        [HttpGet]
        public IActionResult PaymentProcessing(string method)
        {
            if (HttpContext.Session.GetString("AccountId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (method == "Cash")
            {
                int bookingId = ProcessBookingToDB();
                if (bookingId > 0)
                {
                    return RedirectToAction("Success", new { id = bookingId });
                }
                return RedirectToAction("CustomerList", "Vehicle");
            }
            else if (method == "Transfer")
            {
                return RedirectToAction("QRCode");
            }

            return RedirectToAction("Payment");
        }

        [HttpGet]
        public IActionResult QRCode()
        {
            if (HttpContext.Session.GetString("AccountId") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.TotalAmount = HttpContext.Session.GetString("TempTotal");
            return View("CustomerQRCode");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult QRCodeConfirm()
        {
            int bookingId = ProcessBookingToDB();
            if (bookingId > 0)
            {
                return RedirectToAction("Success", new { id = bookingId });
            }
            return RedirectToAction("CustomerList", "Vehicle");
        }

        private int ProcessBookingToDB()
        {
            var accountId = int.Parse(HttpContext.Session.GetString("AccountId")!);
            var customer = _context.Customers.Include(c => c.UserProfile).FirstOrDefault(c => c.UserProfile.AccountId == accountId);

            if (customer == null)
            {
                return 0;
            }

            int vehicleId = int.Parse(HttpContext.Session.GetString("TempVehicleId")!);
            DateTime pickup = DateTime.Parse(HttpContext.Session.GetString("TempPickup")!);
            DateTime returnDt = DateTime.Parse(HttpContext.Session.GetString("TempReturn")!);
            decimal total = decimal.Parse(HttpContext.Session.GetString("TempTotal")!);

            var booking = new Booking
            {
                CustomerId = customer.CustomerId,
                VehicleId = vehicleId,
                PickupLocation = "Tại cửa hàng",
                ReturnLocation = "Tại cửa hàng",
                PickupDateTime = pickup,
                ReturnDateTime = returnDt,
                BasePrice = total,
                TotalAmount = total,
                Status = BookingStatus.Pending,
                BookingChannel = "Online",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            HttpContext.Session.Remove("TempVehicleId");
            HttpContext.Session.Remove("TempPickup");
            HttpContext.Session.Remove("TempReturn");
            HttpContext.Session.Remove("TempTotal");

            return booking.BookingId;
        }

        [HttpGet]
        public IActionResult Success(int id)
        {
            var booking = _context.Bookings.Include(b => b.Vehicle).FirstOrDefault(b => b.BookingId == id);
            if (booking == null)
            {
                return RedirectToAction("CustomerList", "Vehicle");
            }
            return View("CustomerSuccess", booking);
        }

        // --- CÁC HÀM XỬ LÝ TRẠNG THÁI BOOKING CỦA ADMIN/STAFF --- //

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != RoleConstants.Admin && role != RoleConstants.Staff) return RedirectToAction("Login", "Account");

            var booking = await _context.Bookings
                .Include(b => b.Vehicle)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking != null && booking.Status == BookingStatus.Pending)
            {
                booking.Status = BookingStatus.Confirmed;
                booking.UpdatedAt = DateTime.Now;

                // Cập nhật trạng thái xe 
                if (booking.Vehicle != null)
                {
                    booking.Vehicle.Status = VehicleStatus.Rented;
                    booking.Vehicle.UpdatedAt = DateTime.Now;
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != RoleConstants.Admin && role != RoleConstants.Staff) return RedirectToAction("Login", "Account");

            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null && booking.Status == BookingStatus.Pending)
            {
                booking.Status = BookingStatus.Cancelled;
                booking.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HandoverVehicle(int id)
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != RoleConstants.Admin && role != RoleConstants.Staff) return RedirectToAction("Login", "Account");

            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null && booking.Status == BookingStatus.Confirmed)
            {
                booking.Status = BookingStatus.Active;
                booking.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnVehicle(int id)
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != RoleConstants.Admin && role != RoleConstants.Staff) return RedirectToAction("Login", "Account");

            var booking = await _context.Bookings
                .Include(b => b.Vehicle)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking != null && booking.Status == BookingStatus.Active)
            {
                booking.Status = BookingStatus.Completed;
                booking.UpdatedAt = DateTime.Now;

                // Trả xe về trạng thái Available khi đã hoàn thành chuyến đi
                if (booking.Vehicle != null)
                {
                    booking.Vehicle.Status = VehicleStatus.Available;
                    booking.Vehicle.UpdatedAt = DateTime.Now;
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // --- END CÁC HÀM XỬ LÝ TRẠNG THÁI --- //

        [HttpGet]
        public IActionResult Details(int id)
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != RoleConstants.Admin && role != RoleConstants.Staff)
            {
                return RedirectToAction("Login", "Account");
            }

            var booking = _context.Bookings
                .Include(b => b.Customer).ThenInclude(c => c.UserProfile)
                .Include(b => b.Vehicle)
                .FirstOrDefault(b => b.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }
            return View("StaffDetails", booking);
        }

        [HttpGet]
        public IActionResult Invoice(int id)
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != RoleConstants.Admin && role != RoleConstants.Staff)
            {
                return RedirectToAction("Login", "Account");
            }

            var booking = _context.Bookings
                .Include(b => b.Customer).ThenInclude(c => c.UserProfile)
                .Include(b => b.Vehicle)
                .FirstOrDefault(b => b.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            var invoice = _context.Invoices.FirstOrDefault(i => i.BookingId == id);
            if (invoice == null)
            {
                invoice = new Invoice
                {
                    InvoiceNumber = CodeGeneratorHelper.GenerateInvoiceCode(),
                    BookingId = id,
                    CustomerId = booking.CustomerId,
                    SubTotal = booking.BasePrice,
                    TaxRate = 10,
                    DiscountAmount = booking.DiscountAmount,
                    GrandTotal = booking.TotalAmount,
                    Status = InvoiceStatus.Paid,
                    IssueDate = DateTime.Now
                };
                _context.Invoices.Add(invoice);
                _context.SaveChanges();
            }

            ViewBag.InvoiceData = invoice;
            return View("StaffInvoice", booking);
        }

        [HttpGet]
        public IActionResult History()
        {
            var accountIdStr = HttpContext.Session.GetString("AccountId");
            if (string.IsNullOrEmpty(accountIdStr))
            {
                return RedirectToAction("Login", "Account");
            }

            int accountId = int.Parse(accountIdStr);
            var customer = _context.Customers
                .Include(c => c.UserProfile)
                .FirstOrDefault(c => c.UserProfile.AccountId == accountId);

            if (customer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var myBookings = _context.Bookings
                .Include(b => b.Vehicle)
                .Where(b => b.CustomerId == customer.CustomerId)
                .OrderByDescending(b => b.CreatedAt)
                .ToList();

            var completedIds = myBookings
                .Where(b => b.Status == BookingStatus.Completed)
                .Select(b => b.BookingId)
                .ToList();

            var reviewedIds = _context.Reviews
                .Where(r => completedIds.Contains(r.BookingId))
                .Select(r => r.BookingId)
                .ToHashSet();

            ViewBag.ReviewedBookingIds = reviewedIds;

            return View("CustomerHistory", myBookings);
        }

        [HttpGet]
        public IActionResult CustomerDetails(int id)
        {
            var accountIdStr = HttpContext.Session.GetString("AccountId");
            if (string.IsNullOrEmpty(accountIdStr))
            {
                return RedirectToAction("Login", "Account");
            }

            int accountId = int.Parse(accountIdStr);

            var booking = _context.Bookings
                .Include(b => b.Vehicle)
                .ThenInclude(v => v.Category)
                .Include(b => b.Customer)
                .ThenInclude(c => c.UserProfile)
                .FirstOrDefault(b => b.BookingId == id && b.Customer.UserProfile.AccountId == accountId);

            if (booking == null)
            {
                return RedirectToAction("History");
            }

            return View(booking);
        }

        [HttpGet]
        public IActionResult MyInvoices()
        {
            var accountIdStr = HttpContext.Session.GetString("AccountId");
            if (string.IsNullOrEmpty(accountIdStr))
            {
                return RedirectToAction("Login", "Account");
            }

            int accountId = int.Parse(accountIdStr);
            var customer = _context.Customers
                .Include(c => c.UserProfile)
                .FirstOrDefault(c => c.UserProfile.AccountId == accountId);

            if (customer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var invoices = _context.Invoices
                .Include(i => i.Booking)
                .ThenInclude(b => b.Vehicle)
                .Where(i => i.CustomerId == customer.CustomerId)
                .OrderByDescending(i => i.IssueDate)
                .ToList();

            return View("CustomerInvoices", invoices);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(int id)
        {
            var accountIdStr = HttpContext.Session.GetString("AccountId");
            if (string.IsNullOrEmpty(accountIdStr))
                return RedirectToAction("Login", "Account");

            int accountId = int.Parse(accountIdStr);
            var booking = _context.Bookings
                .Include(b => b.Customer).ThenInclude(c => c.UserProfile)
                .Include(b => b.Vehicle)
                .FirstOrDefault(b => b.BookingId == id
                    && b.Customer.UserProfile.AccountId == accountId);

            if (booking == null)
                return RedirectToAction("History");

            if (booking.Status == BookingStatus.Pending || booking.Status == BookingStatus.Confirmed)
            {
                booking.Status = BookingStatus.Cancelled;
                booking.UpdatedAt = DateTime.Now;

                if (booking.Vehicle != null)
                {
                    booking.Vehicle.Status = VehicleStatus.Available;
                    booking.Vehicle.UpdatedAt = DateTime.Now;
                }

                _context.SaveChanges();
                TempData["SuccessMessage"] = "Đã hủy đơn đặt xe thành công.";
            }

            return RedirectToAction("History");
        }

        [HttpGet]
        public IActionResult Review(int id)
        {
            var accountIdStr = HttpContext.Session.GetString("AccountId");
            if (string.IsNullOrEmpty(accountIdStr))
                return RedirectToAction("Login", "Account");

            int accountId = int.Parse(accountIdStr);
            var booking = _context.Bookings
                .Include(b => b.Vehicle)
                .Include(b => b.Customer).ThenInclude(c => c.UserProfile)
                .FirstOrDefault(b => b.BookingId == id
                    && b.Status == BookingStatus.Completed
                    && b.Customer.UserProfile.AccountId == accountId);

            if (booking == null)
                return RedirectToAction("History");

            var existing = _context.Reviews.FirstOrDefault(r => r.BookingId == id);
            if (existing != null)
            {
                TempData["ErrorMessage"] = "Bạn đã đánh giá chuyến đi này rồi.";
                return RedirectToAction("History");
            }

            return View("CustomerReview", booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Review(int id, int rating, string content)
        {
            var accountIdStr = HttpContext.Session.GetString("AccountId");
            if (string.IsNullOrEmpty(accountIdStr))
                return RedirectToAction("Login", "Account");

            int accountId = int.Parse(accountIdStr);
            var booking = _context.Bookings
                .Include(b => b.Customer).ThenInclude(c => c.UserProfile)
                .FirstOrDefault(b => b.BookingId == id
                    && b.Status == BookingStatus.Completed
                    && b.Customer.UserProfile.AccountId == accountId);

            if (booking == null)
                return RedirectToAction("History");

            var existing = _context.Reviews.FirstOrDefault(r => r.BookingId == id);
            if (existing != null)
                return RedirectToAction("History");

            var review = new Review
            {
                BookingId = id,
                CustomerId = booking.CustomerId,
                VehicleId = booking.VehicleId,
                StarRating = rating,
                Content = content,
                NgayReview = DateTime.Now
            };

            _context.Reviews.Add(review);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Cảm ơn bạn đã đánh giá chuyến đi!";
            return RedirectToAction("History");
        }

        [HttpGet]
        public IActionResult CustomerInvoiceDetail(int id)
        {
            var accountIdStr = HttpContext.Session.GetString("AccountId");
            if (string.IsNullOrEmpty(accountIdStr))
            {
                return RedirectToAction("Login", "Account");
            }

            int accountId = int.Parse(accountIdStr);

            var invoice = _context.Invoices
                .Include(i => i.Booking)
                    .ThenInclude(b => b.Vehicle)
                .Include(i => i.Customer)
                    .ThenInclude(c => c.UserProfile)
                .FirstOrDefault(i => i.InvoiceId == id && i.Customer.UserProfile.AccountId == accountId);

            if (invoice == null)
            {
                return RedirectToAction("MyInvoices");
            }

            return View(invoice);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var accountId = HttpContext.Session.GetString("AccountId");
            if (string.IsNullOrEmpty(accountId))
            {
                return RedirectToAction("Login", "Account");
            }

            var booking = await _context.Bookings
                .Include(b => b.Vehicle)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            if (booking.Status != BookingStatus.Pending)
            {
                TempData["ErrorMessage"] = "Bạn chỉ có thể thay đổi lịch trình cho các đơn đặt xe đang chờ xác nhận.";
                return RedirectToAction("CustomerDetails", new { id = booking.BookingId });
            }

            return View("CustomerEdit", booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int BookingId, DateTime PickupDateTime, DateTime ReturnDateTime)
        {
            var accountId = HttpContext.Session.GetString("AccountId");
            if (string.IsNullOrEmpty(accountId))
            {
                return RedirectToAction("Login", "Account");
            }

            var booking = await _context.Bookings
                .Include(b => b.Vehicle)
                .FirstOrDefaultAsync(b => b.BookingId == BookingId);

            if (booking == null)
            {
                return NotFound();
            }

            if (ReturnDateTime <= PickupDateTime)
            {
                ModelState.AddModelError("", "Thời gian trả xe phải sau thời gian nhận xe.");
                return View("CustomerEdit", booking);
            }

            var diffTime = ReturnDateTime - PickupDateTime;
            int diffDays = (int)Math.Ceiling(diffTime.TotalDays);
            if (diffDays <= 0) diffDays = 1;

            decimal basePrice = diffDays * (booking.Vehicle?.PricePerDay ?? 0);

            booking.PickupDateTime = PickupDateTime;
            booking.ReturnDateTime = ReturnDateTime;
            booking.BasePrice = basePrice;
            booking.TotalAmount = basePrice;
            booking.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đã cập nhật lịch trình chuyến đi thành công!";
            return RedirectToAction("CustomerDetails", new { id = booking.BookingId });
        }
    }
}