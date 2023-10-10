namespace AppointmentsAPI.Contracts.Requests
{
    public class CreateAppointmentRequest
    {
        public int SpecializationId {  get; set; }  //active
        public int PatientId { get; set; }
        public int DoctorId { get; set; }   //at work
        public int ServiceId { get; set; }  //active
        public int OfficeId { get; set; }   //active
        public DateTime DateTime { get; set; }
        //public ?? TimeSlot
    }
}
