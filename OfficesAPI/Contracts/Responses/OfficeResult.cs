using OfficesAPI.Models;

namespace OfficesAPI.Contracts.Responses
{
    public class OfficeResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public OfficeResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
