using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BakeryInventorySystem.Models
{
    public class StockOut
    {
        public int StockOutId { get; set; }

        public int IngredientId { get; set; }
        public virtual Ingredient? Ingredient { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        public string? Reason { get; set; }

        public DateTime DateOut { get; set; } = DateTime.Now;
    }
}