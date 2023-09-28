namespace ServicesAPI.Models
{
    public class ServiceCategory
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int TimeSlotSize { get; set; }

        public List<Service> Services { get; set; } = new();
    }
}
