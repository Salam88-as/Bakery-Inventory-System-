using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BakeryInventorySystem.Models;

namespace BakeryInventorySystem.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly BakeryDbContext db;
        public SuppliersController(BakeryDbContext context) { db = context; }

        public IActionResult Index() => View(db.Suppliers.ToList());

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Suppliers.Add(supplier);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        public IActionResult Edit(int id)
        {
            var supplier = db.Suppliers.Find(id);
            if (supplier == null) return NotFound();
            return View(supplier);
        }

        [HttpPost]
        public IActionResult Edit(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        public IActionResult Delete(int id)
        {
            var supplier = db.Suppliers.Find(id);
            if (supplier == null) return NotFound();
            db.Suppliers.Remove(supplier);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

