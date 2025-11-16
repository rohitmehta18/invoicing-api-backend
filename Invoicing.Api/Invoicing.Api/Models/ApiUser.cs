namespace Invoicing.Api.Models
{
    public class ApiUser
    {
        public int ApiUserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public Guid ApiKey { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
