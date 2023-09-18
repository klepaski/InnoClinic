namespace ProfilesAPI.Contracts.Responses
{
    public class CreateUpdateResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public CreateUpdateResponse(bool success, string message) 
        {  
            Success = success;
            Message = message;
        }
    }
}
