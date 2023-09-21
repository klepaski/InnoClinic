namespace ProfilesAPI.Contracts.Responses
{
    public class GetOfficeResponse
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string? PhotoUrl { get; set; }
        public string RegistryPhoneNumber { get; set; }
    }
}
