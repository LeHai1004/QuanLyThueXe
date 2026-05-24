using CarRentalSystem.Data;
using CarRentalSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CarRentalSystem.Controllers
{
    public class StaffController : Controller
    {
        private readonly CarRentalContext _context;

        public StaffController(CarRentalContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != "Admin") return RedirectToAction("Login", "Account");

            // Lấy danh sách nhân viên kèm thông tin cá nhân và tài khoản
            var staffs = _context.Staff
                .Include(s => s.UserProfile)
                    .ThenInclude(u => u.Account)
                .OrderByDescending(s => s.StaffId)
                .ToList();

            // Lấy thêm tổng doanh thu tháng này để hiển thị trên thẻ báo cáo
            ViewBag.TotalRevenue = _context.Invoices
                .Where(i => i.Status == "Da thanh toan" || i.Status == "Đã thanh toán")
                .Sum(i => (decimal?)i.GrandTotal) ?? 0;

            return View("AdminIndex", staffs);
        }
    }
}