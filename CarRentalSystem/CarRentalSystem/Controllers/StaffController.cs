using CarRentalSystem.Data;
using CarRentalSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CarRentalSystem.Enums;
using CarRentalSystem.Extensions;

namespace CarRentalSystem.Controllers
{
    public class StaffController : Controller
    {
        private readonly CarRentalContext _context;

        public StaffController(CarRentalContext context)
        {
            _context = context;
        }

        public IActionResult Index(string search, string department)
        {
            var role = HttpContext.Session.GetRoleName();
            if (role != RoleEnums.Admin)
            {
                return RedirectToAction("Login", "Account");
            }

            var query = _context.Staff
                .Include(s => s.UserProfile)
                    .ThenInclude(u => u.Account)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                string searchNum = search.Replace("NV-", "").Replace("nv-", "");
                bool isIdSearch = int.TryParse(searchNum, out int parsedId);

                query = query.Where(s => s.UserProfile.FullName.Contains(search) 
                                      || s.UserProfile.Account.Email.Contains(search)
                                      || (isIdSearch && s.StaffId == parsedId));
            }

            if (!string.IsNullOrEmpty(department) && department != "Tất cả")
            {
                query = query.Where(s => s.Department != null && s.Department.Contains(department));
            }

            var staffs = query.OrderByDescending(s => s.StaffId).ToList();

            ViewBag.SearchString = search;
            ViewBag.CurrentDepartment = department;

            ViewBag.TotalRevenue = _context.Invoices
                .Where(i => i.Status == InvoiceStatus.Paid)
                .Sum(i => (decimal?)i.GrandTotal) ?? 0;

            return View("AdminIndex", staffs);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var role = HttpContext.Session.GetRoleName();
            if (role != RoleEnums.Admin)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string fullName, string email, string phoneNumber, string password, string confirmPassword, string position, string department, string branch)
        {
            var role = HttpContext.Session.GetRoleName();
            if (role != RoleEnums.Admin)
            {
                return RedirectToAction("Login", "Account");
            }

            if (password != confirmPassword)
            {
                ViewBag.Error = "Mật khẩu xác nhận không khớp.";
                return View();
            }

            if (await _context.Accounts.AnyAsync(a => a.Email == email))
            {
                ViewBag.Error = "Email này đã được sử dụng.";
                return View();
            }

            var newAccount = new Account
            {
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                RoleId = RoleIds.Staff,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _context.Accounts.Add(newAccount);
            await _context.SaveChangesAsync();

            var newProfile = new UserProfile
            {
                AccountId = newAccount.AccountId,
                FullName = fullName,
                PhoneNumber = phoneNumber,
                CreatedAt = DateTime.Now
            };
            _context.UserProfiles.Add(newProfile);
            await _context.SaveChangesAsync();

            var newStaff = new Staff
            {
                UserProfileId = newProfile.UserProfileId,
                StaffCode = "NV-" + DateTime.Now.ToString("yyMMddHHmmss"),
                Position = position,
                Department = department,
                Branch = branch,
                HireDate = DateOnly.FromDateTime(DateTime.Now),
                IsActive = true
            };
            _context.Staff.Add(newStaff);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Thêm nhân viên thành công!";
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var role = HttpContext.Session.GetRoleName();
            if (role != RoleEnums.Admin)
            {
                return RedirectToAction("Login", "Account");
            }

            var staff = _context.Staff
                .Include(s => s.UserProfile)
                    .ThenInclude(u => u.Account)
                .FirstOrDefault(s => s.StaffId == id);

            if (staff == null)
            {
                return NotFound();
            }

            return View("Details", staff);
        }
    }
}