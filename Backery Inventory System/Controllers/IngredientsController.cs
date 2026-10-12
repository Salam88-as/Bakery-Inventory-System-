using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BakeryInventorySystem.Models;

namespace BakeryInventorySystem.Controllers
{
    public class IngredientsController : Controller
    {
        private readonly BakeryDbContext db;
        public IngredientsController(BakeryDbContext context) { db = context; }

        public IActionResult Index() => View(db.Ingredients.Include(i => i.Supplier).ToList());

        public IActionResult Create()
        {
            ViewBag.SupplierId = new SelectList(db.Suppliers, "SupplierId", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                db.Ingredients.Add(ingredient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SupplierId = new SelectList(db.Suppliers, "SupplierId", "Name", ingredient.SupplierId);
            return View(ingredient);
        }

        public IActionResult Edit(int id)
        {
            var ingredient = db.Ingredients.Find(id);
            if (ingredient == null) return NotFound();
            ViewBag.SupplierId = new SelectList(db.Suppliers, "SupplierId", "Name", ingredient.SupplierId);
            return View(ingredient);
        }

        [HttpPost]
        public IActionResult Edit(Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ingredient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SupplierId = new SelectList(db.Suppliers, "SupplierId", "Name", ingredient.SupplierId);
            return View(ingredient);
        }

        public IActionResult Delete(int id)
        {
            var ingredient = db.Ingredients.Find(id);
            if (ingredient == null) return NotFound();
            db.Ingredients.Remove(ingredient);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

