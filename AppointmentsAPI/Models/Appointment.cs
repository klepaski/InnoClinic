namespace AppointmentsAPI.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int ServiceId { get; set; }
        public int SpecializationId { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsApproved { get; set; }

        public Result Result { get; set; }

        //redundancy
        public string DoctorName { get; set; }
        public string DoctorSpecialization { get; set; }

        public string PatientName { get; set; }
        public string? PatientPhoneNumber { get; set; }
        public DateTime PatientBirthday { get; set; }

        public string ServiceName { get; set; }
        public float ServicePrice { get; set; }
    }
}
