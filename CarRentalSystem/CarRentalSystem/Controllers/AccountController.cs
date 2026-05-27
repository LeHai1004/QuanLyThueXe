using CarRentalSystem.Data;
using CarRentalSystem.Models;
using CarRentalSystem.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

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
                if (role == RoleConstants.Admin || role == RoleConstants.Staff)
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
                ViewBag.LoginError = "Email hoặc mật khẩu không chính xác.";
                return View();
            }

            if (!account.IsActive)
            {
                ViewBag.LoginError = "Tài khoản của bạn đã bị khóa.";
                return View();
            }

            HttpContext.Session.SetString("AccountId", account.AccountId.ToString());
            HttpContext.Session.SetString("RoleName", account.Role.TenRole);
            HttpContext.Session.SetString("Email", account.Email);
            HttpContext.Session.SetString("FullName", account.UserProfile?.FullName ?? "Người dùng");

            if (account.Role.TenRole == RoleConstants.Admin || account.Role.TenRole == RoleConstants.Staff)
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
                return View("Login");
            }

            if (await _db.Accounts.AnyAsync(a => a.Email == email))
            {
                ViewBag.RegisterError = "Email này đã được sử dụng.";
                return View("Login");
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

            TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var accountIdStr = HttpContext.Session.GetString("AccountId");
            if (string.IsNullOrEmpty(accountIdStr))
                return RedirectToAction("Login");

            int accountId = int.Parse(accountIdStr);
            var customer = _db.Customers
                .Include(c => c.UserProfile)
                    .ThenInclude(u => u.Account)
                .FirstOrDefault(c => c.UserProfile.AccountId == accountId);

            if (customer == null)
                return RedirectToAction("Login");

            return View("CustomerProfile", customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateProfile(string FullName, string PhoneNumber, string? DateOfBirth,
                                           string? NationalId, string? LicenseNumber, string? StreetAddress)
        {
            var accountIdStr = HttpContext.Session.GetString("AccountId");
            if (string.IsNullOrEmpty(accountIdStr))
                return RedirectToAction("Login");

            int accountId = int.Parse(accountIdStr);
            var customer = _db.Customers
                .Include(c => c.UserProfile)
                .FirstOrDefault(c => c.UserProfile.AccountId == accountId);

            if (customer == null)
                return RedirectToAction("Login");

            // Cập nhật UserProfile
            customer.UserProfile.FullName = FullName;
            customer.UserProfile.PhoneNumber = PhoneNumber;
            customer.UserProfile.StreetAddress = StreetAddress;
            if (!string.IsNullOrEmpty(DateOfBirth))
                customer.UserProfile.DateOfBirth = DateOnly.Parse(DateOfBirth);

            // Cập nhật Customer
            customer.NationalId = NationalId;
            customer.LicenseNumber = LicenseNumber;

            _db.SaveChanges();

            // Cập nhật lại session
            HttpContext.Session.SetString("FullName", FullName);

            TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
            return RedirectToAction("Profile");
        }

        [HttpGet]
        public IActionResult MyReviews()
        {
            var accountIdStr = HttpContext.Session.GetString("AccountId");
            if (string.IsNullOrEmpty(accountIdStr))
                return RedirectToAction("Login");

            int accountId = int.Parse(accountIdStr);
            var customer = _db.Customers
                .Include(c => c.UserProfile)
                .FirstOrDefault(c => c.UserProfile.AccountId == accountId);

            if (customer == null)
                return RedirectToAction("Login");

            var reviews = _db.Reviews
                .Include(r => r.Vehicle)
                .Include(r => r.Booking)
                .Where(r => r.CustomerId == customer.CustomerId)
                .OrderByDescending(r => r.NgayReview)
                .ToList();

            ViewBag.Profile = customer.UserProfile;
            return View("MyReviews", reviews);
        }
        [HttpGet]
        public IActionResult DoiMatKhau()
        {
            if (HttpContext.Session.GetString("AccountId") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DoiMatKhau(string oldPassword, string newPassword, string confirmPassword)
        {
            var accountIdStr = HttpContext.Session.GetString("AccountId");
            if (string.IsNullOrEmpty(accountIdStr))
            {
                return RedirectToAction("Login", "Account");
            }

            if (newPassword != confirmPassword)
            {
                ViewBag.Error = "Mật khẩu mới và xác nhận mật khẩu không trùng khớp.";
                return View();
            }

            int accountId = int.Parse(accountIdStr);
            var account = await _db.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId);

            if (account == null || account.PasswordHash != oldPassword)
            {
                ViewBag.Error = "Mật khẩu hiện tại không chính xác.";
                return View();
            }

            account.PasswordHash = newPassword;
            account.UpdatedAt = DateTime.Now;
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Thay đổi mật khẩu hệ thống thành công!";
            return View();
        }
    }
}