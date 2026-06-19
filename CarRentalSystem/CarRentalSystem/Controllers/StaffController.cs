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
                string searchNum = search.Replace(CodePrefix.Staff, "").Replace(CodePrefix.Staff.ToLower(), "");
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

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
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
                    StaffCode = CodePrefix.Staff + DateTime.Now.ToString("yyMMddHHmmss"),
                    Position = position,
                    Department = department,
                    Branch = branch,
                    HireDate = DateOnly.FromDateTime(DateTime.Now),
                    IsActive = true
                };
                _context.Staff.Add(newStaff);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                ViewBag.Error = "Đã xảy ra lỗi khi tạo tài khoản nhân viên. Vui lòng thử lại.";
                return View();
            }

            TempData["SuccessMessage"] = "Thêm nhân viên thành công!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
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

            return View(staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string fullName, string phoneNumber, string department, string position, string branch, bool isActive = false)
        {
            var role = HttpContext.Session.GetRoleName();
            if (role != RoleEnums.Admin)
            {
                return RedirectToAction("Login", "Account");
            }

            var staff = await _context.Staff
                .Include(s => s.UserProfile)
                .FirstOrDefaultAsync(s => s.StaffId == id);

            if (staff == null)
            {
                return NotFound();
            }

            // Update UserProfile
            if (staff.UserProfile != null)
            {
                staff.UserProfile.FullName = fullName;
                staff.UserProfile.PhoneNumber = phoneNumber;
            }

            // Update Staff
            staff.Department = department;
            staff.Position = position;
            staff.Branch = branch;
            staff.IsActive = isActive;

            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = "Cập nhật thông tin nhân viên thành công!";
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