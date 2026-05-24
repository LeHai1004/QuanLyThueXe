using CarRentalSystem.Data;
using CarRentalSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CarRentalSystem.Controllers
{
    public class ImportReceiptController : Controller
    {
        private readonly CarRentalContext _context;

        public ImportReceiptController(CarRentalContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != "Admin" && role != "Staff") return RedirectToAction("Login", "Account");

            var receipts = _context.ImportReceipts
                .Include(r => r.Supplier)
                .OrderByDescending(r => r.ImportReceiptId)
                .ToList();

            if (role == "Admin")
            {
                return View("AdminIndex", receipts);
            }

            return View(receipts);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != "Staff") return RedirectToAction("Login", "Account");

            ViewBag.Categories = _context.VehicleCategories.ToList();
            ViewBag.Suppliers = _context.Suppliers.ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection form)
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != "Staff") return RedirectToAction("Login", "Account");

            var accountIdStr = HttpContext.Session.GetString("AccountId");
            int accountId = int.Parse(accountIdStr);

            var staff = _context.Staff.FirstOrDefault(s => s.UserProfile.AccountId == accountId);
            if (staff == null) return RedirectToAction("Login", "Account");

            var vehicle = new Vehicle
            {
                VehicleName = form["VehicleName"].ToString(),
                LicensePlate = form["LicensePlate"].ToString(),
                CategoryId = int.Parse(form["CategoryId"]),
                Status = "Cho duyet",
                PricePerDay = 500000,
                ManufactureYear = DateTime.Now.Year,
                Brand = "Chưa xác định",
                Model = "Chưa xác định",
                Seats = 4,
                AverageRating = 0,
                HinhAnh = "https://images.unsplash.com/photo-1550355291-bbee04a92027",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();

            var receipt = new ImportReceipt
            {
                SoImportReceipt = "PN" + DateTime.Now.ToString("ddHHmmss"),
                SupplierId = int.Parse(form["SupplierId"]),
                StaffId = staff.StaffId,
                ImportDate = DateTime.Now,
                TotalAmount = decimal.Parse(form["TotalAmount"]),
                Status = "Cho duyet",
                CreatedAt = DateTime.Now
            };
            _context.ImportReceipts.Add(receipt);
            _context.SaveChanges();

            var receiptDetail = new ImportReceiptDetail
            {
                ImportReceiptId = receipt.ImportReceiptId,
                VehicleId = vehicle.VehicleId,
                Quantity = 1,
                UnitPrice = decimal.Parse(form["TotalAmount"]),
                LineTotal = decimal.Parse(form["TotalAmount"]),
                CurrentKm = 0
            };
            _context.ImportReceiptDetails.Add(receiptDetail);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Gửi phiếu nhập thành công! Đang chờ Admin duyệt.";

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Approve(int id)
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != "Admin") return RedirectToAction("Login", "Account");

            var receipt = _context.ImportReceipts.Find(id);
            if (receipt != null)
            {
                // Cập nhật trạng thái phiếu nhập thành Đã duyệt
                receipt.Status = "Da duyet";

                // Tìm xe tương ứng trong chi tiết phiếu nhập và mở khóa cho thuê
                var details = _context.ImportReceiptDetails.Where(d => d.ImportReceiptId == id).ToList();
                foreach (var detail in details)
                {
                    var vehicle = _context.Vehicles.Find(detail.VehicleId);
                    if (vehicle != null)
                    {
                        vehicle.Status = "San sang"; // Đổi trạng thái xe thành Sẵn sàng cho khách thuê
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
            if (role != "Admin") return RedirectToAction("Login", "Account");

            var receipt = _context.ImportReceipts.Find(id);
            if (receipt != null)
            {
                receipt.Status = "Tu choi";

                // Đổi trạng thái xe thành "Huy bo" vì bị từ chối nhập kho
                var details = _context.ImportReceiptDetails.Where(d => d.ImportReceiptId == id).ToList();
                foreach (var detail in details)
                {
                    var vehicle = _context.Vehicles.Find(detail.VehicleId);
                    if (vehicle != null)
                    {
                        vehicle.Status = "Huy bo";
                    }
                }

                _context.SaveChanges();
                TempData["ErrorMessage"] = "Đã từ chối phiếu nhập!";
            }
            return RedirectToAction("Index");
        }
    }
}