namespace ProfilesAPI.Contracts.Responses
{
    public class CreateDoctorResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public CreateDoctorResponse(bool success, string message) 
        {  
            Success = success;
            Message = message;
        }
    }
}
