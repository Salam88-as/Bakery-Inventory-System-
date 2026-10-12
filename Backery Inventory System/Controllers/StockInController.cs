using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BakeryInventorySystem.Models;

namespace BakeryInventorySystem.Controllers
{
    public class StockInController : Controller
    {
        private readonly BakeryDbContext db;
        public StockInController(BakeryDbContext context) { db = context; }

        public IActionResult Index()
        {
            var list = db.StockIns.Include(s => s.Ingredient).Include(s => s.Supplier)
                          .OrderByDescending(s => s.DateReceived).ToList();
            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.IngredientId = new SelectList(db.Ingredients, "IngredientId", "Name");
            ViewBag.SupplierId = new SelectList(db.Suppliers, "SupplierId", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(StockIn stockIn)
        {
            if (ModelState.IsValid)
            {
                db.StockIns.Add(stockIn);
                var ingredient = db.Ingredients.Find(stockIn.IngredientId);
                if (ingredient != null) ingredient.CurrentStock += stockIn.Quantity;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IngredientId = new SelectList(db.Ingredients, "IngredientId", "Name", stockIn.IngredientId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "SupplierId", "Name", stockIn.SupplierId);
            return View(stockIn);
        }
    }
}

