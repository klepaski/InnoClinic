namespace ProfilesAPI.Contracts.Responses
{
    public class GetDoctorResponse
    {
        public int Id { get; set; }
        public string? PhotoUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int CareerStartYear { get; set; }
        public int Experience { get; set; }
        public string Specialization { get; set; }
        public int OfficeId { get; set; }
        public string OfficeAddress { get; set; }
        public string Status { get; set; }
    }
}
