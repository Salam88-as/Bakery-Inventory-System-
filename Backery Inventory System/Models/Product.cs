using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BakeryInventorySystem.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;
    }
}