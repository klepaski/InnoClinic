namespace JuliaChistyakovaPackage
{
    public enum Role
    {
        Patient,
        Doctor,
        Receptionist
    }

    public static class Ports
    {
        public const string Orchestrator = "http://localhost:5009";
        public const string GatewayAPI = "http://localhost:5000";
        public const string AuthAPI = "http://localhost:5001";
        public const string OfficesAPI = "http://localhost:5002";
        public const string ProfilesAPI = "http://localhost:5003";
        public const string ServicesAPI = "http://localhost:5004";
        public const string AppointmentsAPI = "http://localhost:5005";
        public const string DocumentsAPI = "http://localhost:5006";
    }

    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public Role Role { get; set; }
    }

    public class CreateUserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

        public CreateUserRequest(string email, string password, Role role)
        {
            Email = email;
            Password = password;
            Role = role;
        }
    }

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

    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public string PhoneNumber { get; set; }
    }
}
