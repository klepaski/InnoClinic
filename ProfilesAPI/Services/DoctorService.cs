using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfilesAPI.Contracts.Requests;
using ProfilesAPI.Contracts.Responses;
using ProfilesAPI.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace ProfilesAPI.Services
{
    public interface IDoctorService
    {
        public Task<List<GetDoctorResponse>> GetAll();
        public Task<GetDoctorByDoctorResponse?> GetDoctorByDoctor(int id);
        public Task<GetDoctorByPatientResponse?> GetDoctorByPatient(int id);
        public Task<GeneralResponse> Create(string creatorName, CreateDoctorRequest doctor);
        public Task<GeneralResponse> Update(string updatorName, UpdateDoctorRequest doctor);
        public Task<GeneralResponse> ChangeStatus(int id, string status);
    }

    public class DoctorService : IDoctorService
    {
        private readonly ProfilesDbContext _db;
        private readonly IAccountService _accountService;
        private readonly IGetOfficeService _officeService;

        public DoctorService(ProfilesDbContext db, IAccountService accountService, IGetOfficeService officeService)
        {
            _db = db;
            _accountService = accountService;
            _officeService = officeService;
        }

        public async Task<GetDoctorByDoctorResponse?> GetDoctorByDoctor(int id)
        {
            var doctor = await _db.Doctors
                .Include(x => x.Account)
                .Include(x => x.DoctorSpecialization)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (doctor is null) return null;
            return doctor.ToDoctorResponse();
        }

        public async Task<GetDoctorByPatientResponse?> GetDoctorByPatient(int id)
        {
            var doctor = await _db.Doctors
                .Include(x => x.DoctorSpecialization)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (doctor is null) return null;
            return doctor.ToPatientResponse();
        }

        public async Task<List<GetDoctorResponse>> GetAll()
        {
            var doctors = await _db.Doctors
                .Where(x => x.Status == Status.AtWork)
                .Include(x => x.DoctorSpecialization)
                .Include(x => x.Account)
                .Select(d => new GetDoctorResponse
                {
                    Id = d.Id,
                    PhotoUrl = d.Account.PhotoUrl,
                    FullName = $"{d.FirstName} {d.LastName} {d.MiddleName}",
                    Specialization = d.DoctorSpecialization.SpecializationName,
                    Experience = DateTime.Now.Year - d.CareerStartYear + 1,
                    OfficeAddress = d.OfficeAddress
                })
                .ToListAsync();
            return doctors;
        }

        public async Task<GeneralResponse> Create(string creatorName, CreateDoctorRequest doctor)
        {
            var emailExist = await _db.Accounts.FirstOrDefaultAsync(x => x.Email == doctor.Email);
            if (emailExist != null) return new GeneralResponse(false, "Someone already uses this email.");
            var office = await _officeService.GetById(doctor.OfficeId);
            if (office is null) return new GeneralResponse(false, $"Wrong Office: {office}");
            var newAccount = await _accountService.Create(creatorName, doctor.Email, doctor.PhotoUrl, office.RegistryPhoneNumber);
            Doctor newDoctor = new Doctor()
            {
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                MiddleName = doctor.MiddleName,
                DateOfBirth = doctor.DateOfBirth,
                Account = newAccount,
                SpecializationId = doctor.SpecializationId,
                OfficeId = doctor.OfficeId,
                OfficeAddress = office.Address,
                CareerStartYear = doctor.CareerStartYear,
                Status = doctor.Status,
                RegistryPhoneNumber = office.RegistryPhoneNumber
            };
            await _db.Doctors.AddAsync(newDoctor);
            await _db.SaveChangesAsync();
            return new GeneralResponse(true, "Doctor created.");
        }

        public async Task<GeneralResponse> Update(string updatorName, UpdateDoctorRequest newDoctor)
        {
            var doctor = await _db.Doctors.FindAsync(newDoctor.Id);
            if (doctor == null) return new GeneralResponse(false, $"Doctor with id {newDoctor.Id} not found.");
            var account = await _db.Accounts.FindAsync(doctor.AccountId);
            if (account == null) return new GeneralResponse(false, $"Account with id {doctor.AccountId} not found.");
            var office = _officeService.GetById(newDoctor.OfficeId);
            if (office == null) return new GeneralResponse(false, $"Office with id {newDoctor.Id} not found.");
            doctor.FirstName = newDoctor.FirstName;
            doctor.LastName = newDoctor.LastName;
            doctor.MiddleName = newDoctor.MiddleName;
            doctor.DateOfBirth = newDoctor.DateOfBirth;
            doctor.OfficeId = office.Id;
            doctor.SpecializationId = newDoctor.SpecializationId;
            doctor.CareerStartYear = newDoctor.CareerStartYear;
            doctor.Status = newDoctor.Status;
            account.PhotoUrl = newDoctor.PhotoUrl;
            account.UpdatedBy = updatorName ?? "Undefined";
            account.UpdatedAt = DateTime.Now;
            await _db.SaveChangesAsync();
            return new GeneralResponse(true, "Doctor updated.");
        }

        public async Task<GeneralResponse> ChangeStatus(int id, string status)
        {
            var doctor = await _db.Doctors.FindAsync(id);
            if (doctor == null)
                return new GeneralResponse(false, $"Doctor with id {id} not found.");
            if (Enum.TryParse(status, true, out Status statusValue))
                doctor.Status = statusValue;
            else return new GeneralResponse(false, "Invalid status.");
            await _db.SaveChangesAsync();
            return new GeneralResponse(true, "Status updated.");
        }
    }
}
