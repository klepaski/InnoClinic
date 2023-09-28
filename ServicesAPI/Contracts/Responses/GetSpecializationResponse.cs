namespace ServicesAPI.Contracts.Responses
{
    public class GetSpecializationResponse
    {
        public int Id { get; set; }
        public string SpecializationName { get; set; }
        public bool IsActive { get; set; }
        //Services
        public List<GetServiceResponse> Services { get; set; }
    }
}
