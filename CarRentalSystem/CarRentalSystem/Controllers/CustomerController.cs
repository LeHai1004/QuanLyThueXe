using CarRentalSystem.Data;
using CarRentalSystem.Models;
using CarRentalSystem.Enums;
using CarRentalSystem.Extensions;
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
            var role = HttpContext.Session.GetRoleName();
            if (role != RoleEnums.Admin && role != RoleEnums.Staff) return RedirectToAction("Login", "Account");

            var query = _context.Customers
                .Include(c => c.UserProfile)
                    .ThenInclude(u => u.Account)
                .Include(c => c.Bookings)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status) && status != AccountStatusLabel.All)
            {
                if (status == AccountStatusLabel.Active)
                    query = query.Where(c => c.UserProfile.Account.IsActive == true);
                else if (status == AccountStatusLabel.Locked)
                    query = query.Where(c => c.UserProfile.Account.IsActive == false);
            }

            var customers = query.OrderByDescending(c => c.CustomerId).ToList();

            if (!string.IsNullOrEmpty(tier) && tier != CustomerTierLabel.All)
            {
                customers = customers.Where(item => 
                {
                    decimal totalSpent = item.Bookings?.Where(b => b.Status == BookingStatus.Completed).Sum(b => (decimal?)b.TotalAmount) ?? 0;
                    if (tier == CustomerTierLabel.Gold) return totalSpent >= CustomerTierThreshold.Gold;
                    if (tier == CustomerTierLabel.Silver) return totalSpent >= CustomerTierThreshold.Silver && totalSpent < CustomerTierThreshold.Gold;
                    if (tier == CustomerTierLabel.Bronze) return totalSpent >= CustomerTierThreshold.Bronze && totalSpent < CustomerTierThreshold.Silver;
                    if (tier == CustomerTierLabel.Member) return totalSpent < CustomerTierThreshold.Bronze;
                    return true;
                }).ToList();
            }

            ViewBag.CurrentTier = tier;
            ViewBag.CurrentStatus = status;

            if (role == RoleEnums.Admin)
            {
                return View("AdminIndex", customers);
            }

            return View("StaffIndex", customers);
        }

        public IActionResult Details(int id)
        {
            var role = HttpContext.Session.GetRoleName();
            if (role != RoleEnums.Admin && role != RoleEnums.Staff) return RedirectToAction("Login", "Account");

            var customer = _context.Customers
                .Include(c => c.UserProfile)
                    .ThenInclude(u => u.Account)
                .Include(c => c.Bookings)
                    .ThenInclude(b => b.Vehicle)
                .FirstOrDefault(c => c.CustomerId == id);

            if (customer == null) return NotFound();

            return View("Details", customer);
        }
    }
}