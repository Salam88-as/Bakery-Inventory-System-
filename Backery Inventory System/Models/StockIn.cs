using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BakeryInventorySystem.Models
{
    public class StockIn
    {
        public int StockInId { get; set; }

        public int IngredientId { get; set; }
        public virtual Ingredient? Ingredient { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        public int SupplierId { get; set; }
        public virtual Supplier? Supplier { get; set; }

        public DateTime DateReceived { get; set; } = DateTime.Now;

        public string? Notes { get; set; }
    }
}