using CarRentalSystem.Data;
using CarRentalSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CarRentalSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly CarRentalContext _context;

        public DashboardController(CarRentalContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != "Admin" && role != "Staff")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.TotalOrders = _context.Bookings.Count();
            ViewBag.RentedCars = _context.Bookings.Count(b => b.Status == "Dang thue");
            ViewBag.AvailableCars = _context.Vehicles.Count(v => v.Status == "San sang");
            ViewBag.Revenue = _context.Bookings.Where(b => b.Status == "Hoan thanh").Sum(b => (decimal?)b.TotalAmount) ?? 0;

            var sevenDaysAgo = DateTime.Now.AddDays(-7);
            ViewBag.NewCustomers = _context.Customers.Count(c => c.UserProfile.CreatedAt >= sevenDaysAgo);

            var recentOrders = _context.Bookings
                .Include(b => b.Customer)
                    .ThenInclude(c => c.UserProfile)
                .Include(b => b.Vehicle)
                .OrderByDescending(b => b.CreatedAt)
                .Take(5)
                .ToList();

            if (role == "Admin")
            {
                return View("AdminIndex", recentOrders);
            }

            return View(recentOrders);
        }

        public IActionResult Report()
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != "Admin") return RedirectToAction("Login", "Account");

            var totalRevenue = _context.Invoices
                .Where(i => i.Status == "Da thanh toan" || i.Status == "Đã thanh toán")
                .Sum(i => (decimal?)i.GrandTotal) ?? 0;

            var netProfit = totalRevenue * 0.35m;

            var totalBookings = _context.Bookings.Count();

            var totalVehicles = _context.Vehicles.Count();
            var rentedVehicles = _context.Vehicles.Count(v => v.Status == "Dang thue" || v.Status == "Đang thuê");
            var occupancyRate = totalVehicles > 0 ? (double)rentedVehicles / totalVehicles * 100 : 0;

            var currentMonth = DateTime.Now.Month;

            var branchStats = _context.Bookings
                .Where(b => b.Status == "Hoan thanh" || b.Status == "Đã hoàn thành")
                .ToList()
                .GroupBy(b => b.PickupLocation)
                .Select(g => {
                    var thisMonthRev = g.Where(x => x.ReturnDateTime.Month == currentMonth).Sum(x => x.TotalAmount);
                    var lastMonthRev = g.Where(x => x.ReturnDateTime.Month == (currentMonth == 1 ? 12 : currentMonth - 1)).Sum(x => x.TotalAmount);
                    double growth = 0;

                    if (lastMonthRev > 0)
                    {
                        growth = (double)((thisMonthRev - lastMonthRev) / lastMonthRev * 100);
                    }
                    else if (thisMonthRev > 0)
                    {
                        growth = 100;
                    }

                    return new
                    {
                        BranchName = g.Key.Contains(",") ? "Chi nhánh " + g.Key.Split(',')[0].Trim() : g.Key,
                        Address = g.Key,
                        BookingCount = g.Count(),
                        Revenue = g.Sum(b => b.TotalAmount),
                        Growth = Math.Round(growth, 1)
                    };
                })
                .OrderByDescending(x => x.Revenue)
                .Take(5)
                .ToList();

            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.NetProfit = netProfit;
            ViewBag.TotalBookings = totalBookings;
            ViewBag.OccupancyRate = occupancyRate;
            ViewBag.BranchStats = branchStats;

            return View();
        }
    }
}