using ProfilesAPI.Models;

namespace ProfilesAPI.Contracts.Requests
{
    public class CreatePatientRequest
    {
        public string? PhotoUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class CreatePatientByAdminRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class UpdatePatientRequest
    {
        public int Id { get; set; }
        public string? PhotoUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
