using CarRentalSystem.Data;
using CarRentalSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalSystem.Controllers
{
    public class VehicleController : Controller
    {
        private readonly CarRentalContext _context;

        public VehicleController(CarRentalContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != "Admin" && role != "Staff")
            {
                return RedirectToAction("Login", "Account");
            }

            var vehicles = _context.Vehicles
                .Include(v => v.Category)
                .OrderByDescending(v => v.VehicleId)
                .ToList();

            if (role == "Admin")
            {
                // Đã cập nhật thành AdminIndex
                return View("AdminIndex", vehicles);
            }

            ViewBag.TotalVehicles = vehicles.Count;
            ViewBag.AvailableVehicles = vehicles.Count(v => v.Status == "San sang" || v.Status == "Sẵn sàng");
            ViewBag.RentedVehicles = vehicles.Count(v => v.Status == "Dang thue" || v.Status == "Đang thuê");
            ViewBag.MaintenanceVehicles = vehicles.Count(v => v.Status == "Bao duong" || v.Status == "Bảo dưỡng");

            // Đã cập nhật thành StaffIndex
            return View("StaffIndex", vehicles);
        }

        // Action này có vẻ bị thừa vì hàm Index ở trên đã xử lý cho Admin rồi
        // Nhưng nếu bạn vẫn đang dùng nó ở đâu đó thì tôi cập nhật luôn tên View
        public IActionResult AdminList()
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != "Admin") return RedirectToAction("Login", "Account");

            var vehicles = _context.Vehicles.Include(v => v.Category).ToList();
            return View("AdminIndex", vehicles);
        }

        public async Task<IActionResult> CustomerList(int? categoryId, decimal? maxPrice, int? seats, string sortOrder, int page = 1)
        {
            int pageSize = 6;

            var query = _context.Vehicles
                .Include(v => v.Category)
                .Where(v => v.Status == "San sang" || v.Status == "Sẵn sàng")
                .AsQueryable();

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(v => v.CategoryId == categoryId.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(v => v.PricePerDay <= maxPrice.Value);
            }

            if (seats.HasValue && seats.Value > 0)
            {
                query = query.Where(v => v.Seats == seats.Value);
            }

            query = sortOrder switch
            {
                "price_asc" => query.OrderBy(v => v.PricePerDay),
                "price_desc" => query.OrderByDescending(v => v.PricePerDay),
                _ => query.OrderByDescending(v => v.AverageRating)
            };

            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var vehicles = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var categories = await _context.VehicleCategories.Where(c => c.IsActive).ToListAsync();

            ViewBag.Categories = categories;
            ViewBag.CurrentCategory = categoryId ?? 0;
            ViewBag.CurrentMaxPrice = maxPrice ?? 10000000;
            ViewBag.CurrentSeats = seats ?? 0;
            ViewBag.CurrentSort = string.IsNullOrEmpty(sortOrder) ? "default" : sortOrder;

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;

            // Đã cập nhật thành CustomerIndex
            return View("CustomerIndex", vehicles);
        }

        public async Task<IActionResult> CustomerDetails(int id)
        {
            var vehicle = await _context.Vehicles
                .Include(v => v.Category)
                .FirstOrDefaultAsync(v => v.VehicleId == id);

            if (vehicle == null)
            {
                return NotFound();
            }

            // Đã cập nhật cho chắc chắn
            return View("CustomerDetails", vehicle);
        }
    }
}