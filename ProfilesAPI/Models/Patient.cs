using ProfilesAPI.Contracts.Responses;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfilesAPI.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        [NotMapped]
        public string FullName { get => $"{FirstName} {LastName} {MiddleName}"; }
        public bool IsLinkedToAccount { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int? AccountId { get; set; }
        public Account? Account { get; set; }


        public GetPatientResponse ToResponse()
        {
            return new GetPatientResponse
            {
                Id = Id,
                PhotoUrl = Account?.PhotoUrl,
                FirstName = FirstName,
                LastName = LastName,
                MiddleName = MiddleName,
                PhoneNumber = Account?.PhoneNumber,
                DateOfBirth = DateOfBirth
            };
        }
    }
}