namespace ServicesAPI.Contracts.Responses
{
    public class GetServiceResponse
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public float Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsActive { get; set; }
    }
}
