using CarRentalSystem.Data;
using CarRentalSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != "Admin" && role != "Staff") return RedirectToAction("Login", "Account");

            var bookings = _context.Bookings
                .Include(b => b.Customer).ThenInclude(c => c.UserProfile)
                .Include(b => b.Vehicle)
                .OrderByDescending(b => b.CreatedAt)
                .ToList();

            if (role == "Admin")
            {
                return View("AdminIndex", bookings);
            }

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

            double totalDays = (returnDateTime - pickupDateTime).TotalDays;
            if (totalDays <= 0) totalDays = 1;

            decimal totalAmount = vehicle.PricePerDay * (decimal)totalDays;

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
            if (HttpContext.Session.GetString("AccountId") == null) return RedirectToAction("Login", "Account");

            if (method == "Cash")
            {
                int bookingId = ProcessBookingToDB();
                if (bookingId > 0) return RedirectToAction("Success", new { id = bookingId });
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
            if (HttpContext.Session.GetString("AccountId") == null) return RedirectToAction("Login", "Account");
            ViewBag.TotalAmount = HttpContext.Session.GetString("TempTotal");
            return View("CustomerQRCode");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult QRCodeConfirm()
        {
            int bookingId = ProcessBookingToDB();
            if (bookingId > 0) return RedirectToAction("Success", new { id = bookingId });
            return RedirectToAction("CustomerList", "Vehicle");
        }

        private int ProcessBookingToDB()
        {
            var accountId = int.Parse(HttpContext.Session.GetString("AccountId")!);
            var customer = _context.Customers.Include(c => c.UserProfile).FirstOrDefault(c => c.UserProfile.AccountId == accountId);

            if (customer == null) return 0;

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
                Status = "Cho xac nhan",
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
            if (booking == null) return RedirectToAction("CustomerList", "Vehicle");
            return View("CustomerSuccess", booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Approve(int id)
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != "Admin" && role != "Staff")
            {
                return RedirectToAction("Login", "Account");
            }

            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == id);
            if (booking != null && booking.Status == "Cho xac nhan")
            {
                booking.Status = "Da xac nhan";
                booking.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != "Admin" && role != "Staff") return RedirectToAction("Login", "Account");

            var booking = _context.Bookings
                .Include(b => b.Customer).ThenInclude(c => c.UserProfile)
                .Include(b => b.Vehicle)
                .FirstOrDefault(b => b.BookingId == id);

            if (booking == null) return NotFound();
            return View("StaffDetails", booking);
        }

        [HttpGet]
        public IActionResult Invoice(int id)
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != "Admin" && role != "Staff") return RedirectToAction("Login", "Account");

            var booking = _context.Bookings
                .Include(b => b.Customer).ThenInclude(c => c.UserProfile)
                .Include(b => b.Vehicle)
                .FirstOrDefault(b => b.BookingId == id);

            if (booking == null) return NotFound();

            var invoice = _context.Invoices.FirstOrDefault(i => i.BookingId == id);
            if (invoice == null)
            {
                invoice = new Invoice
                {
                    InvoiceNumber = "INV" + DateTime.Now.ToString("yyMM") + id.ToString("D4"),
                    BookingId = id,
                    CustomerId = booking.CustomerId,
                    SubTotal = booking.BasePrice,
                    TaxRate = 10,
                    DiscountAmount = booking.DiscountAmount,
                    GrandTotal = booking.TotalAmount,
                    Status = "Da thanh toan",
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
    }
}