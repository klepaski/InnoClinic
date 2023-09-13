namespace AuthAPI.Contracts.Responses
{
    public class RegistrationResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public RegistrationResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
