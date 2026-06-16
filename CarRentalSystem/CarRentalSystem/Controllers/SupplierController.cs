using CarRentalSystem.Data;
using CarRentalSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using CarRentalSystem.Enums;
using CarRentalSystem.Extensions;

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
            var role = HttpContext.Session.GetRoleName();
            if (role != RoleEnums.Admin && role != RoleEnums.Staff)
            {
                return RedirectToAction("Login", "Account");
            }

            var query = _context.Suppliers.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                string searchNum = search.Replace("NCC-", "").Replace("ncc-", "");
                bool isIdSearch = int.TryParse(searchNum, out int parsedId);

                query = query.Where(s => s.SupplierName.Contains(search) 
                                      || (s.TaxCode != null && s.TaxCode.Contains(search))
                                      || (s.PhoneNumber != null && s.PhoneNumber.Contains(search))
                                      || (isIdSearch && s.SupplierId == parsedId));
            }

            var suppliers = query.OrderByDescending(s => s.SupplierId).ToList();
            
            ViewBag.SearchString = search;

            if (role == RoleEnums.Admin)
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
            var role = HttpContext.Session.GetRoleName();
            if (role != RoleEnums.Admin && role != RoleEnums.Staff)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string supplierName, string taxCode, string phoneNumber, string email, string contactPerson, string address)
        {
            var role = HttpContext.Session.GetRoleName();
            if (role != RoleEnums.Admin && role != RoleEnums.Staff)
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
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var role = HttpContext.Session.GetRoleName();
            if (role != RoleEnums.Admin && role != RoleEnums.Staff) return RedirectToAction("Login", "Account");

            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null) return NotFound();

            return View(supplier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Supplier supplier)
        {
            var role = HttpContext.Session.GetRoleName();
            if (role != RoleEnums.Admin && role != RoleEnums.Staff) return RedirectToAction("Login", "Account");

            if (id != supplier.SupplierId) return NotFound();

            if (string.IsNullOrEmpty(supplier.SupplierName))
            {
                ViewBag.Error = "Tên nhà cung cấp không được để trống.";
                return View(supplier);
            }

            if (await _context.Suppliers.AnyAsync(s => s.TaxCode == supplier.TaxCode && s.SupplierId != id && !string.IsNullOrEmpty(supplier.TaxCode)))
            {
                ViewBag.Error = "Mã số thuế này đã thuộc về nhà cung cấp khác.";
                return View(supplier);
            }

            try
            {
                var sToUpdate = await _context.Suppliers.FindAsync(id);
                if (sToUpdate == null) return NotFound();

                sToUpdate.SupplierName = supplier.SupplierName;
                sToUpdate.TaxCode = supplier.TaxCode;
                sToUpdate.PhoneNumber = supplier.PhoneNumber;
                sToUpdate.Email = supplier.Email;
                sToUpdate.ContactPerson = supplier.ContactPerson;
                sToUpdate.Address = supplier.Address;
                sToUpdate.IsActive = supplier.IsActive;

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật nhà cung cấp thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Lỗi: {ex.Message}";
                return View(supplier);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var role = HttpContext.Session.GetRoleName();
            if (role != RoleEnums.Admin && role != RoleEnums.Staff) return RedirectToAction("Login", "Account");

            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null) return NotFound();

            // Soft delete because Supplier might be referenced in ImportReceipts
            supplier.IsActive = false;
            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = "Đã tạm dừng hợp tác với nhà cung cấp này.";
            return RedirectToAction("Index");
        }
    }
}