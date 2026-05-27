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
    public class MaintenanceController : Controller
    {
        private readonly CarRentalContext _context;

        public MaintenanceController(CarRentalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != RoleConstants.Staff)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Vehicles = _context.Vehicles
                                       .Where(v => v.Status != VehicleStatus.Inactive)
                                       .ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(IFormCollection form)
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != RoleConstants.Staff)
            {
                return RedirectToAction("Login", "Account");
            }

            var accountIdStr = HttpContext.Session.GetString("AccountId");
            int accountId = int.Parse(accountIdStr);
            var staff = _context.Staff.FirstOrDefault(s => s.UserProfile.AccountId == accountId);

            if (staff == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var maintenanceLog = new MaintenanceLog
            {
                VehicleId = int.Parse(form["VehicleId"]),
                StaffId = staff.StaffId,
                MaintenanceType = form["MaintenanceType"].ToString(),
                Cost = decimal.Parse(form["Cost"]),
                MaintenanceDate = DateOnly.Parse(form["MaintenanceDate"]),
                Description = form["Description"].ToString(),
                Status = MaintenanceStatus.Completed
            };

            if (!string.IsNullOrEmpty(form["MaintenanceDateTiep"]))
            {
                maintenanceLog.MaintenanceDateTiep = DateOnly.Parse(form["MaintenanceDateTiep"]);
            }

            _context.MaintenanceLogs.Add(maintenanceLog);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Ghi nhận lịch sử bảo dưỡng thành công!";

            return RedirectToAction("Index", "Dashboard");
        }
    }
}