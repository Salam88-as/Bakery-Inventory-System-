using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BakeryInventorySystem.Models;

namespace BakeryInventorySystem.Controllers
{
    public class SalesController : Controller
    {
        private readonly BakeryDbContext db;
        public SalesController(BakeryDbContext context) { db = context; }

        public IActionResult Index() => View(db.Sales.OrderByDescending(s => s.SaleDate).ToList());

        public IActionResult Create()
        {
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Sale sale, int ProductId, int Quantity)
        {
            var product = db.Products.Find(ProductId);
            if (product != null && ModelState.IsValid)
            {
                sale.InvoiceNo = "INV-" + DateTime.Now.Ticks.ToString().Substring(10);
                sale.TotalAmount = product.Price * Quantity;
                db.Sales.Add(sale);
                db.SaveChanges();

                var item = new SaleItem
                {
                    SaleId = sale.SaleId,
                    ProductId = ProductId,
                    Quantity = Quantity,
                    UnitPrice = product.Price,
                    Subtotal = product.Price * Quantity
                };
                db.SaleItems.Add(item);
                product.StockQuantity -= Quantity;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name");
            return View(sale);
        }
    }
}

