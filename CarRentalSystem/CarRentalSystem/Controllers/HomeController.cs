using CarRentalSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using CarRentalSystem.Enums;
using CarRentalSystem.Extensions;

namespace CarRentalSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly CarRentalContext _db;

        public HomeController(CarRentalContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var featuredVehicles = await _db.Vehicles
                .Include(v => v.Category)
                .Where(v => v.Status == VehicleStatus.Available)
                .OrderByDescending(v => v.AverageRating)
                .Take(3)
                .ToListAsync();

            return View(featuredVehicles);
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult VeChungToi()
        {
            return View();
        }

        public IActionResult DichVu()
        {
            return View();
        }

        public IActionResult LienHe()
        {
            return View();
        }
    }
}