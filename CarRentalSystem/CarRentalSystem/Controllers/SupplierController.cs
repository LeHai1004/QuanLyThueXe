using CarRentalSystem.Data;
using CarRentalSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using CarRentalSystem.Constants;

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
            if (role != RoleConstants.Admin && role != RoleConstants.Staff)
            {
                return RedirectToAction("Login", "Account");
            }

            var suppliers = _context.Suppliers.OrderByDescending(s => s.SupplierId).ToList();

            if (role == RoleConstants.Admin)
            {
                ViewBag.TotalSuppliers = suppliers.Count;
                ViewBag.ActiveSuppliers = suppliers.Count(s => s.IsActive == true);
                ViewBag.InactiveSuppliers = suppliers.Count(s => s.IsActive == false);

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