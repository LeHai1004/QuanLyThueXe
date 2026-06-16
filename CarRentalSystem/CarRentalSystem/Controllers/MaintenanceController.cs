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
    public class MaintenanceController : Controller
    {
        private readonly CarRentalContext _context;

        public MaintenanceController(CarRentalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(int? vehicleId)
        {
            var role = HttpContext.Session.GetRoleName();
            if (role != RoleEnums.Staff && role != RoleEnums.Admin)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Vehicles = _context.Vehicles
                                       .Where(v => v.Status != VehicleStatus.Inactive)
                                       .ToList();
                                       
            ViewBag.Staffs = _context.Staff
                                     .Include(s => s.UserProfile)
                                     .Where(s => s.IsActive)
                                     .ToList();
                                     
            ViewBag.SelectedVehicleId = vehicleId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(IFormCollection form)
        {
            var role = HttpContext.Session.GetRoleName();
            if (role != RoleEnums.Staff && role != RoleEnums.Admin)
            {
                return RedirectToAction("Login", "Account");
            }

            int vehicleId = int.Parse(form["VehicleId"]);
            int staffId;
            
            if (role == RoleEnums.Admin)
            {
                staffId = int.Parse(form["StaffId"]);
            }
            else
            {
                var accountId = HttpContext.Session.GetAccountId()!.Value;
                var staff = _context.Staff.FirstOrDefault(s => s.UserProfile.AccountId == accountId);
                staffId = staff.StaffId;
            }

            var status = role == RoleEnums.Admin ? MaintenanceStatus.Pending : MaintenanceStatus.Requested;

            var maintenanceLog = new MaintenanceLog
            {
                VehicleId = vehicleId,
                StaffId = staffId,
                MaintenanceType = form["MaintenanceType"].ToString(),
                Cost = decimal.Parse(form["Cost"]),
                MaintenanceDate = DateOnly.Parse(form["MaintenanceDate"]),
                Description = form["Description"].ToString(),
                Status = status
            };

            if (!string.IsNullOrEmpty(form["MaintenanceDateTiep"]))
            {
                maintenanceLog.MaintenanceDateTiep = DateOnly.Parse(form["MaintenanceDateTiep"]);
            }

            _context.MaintenanceLogs.Add(maintenanceLog);
            
            if (role == RoleEnums.Admin)
            {
                var vehicle = _context.Vehicles.Find(vehicleId);
                if (vehicle != null)
                {
                    vehicle.Status = VehicleStatus.Maintenance;
                }
            }

            _context.SaveChanges();

            if (role == RoleEnums.Admin)
                TempData["SuccessMessage"] = "Phân công bảo dưỡng và cập nhật trạng thái xe thành công!";
            else
                TempData["SuccessMessage"] = "Gửi yêu cầu bảo dưỡng thành công! Vui lòng chờ quản lý duyệt.";

            return RedirectToAction("Index", "Vehicle");
        }

        [HttpGet]
        public IActionResult MyTasks()
        {
            var role = HttpContext.Session.GetRoleName();
            if (role != RoleEnums.Staff)
            {
                return RedirectToAction("Login", "Account");
            }

            var accountId = HttpContext.Session.GetAccountId();
            var staff = _context.Staff.FirstOrDefault(s => s.UserProfile.AccountId == accountId);

            if (staff == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var tasks = _context.MaintenanceLogs
                                .Include(m => m.Vehicle)
                                .Where(m => m.StaffId == staff.StaffId)
                                .OrderBy(m => m.Status == MaintenanceStatus.Completed ? 1 : 0)
                                .ThenByDescending(m => m.MaintenanceDate)
                                .ToList();

            return View(tasks);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CompleteTask(int id)
        {
            var role = HttpContext.Session.GetRoleName();
            if (role != RoleEnums.Staff)
            {
                return RedirectToAction("Login", "Account");
            }

            var maintenanceLog = _context.MaintenanceLogs
                                         .Include(m => m.Vehicle)
                                         .FirstOrDefault(m => m.MaintenanceId == id);

            if (maintenanceLog != null && maintenanceLog.Status != MaintenanceStatus.Completed)
            {
                maintenanceLog.Status = MaintenanceStatus.Completed;
                
                if (maintenanceLog.Vehicle != null)
                {
                    maintenanceLog.Vehicle.Status = VehicleStatus.Available;
                }

                _context.SaveChanges();
                TempData["SuccessMessage"] = "Đã cập nhật trạng thái hoàn thành phiếu bảo dưỡng và đưa xe trở lại sẵn sàng!";
            }

            return RedirectToAction("MyTasks");
        }

        [HttpGet]
        public IActionResult Requests()
        {
            var role = HttpContext.Session.GetRoleName();
            if (role != RoleEnums.Admin)
            {
                return RedirectToAction("Login", "Account");
            }

            var requests = _context.MaintenanceLogs
                                   .Include(m => m.Vehicle)
                                   .Include(m => m.Staff)
                                   .ThenInclude(s => s.UserProfile)
                                   .Where(m => m.Status == MaintenanceStatus.Requested)
                                   .OrderByDescending(m => m.MaintenanceDate)
                                   .ToList();

            return View(requests);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ApproveRequest(int id)
        {
            var role = HttpContext.Session.GetRoleName();
            if (role != RoleEnums.Admin)
            {
                return RedirectToAction("Login", "Account");
            }

            var log = _context.MaintenanceLogs.Include(m => m.Vehicle).FirstOrDefault(m => m.MaintenanceId == id);
            if (log != null && log.Status == MaintenanceStatus.Requested)
            {
                log.Status = MaintenanceStatus.Pending;
                if (log.Vehicle != null)
                {
                    log.Vehicle.Status = VehicleStatus.Maintenance;
                }
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Đã duyệt yêu cầu và giao việc cho nhân viên!";
            }

            return RedirectToAction("Requests");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RejectRequest(int id, string reason)
        {
            var role = HttpContext.Session.GetRoleName();
            if (role != RoleEnums.Admin)
            {
                return RedirectToAction("Login", "Account");
            }

            var log = _context.MaintenanceLogs.Find(id);
            if (log != null && log.Status == MaintenanceStatus.Requested)
            {
                log.Status = MaintenanceStatus.Rejected;
                log.Description = log.Description + "\n\n[Lý do từ chối]: " + reason;
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Đã từ chối yêu cầu bảo dưỡng.";
            }

            return RedirectToAction("Requests");
        }
    }
}