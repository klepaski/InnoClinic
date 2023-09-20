namespace ProfilesAPI.Models
{
    public enum Status
    {
        AtWork,
        OnVacation,
        SickDay,
        SickLeave,
        SelfIsolation,
        LeaveWithoutPay,
        Inactive
    }

    public class Doctor
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }

        public int SpecializationId { get; set; }
        public DoctorSpecialization DoctorSpecialization { get; set; }

        public int OfficeId { get; set; }

        public int CareerStartYear { get; set; }
        public Status Status { get; set; }

        //redundancy
        public string OfficeAddress { get; set; }
        public string RegistryPhoneNumber { get; set; }
    }
}
