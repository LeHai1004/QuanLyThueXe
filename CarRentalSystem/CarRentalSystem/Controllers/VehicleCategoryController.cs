using CarRentalSystem.Data;
using CarRentalSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Controllers
{
    public class VehicleCategoryController : Controller
    {
        private readonly CarRentalContext _db;

        public VehicleCategoryController(CarRentalContext db)
        {
            _db = db;
        }

        // GET: /VehicleCategory (Đã thêm Phân trang & Tìm kiếm)
        public async Task<IActionResult> Index(string search, int page = 1)
        {
            // Bỏ comment nếu đã có Đăng nhập
            // if (HttpContext.Session.GetString("RoleName") == null) return RedirectToAction("Login", "Account");

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

        // GET: /VehicleCategory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /VehicleCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleCategory model)
        {
            if (ModelState.IsValid)
            {
                _db.VehicleCategories.Add(model);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Thêm loại xe mới thành công!";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: /VehicleCategory/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.VehicleCategories.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        // POST: /VehicleCategory/Edit/5
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

        // 1. HÀM GET: Chuyển sang giao diện Danger Card để xác nhận Xóa
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var item = await _db.VehicleCategories
                .Include(c => c.Vehicles)
                .FirstOrDefaultAsync(m => m.CategoryId == id);

            if (item == null) return NotFound();

            return View(item);
        }

        // 2. HÀM POST: Thực thi xóa khi bấm nút Xác nhận trên Danger Card
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