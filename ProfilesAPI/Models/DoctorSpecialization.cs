namespace ProfilesAPI.Models
{
    public class DoctorSpecialization
    {
        public int Id { get; set; }
        public string SpecializationName { get; set; }
        public bool IsActive { get; set; }

        public List<Doctor> Doctors { get; set; } = new();
    }
}
