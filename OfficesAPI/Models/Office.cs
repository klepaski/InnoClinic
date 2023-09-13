namespace OfficesAPI.Models
{
    public enum Status
    {
        Active,
        Inactive
    }

    public class Address
    {
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string OfficeNumber { get; set; }

        public override string ToString() => $"{City}, {Street} street, {HouseNumber}. Office: {OfficeNumber}";
    }

    public class Office
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int? PhotoId { get; set; }
        public string? PhotoUrl { get; set; }
        public string RegistryPhoneNumber { get; set; }
        public Status Status { get; set; }

        public Receptionist? Receptionist { get; set; }
    }
}
