using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BakeryInventorySystem.Models;

namespace BakeryInventorySystem.Controllers
{
    public class ReportsController : Controller
    {
        private readonly BakeryDbContext db;
        public ReportsController(BakeryDbContext context) { db = context; }

        public IActionResult Index(DateTime? fromDate, DateTime? toDate)
        {
            var from = fromDate ?? DateTime.Today.AddDays(-30);
            var to = toDate ?? DateTime.Today;
            var toInclusive = to.AddDays(1).AddTicks(-1);

            var salesInRange = db.Sales
                .Where(s => s.SaleDate >= from && s.SaleDate <= toInclusive)
                .OrderByDescending(s => s.SaleDate)
                .ToList();

            var topProducts = db.SaleItems
                .Where(si => si.Sale != null && si.Sale.SaleDate >= from && si.Sale.SaleDate <= toInclusive)
                .GroupBy(si => si.Product != null ? si.Product.Name : "Unknown")
                .Select(g => new TopProductReport
                {
                    ProductName = g.Key,
                    TotalQuantitySold = g.Sum(x => x.Quantity),
                    TotalRevenue = g.Sum(x => x.Subtotal)
                })
                .OrderByDescending(p => p.TotalRevenue)
                .Take(5)
                .ToList();

            var model = new ReportViewModel
            {
                FromDate = from,
                ToDate = to,
                SalesList = salesInRange,
                TotalSalesAmount = salesInRange.Sum(s => s.TotalAmount),
                TotalSalesCount = salesInRange.Count,
                TopProducts = topProducts,
                LowStockReport = db.Ingredients.Where(i => i.CurrentStock <= i.ReorderLevel).ToList()
            };

            return View(model);
        }
    }
}