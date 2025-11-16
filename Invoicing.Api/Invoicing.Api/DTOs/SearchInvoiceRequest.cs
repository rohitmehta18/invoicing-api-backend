namespace Invoicing.Api.DTOs
{
    public class SearchInvoiceRequest
    {
        public int? CustomerId { get; set; }
        public string? InvoiceNumber { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
