namespace Invoicing.Api.DTOs
{
    public class InvoiceResponse
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public DateTime InvoiceDate { get; set; }

        public decimal Subtotal { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Active";

        public List<InvoiceItemResponse> Items { get; set; } = new();
    }

    public class InvoiceItemResponse
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal LineTotal { get; set; }
    }
}
