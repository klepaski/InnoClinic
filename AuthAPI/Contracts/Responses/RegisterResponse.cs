namespace AuthAPI.Contracts.Responses
{
    public class RegisterResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public RegisterResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
