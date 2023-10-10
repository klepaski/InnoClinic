namespace AppointmentsAPI.Contracts.Requests
{
    public class UpdateResultRequest
    {
        public int Id { get; set; }
        public string Complaints { get; set; }
        public string Conclusion { get; set; }
        public string Recommendations { get; set; }
    }
}
