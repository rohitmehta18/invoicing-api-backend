using System.ComponentModel.DataAnnotations;

namespace Invoicing.Api.DTOs
{
    public class InvoiceItemDto
    {
        [Required]
        public string ProductName { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Rate { get; set; }

        [Range(0, 100)]
        public decimal DiscountPercent { get; set; }

        [Range(0, double.MaxValue)]
        public decimal DiscountAmount { get; set; }
    }
}
