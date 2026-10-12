using System.Collections.Generic;

namespace BakeryInventorySystem.Models
{
    public class ReportViewModel
    {
        public decimal TotalSalesAmount { get; set; }
        public int TotalSalesCount { get; set; }
        public List<Sale> SalesList { get; set; } = new List<Sale>();

        public List<TopProductReport> TopProducts { get; set; } = new List<TopProductReport>();
        public List<Ingredient> LowStockReport { get; set; } = new List<Ingredient>();

        public System.DateTime FromDate { get; set; }
        public System.DateTime ToDate { get; set; }
    }

    public class TopProductReport
    {
        public string ProductName { get; set; } = string.Empty;
        public int TotalQuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}