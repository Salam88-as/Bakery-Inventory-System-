using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BakeryInventorySystem.Models;

namespace BakeryInventorySystem.Controllers
{
    public class StockOutController : Controller
    {
        private readonly BakeryDbContext db;
        public StockOutController(BakeryDbContext context) { db = context; }

        public IActionResult Index()
        {
            var list = db.StockOuts.Include(s => s.Ingredient).OrderByDescending(s => s.DateOut).ToList();
            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.IngredientId = new SelectList(db.Ingredients, "IngredientId", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(StockOut stockOut)
        {
            if (ModelState.IsValid)
            {
                var ingredient = db.Ingredients.Find(stockOut.IngredientId);
                if (ingredient != null && ingredient.CurrentStock >= stockOut.Quantity)
                {
                    ingredient.CurrentStock -= stockOut.Quantity;
                    db.StockOuts.Add(stockOut);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Not enough stock available!");
            }
            ViewBag.IngredientId = new SelectList(db.Ingredients, "IngredientId", "Name", stockOut.IngredientId);
            return View(stockOut);
        }
    }
}

