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
        public Task<List<GetDoctorsResponse>> GetAll();
        public Task<GetDoctorByDoctorResponse?> GetDoctorByDoctor(int id);
        public Task<CreateUpdateResponse> Create(string receptionistName, CreateDoctorRequest doctor);
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
                .Select(x => new GetDoctorByDoctorResponse
                {
                    Id = x.Id,
                    PhotoUrl = x.Account.PhotoUrl,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    MiddleName = x.MiddleName,
                    DateOfBirth = x.DateOfBirth,
                    Specialization = x.DoctorSpecialization.SpecializationName,
                    OfficeId = x.OfficeId,
                    OfficeAddress = x.OfficeAddress,
                    CareerStartYear = x.CareerStartYear
                })
                .FirstOrDefaultAsync(x => x.Id == id);
            if (doctor is null) return null;
            return doctor;
        }

        public async Task<List<GetDoctorsResponse>> GetAll()
        {
            var doctors = await _db.Doctors
                .Where(x => x.Status == Status.AtWork)
                .Include(x => x.DoctorSpecialization)
                .Include(x => x.Account)
                .Select(d => new GetDoctorsResponse
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

        public async Task<CreateUpdateResponse> Create(string creatorName, CreateDoctorRequest doctor)
        {
            var userExist = await _db.Accounts.FirstOrDefaultAsync(x => x.Email == doctor.Email);
            if (userExist != null) return new CreateUpdateResponse(false, "Someone already uses this email.");

            var office = await _officeService.GetById(doctor.OfficeId);
            if (office is null) return new CreateUpdateResponse(false, $"Wrong Office: {office}");

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
            return new CreateUpdateResponse(true, "Doctor created.");
        }
    }
}
