namespace Invoicing.Api.Models
{
    public class InvoiceItem
    {
        public int InvoiceItemId { get; set; }
        public int InvoiceId { get; set; }

        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Rate { get; set; }

        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal LineTotal { get; set; }
    }
}
