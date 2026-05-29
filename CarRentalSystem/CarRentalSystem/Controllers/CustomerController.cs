using CarRentalSystem.Data;
using CarRentalSystem.Models;
using CarRentalSystem.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CarRentalSystem.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CarRentalContext _context;

        public CustomerController(CarRentalContext context)
        {
            _context = context;
        }

        public IActionResult Index(string tier, string status)
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != RoleConstants.Admin && role != RoleConstants.Staff) return RedirectToAction("Login", "Account");

            var query = _context.Customers
                .Include(c => c.UserProfile)
                    .ThenInclude(u => u.Account)
                .Include(c => c.Bookings)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status) && status != "Tất cả")
            {
                if (status == "Hoạt động")
                    query = query.Where(c => c.UserProfile.Account.IsActive == true);
                else if (status == "Bị khóa")
                    query = query.Where(c => c.UserProfile.Account.IsActive == false);
            }

            var customers = query.OrderByDescending(c => c.CustomerId).ToList();

            if (!string.IsNullOrEmpty(tier) && tier != "Tất cả")
            {
                customers = customers.Where(item => 
                {
                    decimal totalSpent = item.Bookings?.Where(b => b.Status == BookingStatus.Completed).Sum(b => (decimal?)b.TotalAmount) ?? 0;
                    if (tier == "VIP Gold") return totalSpent >= 50000000;
                    if (tier == "VIP Silver") return totalSpent >= 20000000 && totalSpent < 50000000;
                    if (tier == "VIP Bronze") return totalSpent >= 5000000 && totalSpent < 20000000;
                    if (tier == "Member") return totalSpent < 5000000;
                    return true;
                }).ToList();
            }

            ViewBag.CurrentTier = tier;
            ViewBag.CurrentStatus = status;

            if (role == RoleConstants.Admin)
            {
                return View("AdminIndex", customers);
            }

            return View("StaffIndex", customers);
        }
    }
}