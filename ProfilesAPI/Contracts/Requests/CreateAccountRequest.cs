namespace ProfilesAPI.Contracts.Requests
{
    public enum Role
    {
        Patient,
        Doctor,
        Receptionist
    }

    public class CreateAccountRequest
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public string PhoneNumber { get; set; }
    }
}
