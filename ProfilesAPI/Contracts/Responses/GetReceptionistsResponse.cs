using Microsoft.Identity.Client;

namespace ProfilesAPI.Contracts.Responses
{
    public class GetReceptionistsResponse
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string OfficeAddress { get; set; }
    }
}
