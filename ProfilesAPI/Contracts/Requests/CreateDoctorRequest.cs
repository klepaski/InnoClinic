using ProfilesAPI.Models;

namespace ProfilesAPI.Contracts.Requests
{
    public class CreateDoctorRequest
    {
        public string? PhotoUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        
        public int SpecializationId { get; set; }   //
        public int OfficeId { get; set; }           //

        public int CareerStartYear { get; set; }
        public Status Status { get; set; }
    }
}
