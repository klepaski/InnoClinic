using ProfilesAPI.Contracts.Responses;

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
        public string MiddleName { get; set; }
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
        public string RegistryPhoneNumber { get; set; }


        public GetDoctorByDoctorResponse ToDoctorResponse()
        {
            return new GetDoctorByDoctorResponse
            {
                Id = this.Id,
                PhotoUrl = this.Account.PhotoUrl,
                FirstName = this.FirstName,
                LastName = this.LastName,
                MiddleName = this.MiddleName,
                DateOfBirth = this.DateOfBirth,
                Specialization = this.DoctorSpecialization.SpecializationName,
                OfficeId = this.OfficeId,
                OfficeAddress = this.OfficeAddress,
                CareerStartYear = this.CareerStartYear
            };
        }

        public GetDoctorByPatientResponse ToPatientResponse()
        {
            return new GetDoctorByPatientResponse
            {
                Id = this.Id,
                FullName = $"{this.FirstName} {this.LastName} {this.MiddleName}",
                OfficeAddress = this.OfficeAddress,
                Experience = DateTime.Now.Year - this.CareerStartYear + 1,
                Specialization = this.DoctorSpecialization.SpecializationName,
                Services = this.DoctorSpecialization.Services
            };
        }
    }
}
