using CarRentalSystem.Data;
using CarRentalSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly CarRentalContext _db;

        public AccountController(CarRentalContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("AccountId") != null)
            {
                var role = HttpContext.Session.GetString("RoleName");
                if (role == "Admin" || role == "Staff")
                    return RedirectToAction("Index", "Dashboard");
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.LoginError = "Vui lòng nhập đầy đủ thông tin.";
                return View();
            }

            var account = await _db.Accounts
                .Include(a => a.Role)
                .Include(a => a.UserProfile)
                .FirstOrDefaultAsync(a => a.Email == email);

            if (account == null || account.PasswordHash != password)
            {
                ViewBag.LoginError = "Email hoặc mật khẩu không đúng.";
                return View();
            }

            if (!account.IsActive)
            {
                ViewBag.LoginError = "Tài khoản đã bị khóa.";
                return View();
            }

            var fullName = account.UserProfile?.FullName ?? account.Email;

            HttpContext.Session.SetString("AccountId", account.AccountId.ToString());
            HttpContext.Session.SetString("Email", account.Email);
            HttpContext.Session.SetString("RoleName", account.Role?.TenRole ?? "Customer");
            HttpContext.Session.SetString("FullName", fullName);

            if (account.Role?.TenRole == "Admin" || account.Role?.TenRole == "Staff")
                return RedirectToAction("Index", "Dashboard");

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string fullName, string email, string phoneNumber, string nationalId, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ViewBag.RegisterError = "Mật khẩu xác nhận không khớp.";
                return View();
            }

            if (await _db.Accounts.AnyAsync(a => a.Email == email))
            {
                ViewBag.RegisterError = "Email này đã được sử dụng.";
                return View();
            }

            var newAccount = new Account
            {
                Email = email,
                PasswordHash = password,
                RoleId = 3,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _db.Accounts.Add(newAccount);
            await _db.SaveChangesAsync();

            var newProfile = new UserProfile
            {
                AccountId = newAccount.AccountId,
                FullName = fullName,
                PhoneNumber = phoneNumber,
                CreatedAt = DateTime.Now
            };
            _db.UserProfiles.Add(newProfile);
            await _db.SaveChangesAsync();

            var newCustomer = new Customer
            {
                UserProfileId = newProfile.UserProfileId,
                NationalId = nationalId,
                CreatedAt = DateTime.Now
            };
            _db.Customers.Add(newCustomer);
            await _db.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}