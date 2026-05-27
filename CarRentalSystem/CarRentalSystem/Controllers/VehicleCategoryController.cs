using CarRentalSystem.Data;
using CarRentalSystem.Models;
using CarRentalSystem.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalSystem.Controllers
{
    public class VehicleCategoryController : Controller
    {
        private readonly CarRentalContext _db;

        public VehicleCategoryController(CarRentalContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(string search, int page = 1)
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != RoleConstants.Admin && role != RoleConstants.Staff)
            {
                return RedirectToAction("Login", "Account");
            }

            int pageSize = 10;
            var query = _db.VehicleCategories.Include(c => c.Vehicles).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.CategoryName.Contains(search) || c.Description.Contains(search));
            }

            int totalCount = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var list = await query
                .OrderByDescending(c => c.CategoryId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchString = search;

            return View(list);
        }

        public IActionResult Create()
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != RoleConstants.Admin) return RedirectToAction("Login", "Account");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleCategory model)
        {
            if (ModelState.IsValid)
            {
                _db.VehicleCategories.Add(model);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Đã thêm loại xe mới thành công!";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != RoleConstants.Admin) return RedirectToAction("Login", "Account");

            if (id == null) return NotFound();

            var item = await _db.VehicleCategories.FindAsync(id);
            if (item == null) return NotFound();

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VehicleCategory model)
        {
            if (ModelState.IsValid)
            {
                _db.VehicleCategories.Update(model);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Cập nhật loại xe thành công!";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != RoleConstants.Admin) return RedirectToAction("Login", "Account");

            if (id == null) return NotFound();

            var item = await _db.VehicleCategories
                .Include(c => c.Vehicles)
                .FirstOrDefaultAsync(m => m.CategoryId == id);

            if (item == null) return NotFound();

            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _db.VehicleCategories.FindAsync(id);
            if (item != null)
            {
                _db.VehicleCategories.Remove(item);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Đã xóa danh mục xe thành công!";
            }
            return RedirectToAction("Index");
        }
    }
}