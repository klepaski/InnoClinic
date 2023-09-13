using OfficesAPI.Models;

namespace OfficesAPI.Contracts.Requests
{
    public class UpdateOfficeRequest
    {
        public int Id { get; set; }
        public string? PhotoUrl { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string OfficeNumber { get; set; }
        public string RegistryPhoneNumber { get; set; }
        public Status Status { get; set; }
    }
}
