using CarRentalSystem.Data;
using CarRentalSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using CarRentalSystem.Constants;
using CarRentalSystem.Helpers;

namespace CarRentalSystem.Controllers
{
    public class ImportReceiptController : Controller
    {
        private readonly CarRentalContext _context;

        public ImportReceiptController(CarRentalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search, string status)
        {
            // 1. Kiểm tra phân quyền giống hệt code cũ của bạn
            var role = HttpContext.Session.GetString("RoleName");
            if (role != RoleConstants.Admin && role != RoleConstants.Staff)
            {
                return RedirectToAction("Login", "Account");
            }

            // 2. Khởi tạo câu truy vấn
            var query = _context.ImportReceipts
                .Include(r => r.Supplier)
                .AsQueryable();

            // 3. Xử lý logic Tìm kiếm
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(i => i.SoImportReceipt.Contains(search)
                                      || i.Supplier.SupplierName.Contains(search));
                ViewBag.SearchString = search;
            }

            // 4. Xử lý logic Lọc theo Trạng thái
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(i => i.Status == status);
                ViewBag.CurrentStatus = status;
            }

            // 5. Sắp xếp phiếu mới nhất lên đầu (như code cũ)
            var receipts = await query.OrderByDescending(r => r.ImportReceiptId).ToListAsync();

            // 6. Trả về View tương ứng theo Role
            if (role == RoleConstants.Admin)
            {
                return View("AdminIndex", receipts);
            }

