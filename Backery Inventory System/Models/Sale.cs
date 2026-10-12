using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BakeryInventorySystem.Models
{
    public class Sale
    {
        public int SaleId { get; set; }
        public string InvoiceNo { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public string? CustomerName { get; set; }
        public string? PaymentMethod { get; set; }

        public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
    }

    public class SaleItem
    {
        public int SaleItemId { get; set; }
        public int SaleId { get; set; }
        public virtual Sale? Sale { get; set; }

        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }
    }
}