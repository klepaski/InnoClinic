namespace ServicesAPI.Contracts.Responses
{
    public class GeneralResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public GeneralResponse(bool success, string message) 
        {  
            Success = success;
            Message = message;
        }
    }
}
