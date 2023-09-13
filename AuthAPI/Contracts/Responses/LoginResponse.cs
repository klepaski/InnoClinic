namespace AuthAPI.Contracts.Responses
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public string? Message { get; set; }

        public LoginResponse(bool success, string token, string refreshToken)
        {
            Success = success;
            Token = token;
            RefreshToken = refreshToken;
        }

        public LoginResponse(bool success, string message)
        {
            Success=success;
            Message = message;
        }
    }
}
