using ProfilesAPI.Models;

namespace ProfilesAPI.Contracts.Responses
{
    public class CreatePatientResponse
    {
        public bool Success { get; set; }

        public Patient? FoundPatient { get; set; }

        public string Message { get; set; }
    }
}
