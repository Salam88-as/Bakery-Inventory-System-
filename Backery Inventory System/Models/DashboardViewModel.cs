using System.Collections.Generic;

namespace BakeryInventorySystem.Models
{
    public class DashboardViewModel
    {
        public int TotalProducts { get; set; }
        public int TotalIngredients { get; set; }
        public int LowStockItems { get; set; }
        public decimal TodaySales { get; set; }
        public decimal MonthlySales { get; set; }

        public List<string> Months { get; set; } = new List<string>();
        public List<int> MonthlySalesData { get; set; } = new List<int>();

        public int InStockCount { get; set; }
        public int LowStockCount { get; set; }
        public int OutOfStockCount { get; set; }

        public List<LowStockItem> LowStockList { get; set; } = new List<LowStockItem>();
        public List<RecentActivity> RecentActivities { get; set; } = new List<RecentActivity>();
    }

    public class LowStockItem
    {
        public string ItemName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int CurrentStock { get; set; }
        public string Unit { get; set; } = string.Empty;
        public int ReorderLevel { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class RecentActivity
    {
        public string Icon { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DateTime { get; set; } = string.Empty;
    }
}