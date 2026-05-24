using CarRentalSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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
                .Where(v => v.Status == "San sang" || v.Status == "Sẵn sàng")
                .OrderByDescending(v => v.AverageRating)
                .Take(3)
                .ToListAsync();

            return View(featuredVehicles);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}