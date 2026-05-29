using CarRentalSystem.Data;
using CarRentalSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using CarRentalSystem.Constants; // Gọi bộ hằng số chống sai chính tả
using CarRentalSystem.Business;  // Gọi bộ não xử lý tính toán

namespace CarRentalSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly CarRentalContext _context;

        public DashboardController(CarRentalContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != RoleConstants.Admin && role != RoleConstants.Staff)
            {
                return RedirectToAction("Login", "Account");
            }

            // Đã thay thế toàn bộ chữ thuần bằng Enum/Constants
            ViewBag.TotalOrders = _context.Bookings.Count();
            ViewBag.RentedCars = _context.Bookings.Count(b => b.Status == BookingStatus.Active);
            ViewBag.AvailableCars = _context.Vehicles.Count(v => v.Status == VehicleStatus.Available);
            ViewBag.Revenue = _context.Bookings.Where(b => b.Status == BookingStatus.Completed).Sum(b => (decimal?)b.TotalAmount) ?? 0;

            var sevenDaysAgo = DateTime.Now.AddDays(-7);
            ViewBag.NewCustomers = _context.Customers.Count(c => c.UserProfile.CreatedAt >= sevenDaysAgo);

            var recentOrders = _context.Bookings
                .Include(b => b.Customer)
                    .ThenInclude(c => c.UserProfile)
                .Include(b => b.Vehicle)
                .OrderByDescending(b => b.CreatedAt)
                .Take(5)
                .ToList();

            if (role == RoleConstants.Admin)
            {
                return View("AdminIndex", recentOrders);
            }

            return View(recentOrders);
        }

        public IActionResult Report()
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != "Admin") return RedirectToAction("Login", "Account");

            // 1. Khởi tạo bộ não Business Logic
            var dashboardBiz = new DashboardBusiness();

            // 2. Gom trạng thái hóa đơn bằng Constants
            var totalRevenue = _context.Invoices
                .Where(i => i.Status == InvoiceStatus.Paid)
                .Sum(i => (decimal?)i.GrandTotal) ?? 0;

            var netProfit = totalRevenue * 0.35m;

            var totalBookings = _context.Bookings.Count();

            // 3. Tính tỷ lệ lấp đầy thông qua Business Logic
            var totalVehicles = _context.Vehicles.Count();
            var rentedVehicles = _context.Vehicles.Count(v => v.Status == VehicleStatus.Rented);
            var occupancyRate = dashboardBiz.CalculateOccupancyRate(totalVehicles, rentedVehicles);

            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            // 4. Xử lý tăng trưởng chi nhánh: Giao phó phép chia cho Business
            var branchStats = _context.Bookings
                .Where(b => b.Status == BookingStatus.Completed)
                .AsEnumerable() // Chuyển về xử lý RAM để hàm GroupBy chạy mượt với logic phức tạp
                .GroupBy(b => b.PickupLocation)
                .Select(g => {
                    var thisMonthRev = g.Where(x => x.ReturnDateTime.Month == currentMonth && x.ReturnDateTime.Year == currentYear).Sum(x => x.TotalAmount);
                    var lastMonthYear = currentMonth == 1 ? currentYear - 1 : currentYear;
                    var lastMonthNum = currentMonth == 1 ? 12 : currentMonth - 1;
                    var lastMonthRev = g.Where(x => x.ReturnDateTime.Month == lastMonthNum && x.ReturnDateTime.Year == lastMonthYear).Sum(x => x.TotalAmount);

                    return new
                    {
                        BranchName = g.Key.Contains(",") ? "Chi nhánh " + g.Key.Split(',')[0].Trim() : g.Key,
                        Address = g.Key,
                        BookingCount = g.Count(),
                        Revenue = g.Sum(b => b.TotalAmount),

                        // Đẩy toàn bộ khối if/else rườm rà qua cho DashboardBusiness lo
                        Growth = Math.Round(dashboardBiz.CalculateGrowthRate(thisMonthRev, lastMonthRev), 1)
                    };
                })
                .OrderByDescending(x => x.Revenue)
                .Take(5)
                .ToList();

            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.NetProfit = netProfit;
            ViewBag.TotalBookings = totalBookings;
            ViewBag.OccupancyRate = occupancyRate;
            ViewBag.BranchStats = branchStats;

            // ✅ THÊM: load danh sách khách hàng truyền vào Model
            var customers = _context.Customers
                .Include(c => c.UserProfile)
                    .ThenInclude(u => u.Account)
                .Include(c => c.Bookings)
                .OrderByDescending(c => c.CustomerId)
                .ToList();

            return View(customers); // ✅ truyền customers thay vì View()
        }

        [HttpGet]
        public IActionResult ExportReport()
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != RoleConstants.Admin) return Unauthorized();

            var bookings = _context.Bookings
                .Include(b => b.Customer)
                    .ThenInclude(c => c.UserProfile)
                .Include(b => b.Vehicle)
                .OrderByDescending(b => b.CreatedAt)
                .ToList();

            var builder = new System.Text.StringBuilder();
            builder.AppendLine("Mã Đơn,Khách Hàng,Phương Tiện,Thời Gian Nhận,Thời Gian Trả,Tổng Tiền,Trạng Thái");

            foreach (var item in bookings)
            {
                var customerName = item.Customer?.UserProfile?.FullName ?? "Khách vãng lai";
                var vehicleName = item.Vehicle?.VehicleName ?? "N/A";
                // Escape commas by quoting
                builder.AppendLine($"RC-{item.BookingId.ToString("D4")},\"{customerName}\",\"{vehicleName}\",{item.PickupDateTime:dd/MM/yyyy HH:mm},{item.ReturnDateTime:dd/MM/yyyy HH:mm},{item.TotalAmount},{item.Status}");
            }

            byte[] preamble = System.Text.Encoding.UTF8.GetPreamble();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(builder.ToString());
            byte[] result = preamble.Concat(data).ToArray();

            return File(result, "text/csv", $"BaoCao_DonDatXe_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
        }
    }
}