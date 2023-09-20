namespace ProfilesAPI.Contracts.Responses
{
    public class GetDoctorByDoctorResponse
    {
        public string? PhotoUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Specialization {  get; set; }

        public int OfficeId { get; set; }
        public string OfficeAddress { get; set; }

        public int CareerStartYear { get; set; }
    }
}
