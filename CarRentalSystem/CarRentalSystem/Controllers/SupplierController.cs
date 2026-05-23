using CarRentalSystem.Data;
using CarRentalSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CarRentalSystem.Controllers
{
    public class SupplierController : Controller
    {
        private readonly CarRentalContext _context;

        public SupplierController(CarRentalContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != "Admin" && role != "Staff") return RedirectToAction("Login", "Account");

            var suppliers = _context.Suppliers.OrderByDescending(s => s.SupplierId).ToList();

            if (role == "Admin")
            {
                // Tính toán số liệu thống kê cho Admin
                ViewBag.TotalSuppliers = suppliers.Count;
                // Thay đoạn đếm cũ bằng đoạn này:
                ViewBag.ActiveSuppliers = suppliers.Count(s => s.IsActive == true); // Đang hợp tác
                ViewBag.InactiveSuppliers = suppliers.Count(s => s.IsActive == false); // Ngừng giao dịch

                // Đếm số phiếu nhập kho trong tháng này
                var currentMonth = DateTime.Now.Month;
                var currentYear = DateTime.Now.Year;
                ViewBag.MonthlyImports = _context.ImportReceipts
                    .Count(r => r.ImportDate.Month == currentMonth && r.ImportDate.Year == currentYear);

                return View("AdminIndex", suppliers);
            }

            return View("StaffIndex", suppliers);
        }
    }
}