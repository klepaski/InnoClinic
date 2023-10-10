namespace AppointmentsAPI.Contracts.Responses
{
    public class GetResultResponse
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string PatientName { get; set; }
        public DateTime PatientBirthday { get; set; }
        public string DoctorName { get; set; }
        public string DoctorSpecialization { get; set; }
        public string ServiceName { get; set; }
        public string Complaints { get; set; }
        public string Conclusion { get; set; }
        public string Recommendations { get; set; }
    }
}
