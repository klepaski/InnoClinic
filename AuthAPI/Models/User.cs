namespace AuthAPI.Models
{
    public enum Role
    {
        Patient,
        Doctor,
        Receptionist
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

    // Влад сказал
    //public class User
    //{
    //    public int Id { get; set; }
    //    public string? RefreshToken { get; set; }
    //    public DateTime? RefreshTokenExpiryTime { get; set; }
    //    public Role Role { get; set; }
    //}
}
