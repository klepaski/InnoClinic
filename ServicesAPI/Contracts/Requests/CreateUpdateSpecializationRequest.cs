using ServicesAPI.Contracts.Responses;

namespace ServicesAPI.Contracts.Requests
{
    public class CreateSpecializationRequest
    {
        public string SpecializationName { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateSpecializationRequest
    {
        public int Id { get; set; }
        public string SpecializationName { get; set; }
        public bool IsActive { get; set; }
    }
}
