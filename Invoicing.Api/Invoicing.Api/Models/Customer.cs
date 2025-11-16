namespace Invoicing.Api.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
