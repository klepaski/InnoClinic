namespace ProfilesAPI.Models
{
    public class Receptionist
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }

        public int OfficeId { get; set; }

        //redundancy
        public string OfficeAddress { get; set; }
        public string RegistryPhoneNumber { get; set; }
    }
}
