namespace AppointmentsAPI.Contracts.Requests
{
    public class CreateResultRequest
    {
        public int AppointmentId { get; set; }
        //prefilled
        //public DateTime Date { get; set; }
        //public string PatientName { get; set; }
        //public DateTime PatientBirthday { get; set; }
        //public string DoctorName { get; set; }
        //public int SpecializationId { get; set; }
        //public int ServiceId { get; set; }

        public string Complaints { get; set; }
        public string Conclusion { get; set; }
        public string Recommendations { get; set; }
    }
}
