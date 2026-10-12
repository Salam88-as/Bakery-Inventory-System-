using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BakeryInventorySystem.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentStock { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ReorderLevel { get; set; }

        public int SupplierId { get; set; }
        public virtual Supplier? Supplier { get; set; }

        public string Status => CurrentStock <= ReorderLevel ? "Low Stock" : "In Stock";
    }
}