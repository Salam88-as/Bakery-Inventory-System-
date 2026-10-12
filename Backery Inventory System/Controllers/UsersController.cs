using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BakeryInventorySystem.Models;

namespace BakeryInventorySystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly BakeryDbContext db;
        private readonly IPasswordHasher<AppUser> hasher;

        public UsersController(BakeryDbContext context, IPasswordHasher<AppUser> passwordHasher)
        {
            db = context;
            hasher = passwordHasher;
        }

        public IActionResult Index() => View(db.Users.ToList());

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(AppUser user, string password)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(password))
            {
                user.PasswordHash = hasher.HashPassword(user, password);
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public IActionResult Delete(int id)
        {
            var user = db.Users.Find(id);
            if (user == null) return NotFound();
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}