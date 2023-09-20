using ProfilesAPI.Contracts.Responses;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfilesAPI.Models
{
    public enum Status
    {
        AtWork,
        OnVacation,
        SickDay,
        SickLeave,
        SelfIsolation,
        LeaveWithoutPay,
        Inactive
    }

    public class Doctor
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        [NotMapped]
        public string FullName { get => $"{FirstName} {LastName} {MiddleName}"; }
        public DateTime DateOfBirth { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }

        public int SpecializationId { get; set; }
        public DoctorSpecialization DoctorSpecialization { get; set; }

        public int OfficeId { get; set; }

        public int CareerStartYear { get; set; }
        public Status Status { get; set; }

        //redundancy
        public string OfficeAddress { get; set; }


        public GetDoctorResponse ToResponse()
        {
            return new GetDoctorResponse
            {
                Id = Id,
                PhotoUrl = Account.PhotoUrl,
                FirstName = FirstName,
                LastName = LastName,
                MiddleName = MiddleName,
                DateOfBirth = DateOfBirth,
                CareerStartYear = CareerStartYear,
                Experience = DateTime.Now.Year - CareerStartYear + 1,
                Specialization = DoctorSpecialization.SpecializationName,
                OfficeId = OfficeId,
                OfficeAddress = OfficeAddress,
                Status = this.Status.ToString()
            };
        }
    }
}
