namespace Orchestrator.Contracts.Responses
{
    public class AccountResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsEmailVerified { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
