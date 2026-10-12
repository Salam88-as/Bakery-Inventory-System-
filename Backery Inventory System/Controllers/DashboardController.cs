using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BakeryInventorySystem.Models;

namespace BakeryInventorySystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly BakeryDbContext db;
        public DashboardController(BakeryDbContext context) { db = context; }

        public IActionResult Index()
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var currentYear = today.Year;

            var model = new DashboardViewModel
            {
                TotalProducts = db.Products.Count(),
                TotalIngredients = db.Ingredients.Count(),
                LowStockItems = db.Ingredients.Count(i => i.CurrentStock <= i.ReorderLevel),

                TodaySales = db.Sales.Where(s => s.SaleDate.Date == today).Sum(s => (decimal?)s.TotalAmount) ?? 0,
                MonthlySales = db.Sales.Where(s => s.SaleDate >= startOfMonth).Sum(s => (decimal?)s.TotalAmount) ?? 0,

                Months = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" },
                MonthlySalesData = Enumerable.Range(1, 12)
                    .Select(m => db.Sales
                        .Where(s => s.SaleDate.Year == currentYear && s.SaleDate.Month == m)
                        .Sum(s => (decimal?)s.TotalAmount) ?? 0)
                    .Select(v => (int)v)
                    .ToList(),

                InStockCount = db.Ingredients.Count(i => i.CurrentStock > i.ReorderLevel),
                LowStockCount = db.Ingredients.Count(i => i.CurrentStock > 0 && i.CurrentStock <= i.ReorderLevel),
                OutOfStockCount = db.Ingredients.Count(i => i.CurrentStock <= 0),

                LowStockList = db.Ingredients
                    .Where(i => i.CurrentStock <= i.ReorderLevel)
                    .Select(i => new LowStockItem
                    {
                        ItemName = i.Name,
                        Category = "Ingredients",
                        CurrentStock = (int)i.CurrentStock,
                        Unit = i.Unit,
                        ReorderLevel = (int)i.ReorderLevel,
                        Status = "Low Stock"
                    })
                    .ToList(),

                RecentActivities = GetRecentActivities()
            };

            return View(model);
        }

        private List<RecentActivity> GetRecentActivities()
        {
            var activities = new List<RecentActivity>();

            var recentSales = db.Sales
                .OrderByDescending(s => s.SaleDate)
                .Take(3)
                .Select(s => new RecentActivity
                {
                    Icon = "🛒",
                    Title = "New Sale",
                    Description = $"Invoice #{s.InvoiceNo} completed",
                    DateTime = s.SaleDate.ToString("M/d/yyyy hh:mm tt")
                });

            var recentStockIn = db.StockIns
                .OrderByDescending(s => s.DateReceived)
                .Take(3)
                .Select(s => new RecentActivity
                {
                    Icon = "⬇",
                    Title = "Stock In",
                    Description = $"{(s.Ingredient != null ? s.Ingredient.Name : "Ingredient")} - {s.Quantity} {(s.Ingredient != null ? s.Ingredient.Unit : "")} added",
                    DateTime = s.DateReceived.ToString("M/d/yyyy hh:mm tt")
                });

            var recentStockOut = db.StockOuts
                .OrderByDescending(s => s.DateOut)
                .Take(3)
                .Select(s => new RecentActivity
                {
                    Icon = "⬆",
                    Title = "Stock Out",
                    Description = $"{(s.Ingredient != null ? s.Ingredient.Name : "Ingredient")} - {s.Quantity} {(s.Ingredient != null ? s.Ingredient.Unit : "")} removed ({s.Reason})",
                    DateTime = s.DateOut.ToString("M/d/yyyy hh:mm tt")
                });

            activities.AddRange(recentSales);
            activities.AddRange(recentStockIn);
            activities.AddRange(recentStockOut);

            return activities
                .OrderByDescending(a => DateTime.Parse(a.DateTime))
                .Take(5)
                .ToList();
        }
    }
}