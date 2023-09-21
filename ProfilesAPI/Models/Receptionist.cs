using ProfilesAPI.Contracts.Responses;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfilesAPI.Models
{
    public class Receptionist
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        [NotMapped]
        public string FullName { get => $"{FirstName} {LastName} {MiddleName}"; }

        public int AccountId { get; set; }
        public Account Account { get; set; }

        public int OfficeId { get; set; }

        //redundancy
        public string OfficeAddress { get; set; }
        public string RegistryPhoneNumber { get; set; }


        public GetReceptionistResponse ToResponse()
        {
            return new GetReceptionistResponse
            {
                Id = Id,
                PhotoUrl = Account.PhotoUrl,
                FirstName = FirstName,
                LastName = LastName,
                MiddleName = MiddleName,
                OfficeId = OfficeId,
                OfficeAddress = OfficeAddress
            };
        }
    }
}
