namespace ProfilesAPI.Models
{
    public class Account
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsEmailVerified { get; set; }
        public int? PhotoId { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Doctor? Doctor { get; set; }
        public Patient? Patient { get; set; }
        public Receptionist? Receptionist { get; set; }

        //redundancy
        public string? PhotoUrl { get; set; }
    }
}
