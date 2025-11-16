namespace Invoicing.Api.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public DateTime InvoiceDate { get; set; }

        // Billing
        public string BillingAddressLine1 { get; set; } = string.Empty;
        public string? BillingAddressLine2 { get; set; }
        public string BillingCity { get; set; } = string.Empty;
        public string BillingState { get; set; } = string.Empty;
        public string? BillingPostalCode { get; set; }

        // Shipping
        public string ShippingAddressLine1 { get; set; } = string.Empty;
        public string? ShippingAddressLine2 { get; set; }
        public string ShippingCity { get; set; } = string.Empty;
        public string ShippingState { get; set; } = string.Empty;
        public string? ShippingPostalCode { get; set; }

        public decimal Subtotal { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Active";
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
