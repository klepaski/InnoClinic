using ServicesAPI.Contracts.Responses;

namespace ServicesAPI.Models
{
    public class Specialization
    {
        public int Id { get; set; }
        public string SpecializationName { get; set; }
        public bool IsActive { get; set; }

        public List<Service> Services { get; set; } = new();

        public GetSpecializationResponse ToResponse()
        {
            return new GetSpecializationResponse
            {
                Id = Id,
                SpecializationName = SpecializationName,
                IsActive = IsActive,
                Services = Services.Select(x => x.ToResponse()).ToList()
            };
        }
    }
}
