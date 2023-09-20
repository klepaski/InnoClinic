namespace ProfilesAPI.Contracts.Responses
{
    public class GetDoctorsResponse
    {
        public string? PhotoUrl { get; set; }
        public string FullName { get; set; }
        public string Specialization { get; set; }
        public int Experience { get; set; }
        public string OfficeAddress { get; set; }
    }
}
