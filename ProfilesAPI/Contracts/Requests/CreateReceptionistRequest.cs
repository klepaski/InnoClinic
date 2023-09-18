namespace ProfilesAPI.Contracts.Requests
{
    public class CreateReceptionistRequest
    {
        public string? PhotoUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public int OfficeId { get; set; }
    }
}
