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

        public IActionResult Index(string search)
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != RoleConstants.Admin && role != RoleConstants.Staff)
            {
                return RedirectToAction("Login", "Account");
            }

            var query = _context.Suppliers.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.SupplierName.Contains(search) 
                                      || (s.TaxCode != null && s.TaxCode.Contains(search))
                                      || (s.PhoneNumber != null && s.PhoneNumber.Contains(search))
                                      || ("NCC-" + s.SupplierId.ToString("D3")).Contains(search));
            }

            var suppliers = query.OrderByDescending(s => s.SupplierId).ToList();
            
            ViewBag.SearchString = search;

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

        [HttpGet]
        public IActionResult Create()
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != RoleConstants.Admin && role != RoleConstants.Staff)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string supplierName, string taxCode, string phoneNumber, string email, string contactPerson, string address)
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != RoleConstants.Admin && role != RoleConstants.Staff)
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrEmpty(supplierName))
            {
                ViewBag.Error = "Tên nhà cung cấp không được để trống.";
                return View();
            }

            if (await _context.Suppliers.AnyAsync(s => s.TaxCode == taxCode && !string.IsNullOrEmpty(taxCode)))
            {
                ViewBag.Error = "Mã số thuế này đã tồn tại trong hệ thống.";
                return View();
            }

            var newSupplier = new Supplier
            {
                SupplierName = supplierName,
                TaxCode = taxCode,
                PhoneNumber = phoneNumber,
                Email = email,
                ContactPerson = contactPerson,
                Address = address,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            _context.Suppliers.Add(newSupplier);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Thêm nhà cung cấp thành công!";
            return RedirectToAction("Index");
        }
    }
}