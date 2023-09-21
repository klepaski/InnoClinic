using Orchestrator.Contracts.Requests;

namespace Orchestrator.Contracts.Responses
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Role Role { get; set; }
    }
}