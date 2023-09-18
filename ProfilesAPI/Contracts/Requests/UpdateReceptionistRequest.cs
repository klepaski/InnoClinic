namespace ProfilesAPI.Contracts.Requests
{
    public class UpdateReceptionistRequest
    {
        public int Id { get; set; }
        public string? PhotoUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int OfficeId { get; set; }
    }
}
