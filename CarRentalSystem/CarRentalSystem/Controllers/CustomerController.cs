using CarRentalSystem.Data;
using CarRentalSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CarRentalSystem.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CarRentalContext _context;

        public CustomerController(CarRentalContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != "Admin" && role != "Staff") return RedirectToAction("Login", "Account");

            // Lấy danh sách khách hàng, bao gồm thông tin cá nhân và lịch sử đặt xe để tính tổng chi tiêu
            // Lấy danh sách khách hàng, bao gồm thông tin cá nhân, tài khoản và lịch sử đặt xe
            var customers = _context.Customers
                .Include(c => c.UserProfile)
                    .ThenInclude(u => u.Account) // <-- Thêm dòng này để lấy được Email
                .Include(c => c.Bookings)
                .OrderByDescending(c => c.CustomerId)
                .ToList();

            if (role == "Admin")
            {
                return View("AdminIndex", customers);
            }

            // Dành cho nhân viên
            return View("StaffIndex", customers);
        }
    }
}