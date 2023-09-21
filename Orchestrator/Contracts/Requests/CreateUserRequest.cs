namespace Orchestrator.Contracts.Requests
{
    public class CreateUserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
