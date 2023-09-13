namespace OfficesAPI.Models
{
    public class Receptionist
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int AccountId { get; set; }

        public int OfficeId { get; set; }
        public Office Office { get; set; }
    }
}
