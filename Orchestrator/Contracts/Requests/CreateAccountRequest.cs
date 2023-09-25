namespace Orchestrator.Contracts.Requests
{
    public class CreateAccountRequest
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public string PhoneNumber { get; set; }

        public CreateAccountRequest(int userId, string email, string password, Role role, string phoneNumber)
        {
            UserId = userId;
            Email = email;
            Password = password;
            Role = role;
            PhoneNumber = phoneNumber;
        }
    }
}
