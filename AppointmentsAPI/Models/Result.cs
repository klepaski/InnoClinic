using AppointmentsAPI.Contracts.Responses;

namespace AppointmentsAPI.Models
{
    public class Result
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
        public string Complaints { get; set; }
        public string Conclusion { get; set; }
        public string Recommendations { get; set; }

        public GetResultResponse ToResponse()
        {
            return new GetResultResponse
            {
                Id = Id,
                DateTime = Appointment.DateTime,
                PatientName = Appointment.PatientName,
                PatientBirthday = Appointment.PatientBirthday,
                DoctorName = Appointment.DoctorName,
                DoctorSpecialization = Appointment.DoctorSpecialization,
                ServiceName = Appointment.ServiceName,
                Complaints = Complaints,
                Conclusion = Conclusion,
                Recommendations = Recommendations
            };
        }
    }
}
