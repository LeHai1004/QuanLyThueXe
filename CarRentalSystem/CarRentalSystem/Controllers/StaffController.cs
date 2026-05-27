using CarRentalSystem.Data;
using CarRentalSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CarRentalSystem.Constants;

namespace CarRentalSystem.Controllers
{
    public class StaffController : Controller
    {
        private readonly CarRentalContext _context;

        public StaffController(CarRentalContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != RoleConstants.Admin)
            {
                return RedirectToAction("Login", "Account");
            }

            var staffs = _context.Staff
                .Include(s => s.UserProfile)
                    .ThenInclude(u => u.Account)
                .OrderByDescending(s => s.StaffId)
                .ToList();

            ViewBag.TotalRevenue = _context.Invoices
                .Where(i => i.Status == InvoiceStatus.Paid)
                .Sum(i => (decimal?)i.GrandTotal) ?? 0;

            return View("AdminIndex", staffs);
        }
    }
}