namespace ServicesAPI.Contracts.Requests
{
    public class CreateServiceRequest
    {
        public string ServiceName { get; set; }
        public float Price { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId {  get; set; }
        public int SpecializationId { get; set; }
    }

    public class UpdateServiceRequest
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public float Price { get; set; }
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
    }
}
