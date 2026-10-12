using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BakeryInventorySystem.Models;

namespace BakeryInventorySystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly BakeryDbContext db;
        private readonly IPasswordHasher<AppUser> hasher;

        public AccountController(BakeryDbContext context, IPasswordHasher<AppUser> passwordHasher)
        {
            db = context;
            hasher = passwordHasher;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Please enter username and password";
                return View();
            }

            var user = db.Users.FirstOrDefault(u => u.Username == username && u.IsActive);

            if (user != null)
            {
                var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);
                if (result == PasswordVerificationResult.Success)
                {
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("UserName", user.FullName ?? "");
                    HttpContext.Session.SetString("UserRole", user.Role ?? "");
                    return RedirectToAction("Index", "Dashboard");
                }
            }

            ViewBag.Error = "Invalid username or password";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult ChangePassword()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login");
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login");

            var user = db.Users.Find(userId);
            if (user == null) return RedirectToAction("Login");

            if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                ViewBag.Error = "All fields are required";
                return View();
            }

            var checkResult = hasher.VerifyHashedPassword(user, user.PasswordHash, currentPassword);
            if (checkResult != PasswordVerificationResult.Success)
            {
                ViewBag.Error = "Current password is incorrect";
                return View();
            }

            if (newPassword != confirmPassword)
            {
                ViewBag.Error = "New password and confirm password do not match";
                return View();
            }

            if (newPassword.Length < 6)
            {
                ViewBag.Error = "New password must be at least 6 characters";
                return View();
            }

            user.PasswordHash = hasher.HashPassword(user, newPassword);
            db.SaveChanges();

            ViewBag.Success = "Password changed successfully!";
            return View();
        }
    }
}