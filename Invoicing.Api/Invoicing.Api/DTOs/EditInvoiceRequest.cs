using System.ComponentModel.DataAnnotations;

namespace Invoicing.Api.DTOs
{
    public class EditInvoiceRequest
    {
        [Required]
        public DateTime InvoiceDate { get; set; }

        // Billing
        [Required]
        public string BillingAddressLine1 { get; set; } = string.Empty;
        public string? BillingAddressLine2 { get; set; }

        [Required]
        public string BillingCity { get; set; } = string.Empty;

        [Required]
        public string BillingState { get; set; } = string.Empty;

        public string? BillingPostalCode { get; set; }

        // Shipping
        [Required]
        public string ShippingAddressLine1 { get; set; } = string.Empty;
        public string? ShippingAddressLine2 { get; set; }

        [Required]
        public string ShippingCity { get; set; } = string.Empty;

        [Required]
        public string ShippingState { get; set; } = string.Empty;

        public string? ShippingPostalCode { get; set; }

        // Items
        [Required]
        public List<InvoiceItemDto> Items { get; set; } = new();
    }
}
