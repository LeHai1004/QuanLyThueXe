using CarRentalSystem.Data;
using CarRentalSystem.Models;
using CarRentalSystem.Constants;
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

        public async Task<IActionResult> Index(string search, int page = 1)
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != RoleConstants.Admin && role != RoleConstants.Staff)
            {
                return RedirectToAction("Login", "Account");
            }

            var query = _context.Vehicles.Include(v => v.Category).AsQueryable();

            ViewBag.TotalVehicles = await query.CountAsync();
            ViewBag.AvailableVehicles = await query.CountAsync(v => v.Status == VehicleStatus.Available);
            ViewBag.RentedVehicles = await query.CountAsync(v => v.Status == VehicleStatus.Rented);
            ViewBag.MaintenanceVehicles = await query.CountAsync(v => v.Status == VehicleStatus.Maintenance);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(v => v.VehicleName.Contains(search) || v.LicensePlate.Contains(search));
                ViewBag.SearchString = search;
            }

            query = query.OrderByDescending(v => v.VehicleId);

            // Truyền Role vào ViewBag để View nhận diện
            ViewBag.UserRole = role;

            // Nếu Admin thì không cần pagination, show all
            if (role == RoleConstants.Admin)
            {
                var allVehicles = await query.ToListAsync();
                ViewBag.CurrentPage = 1;
                ViewBag.TotalPages = 1;
                return View("AdminStaffIndex", allVehicles);
            }

            // Staff thì có pagination
            int pageSize = 6;
            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var staffVehicles = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;

            return View("AdminStaffIndex", staffVehicles);
        }

        public async Task<IActionResult> CustomerList(int? categoryId, decimal? maxPrice, int? seats, string sortOrder, int page = 1)
        {
            int pageSize = 6;
            var query = _context.Vehicles
                .Include(v => v.Category)
                .Where(v => v.Status == VehicleStatus.Available)
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

            ViewBag.Categories = await _context.VehicleCategories.Where(c => c.IsActive).ToListAsync();
            ViewBag.CurrentCategory = categoryId ?? 0;
            ViewBag.CurrentMaxPrice = maxPrice ?? 10000000;
            ViewBag.CurrentSeats = seats ?? 0;
            ViewBag.CurrentSort = string.IsNullOrEmpty(sortOrder) ? "default" : sortOrder;

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;

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

            return View("CustomerDetails", vehicle);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != RoleConstants.Admin && role != RoleConstants.Staff) return RedirectToAction("Login", "Account");

            ViewBag.Categories = _context.VehicleCategories.Where(c => c.IsActive).ToList();
            return View("AdminStaffCreate");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehicle vehicle)
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != RoleConstants.Admin && role != RoleConstants.Staff)
                return RedirectToAction("Login", "Account");

            // Kiểm tra validate dữ liệu bắt buộc
            if (string.IsNullOrEmpty(vehicle.VehicleName) || string.IsNullOrEmpty(vehicle.LicensePlate))
            {
                ViewBag.Error = "Vui lòng nhập tên xe và biển kiểm soát!";
                ViewBag.Categories = await _context.VehicleCategories.Where(c => c.IsActive).ToListAsync();
                return View("AdminStaffCreate", vehicle);
            }

            // Kiểm tra biển kiểm soát có trùng không
            var existingVehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.LicensePlate == vehicle.LicensePlate);

            if (existingVehicle != null)
            {
                ViewBag.Error = "Biển kiểm soát này đã tồn tại trong hệ thống!";
                ViewBag.Categories = await _context.VehicleCategories.Where(c => c.IsActive).ToListAsync();
                return View("AdminStaffCreate", vehicle);
            }

            try
            {
                // Gán giá trị mặc định
                vehicle.CreatedAt = DateTime.Now;
                vehicle.UpdatedAt = DateTime.Now;

                // Nếu không có trạng thái, mặc định là Available
                if (string.IsNullOrEmpty(vehicle.Status))
                    vehicle.Status = VehicleStatus.Available;

                // Nếu không có hình ảnh, dùng ảnh mặc định
                if (string.IsNullOrEmpty(vehicle.HinhAnh))
                    vehicle.HinhAnh = "https://images.unsplash.com/photo-1550355291-bbee04a92027";

                _context.Vehicles.Add(vehicle);
                await _context.SaveChangesAsync();

                // Thành công - chuyển hướng về danh sách quản lý xe
                return RedirectToAction(role == RoleConstants.Admin ? "Index" : "Index", "Vehicle");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Lỗi khi lưu dữ liệu: {ex.Message}";
                ViewBag.Categories = await _context.VehicleCategories.Where(c => c.IsActive).ToListAsync();
                return View("AdminStaffCreate", vehicle);
            }
        }
    }
}