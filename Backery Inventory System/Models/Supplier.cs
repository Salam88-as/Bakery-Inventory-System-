using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BakeryInventorySystem.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; } = string.Empty;

        public string? ContactPerson { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

        public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    }
}