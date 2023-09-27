using ServicesAPI.Contracts.Responses;

namespace ServicesAPI.Models
{
    public class Service
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string ServiceName { get; set; }
        public float Price { get; set; }
        public int SpecializationId { get; set; }
        public bool IsActive { get; set; }

        public ServiceCategory ServiceCategory { get; set; }
        public Specialization Specialization { get; set; }

        public GetServiceResponse ToResponse()
        {
            return new GetServiceResponse
            {
                Id = Id,
                ServiceName = ServiceName,
                Price = Price,
                CategoryId = CategoryId,
                CategoryName = ServiceCategory.CategoryName,
                IsActive = IsActive
            };
        }
    }
}
