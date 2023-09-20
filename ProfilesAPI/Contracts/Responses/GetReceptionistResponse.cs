using Microsoft.Identity.Client;

namespace ProfilesAPI.Contracts.Responses
{
    public class GetReceptionistResponse
    {
        public int Id { get; set; }
        public string? PhotoUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int OfficeId { get; set; }
        public string OfficeAddress { get; set; }
    }
}