            return View(receipts); // Dành cho Staff
        }

        [HttpGet]
        public IActionResult Create()
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != RoleConstants.Staff)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Categories = _context.VehicleCategories.ToList();
            ViewBag.Suppliers = _context.Suppliers.ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection form)
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != RoleConstants.Staff)
            {
                return RedirectToAction("Login", "Account");
            }

            var accountIdStr = HttpContext.Session.GetString("AccountId");
            if (string.IsNullOrEmpty(accountIdStr))
            {
                return RedirectToAction("Login", "Account");
            }

            int accountId = int.Parse(accountIdStr);

            // Đã thêm Include để tránh lỗi Null Reference
            var staff = _context.Staff
                .Include(s => s.UserProfile)
                .FirstOrDefault(s => s.UserProfile.AccountId == accountId);

            if (staff == null)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                // 1. Kiểm tra các trường bắt buộc
                if (string.IsNullOrEmpty(form["CategoryId"]) ||
                    string.IsNullOrEmpty(form["VehicleName"]) ||
                    string.IsNullOrEmpty(form["LicensePlate"]) ||
                    string.IsNullOrEmpty(form["SupplierId"]) ||
                    string.IsNullOrEmpty(form["Brand"]) ||
                    string.IsNullOrEmpty(form["Model"]))
                {
                    TempData["ErrorMessage"] = "Vui lòng điền đầy đủ Tên xe, Hãng xe, Dòng xe, Biển số, Loại xe và Nhà cung cấp.";
                    ViewBag.Categories = _context.VehicleCategories.ToList();
                    ViewBag.Suppliers = _context.Suppliers.ToList();
                    return View();
                }

                // 2. Kiểm tra biển số trùng lặp (tránh lỗi UNIQUE trong DB)
                string licensePlate = form["LicensePlate"].ToString();
                var existingVehicle = _context.Vehicles.FirstOrDefault(v => v.LicensePlate == licensePlate);
                if (existingVehicle != null)
                {
                    TempData["ErrorMessage"] = $"Biển số xe '{licensePlate}' đã tồn tại trong hệ thống. Vui lòng kiểm tra lại.";
                    ViewBag.Categories = _context.VehicleCategories.ToList();
                    ViewBag.Suppliers = _context.Suppliers.ToList();
                    return View();
                }

                // 3. Tạo mới Vehicle
                var vehicle = new Vehicle
                {
                    CategoryId = int.Parse(form["CategoryId"]),
                    VehicleName = form["VehicleName"].ToString(),
                    LicensePlate = licensePlate,
                    Brand = form["Brand"].ToString(),
                    Model = form["Model"].ToString(),
                    ManufactureYear = string.IsNullOrEmpty(form["ManufactureYear"]) ? DateTime.Now.Year : int.Parse(form["ManufactureYear"]),
                    Color = form["Color"].ToString() ?? "Không xác định",
                    Seats = string.IsNullOrEmpty(form["Seats"]) ? 4 : int.Parse(form["Seats"]),
                    Transmission = form["Transmission"].ToString() ?? "Tự động",
                    FuelType = form["FuelType"].ToString() ?? "Xăng",
                    PricePerDay = string.IsNullOrEmpty(form["TotalAmount"]) ? 0 : decimal.Parse(form["TotalAmount"]),
                    Status = VehicleStatus.Inactive, // Xe mới nhập, chờ duyệt
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _context.Vehicles.Add(vehicle);
                _context.SaveChanges();

                // 4. Tạo Import Receipt (Phiếu nhập)
                var totalAmount = string.IsNullOrEmpty(form["TotalAmount"]) ? 0 : decimal.Parse(form["TotalAmount"]);

                var receipt = new ImportReceipt
                {
                    SoImportReceipt = CodeGeneratorHelper.GenerateReceiptCode(),
                    SupplierId = int.Parse(form["SupplierId"]),
                    StaffId = staff.StaffId,
                    ImportDate = DateTime.Now,
                    TotalAmount = totalAmount,
                    Status = ImportReceiptStatus.Pending,
                    CreatedAt = DateTime.Now
                };

                _context.ImportReceipts.Add(receipt);
                _context.SaveChanges();

                // 5. Tạo Import Receipt Detail (Chi tiết nhập)
                var detail = new ImportReceiptDetail
                {
                    ImportReceiptId = receipt.ImportReceiptId,
                    VehicleId = vehicle.VehicleId,
                    Quantity = 1,                           // BỔ SUNG: Nhập 1 chiếc xe
                    UnitPrice = totalAmount,
                    LineTotal = totalAmount,                // BỔ SUNG: Tổng tiền dòng này
                    CurrentKm = 0,                          // BỔ SUNG: Xe mới nhập Km = 0
                    VehicleCondition = "Bình thường"        // BỔ SUNG: Tình trạng xe
                };

                _context.ImportReceiptDetails.Add(detail);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Tạo phiếu nhập kho thành công! Chờ Admin phê duyệt.";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException dbEx)
            {
                // Bắt lỗi sâu từ Database (Ví dụ: lỗi khóa ngoại, lỗi null)
                string dbErrorMsg = dbEx.InnerException?.Message ?? dbEx.Message;
                TempData["ErrorMessage"] = $"Lỗi cơ sở dữ liệu: {dbErrorMsg}";

                ViewBag.Categories = _context.VehicleCategories.ToList();
                ViewBag.Suppliers = _context.Suppliers.ToList();
                return View();
            }
            catch (Exception ex)
            {
                // Bắt các lỗi chung khác
                TempData["ErrorMessage"] = $"Lỗi hệ thống: {ex.Message}";

                ViewBag.Categories = _context.VehicleCategories.ToList();
                ViewBag.Suppliers = _context.Suppliers.ToList();
                return View();
            }
        }

        [HttpPost]
        public IActionResult Approve(int id)
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != RoleConstants.Admin)
            {
                return RedirectToAction("Login", "Account");
            }

            var receipt = _context.ImportReceipts.Find(id);
            if (receipt != null)
            {
                receipt.Status = ImportReceiptStatus.Approved;
                receipt.ApprovalDate = DateTime.Now;

                var details = _context.ImportReceiptDetails.Where(d => d.ImportReceiptId == id).ToList();
                foreach (var detail in details)
                {
                    var vehicle = _context.Vehicles.Find(detail.VehicleId);
                    if (vehicle != null)
                    {
                        vehicle.Status = VehicleStatus.Available;
                    }
                }

                _context.SaveChanges();
                TempData["SuccessMessage"] = "Đã phê duyệt thành công! Xe đã được đưa vào kho sẵn sàng.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Reject(int id)
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != RoleConstants.Admin)
            {
                return RedirectToAction("Login", "Account");
            }

            var receipt = _context.ImportReceipts.Find(id);
            if (receipt != null)
            {
                receipt.Status = ImportReceiptStatus.Rejected;
                receipt.ApprovalDate = DateTime.Now;

                var details = _context.ImportReceiptDetails.Where(d => d.ImportReceiptId == id).ToList();
                foreach (var detail in details)
                {
                    var vehicle = _context.Vehicles.Find(detail.VehicleId);
                    if (vehicle != null)
                    {
                        vehicle.Status = VehicleStatus.Inactive;
                    }
                }

                _context.SaveChanges();
                TempData["ErrorMessage"] = "Đã từ chối phiếu nhập!";
            }
            return RedirectToAction("Index");
        }
    }
}