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
        public Task<CreateDoctorResponse> Create(string receptionistName, CreateDoctorRequest doctor);
    }

    public class DoctorService : IDoctorService
    {
        private readonly ProfilesDbContext _db;
        private readonly IEmailService _emailService;

        public DoctorService(ProfilesDbContext db, IEmailService emailService)
        {
            _db = db;
            _emailService = emailService;
        }

        public async Task<GetDoctorByDoctorResponse?> GetDoctorByDoctor(int id)
        {
            var doctor = await _db.Doctors
                .Include(x => x.Account)
                .Include(x => x.DoctorSpecialization)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (doctor == null) return null;
            var result = new GetDoctorByDoctorResponse
            {
                PhotoUrl = doctor.Account.PhotoUrl,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                MiddleName = doctor.MiddleName,
                DateOfBirth = doctor.DateOfBirth,
                Specialization = doctor.DoctorSpecialization.SpecializationName,
                OfficeId = doctor.OfficeId,
                OfficeAddress = doctor.OfficeAddress,
                CareerStartYear = doctor.CareerStartYear
            };
            return result;
        }

        public async Task<List<GetDoctorsResponse>> GetAll()
        {
            var doctors = await _db.Doctors
                .Where(x => x.Status == Status.AtWork)
                .Include(x => x.DoctorSpecialization)
                .Include(x => x.Account)
                .ToListAsync();

            var result = new List<GetDoctorsResponse>();
            foreach (var d in doctors)
            {
                result.Add(new GetDoctorsResponse
                {
                    PhotoUrl = d.Account.PhotoUrl,
                    FullName = $"{d.FirstName} {d.LastName} {d.MiddleName}",
                    Specialization = d.DoctorSpecialization.SpecializationName,
                    Experience = DateTime.Now.Year - d.CareerStartYear + 1,
                    OfficeAddress = d.OfficeAddress
                });
            }
            return result;
        }

        public async Task<CreateDoctorResponse> Create(string receptionistName, CreateDoctorRequest doctor)
        {
            var userExist = await _db.Accounts.FirstOrDefaultAsync(x => x.Email == doctor.Email);
            if (userExist != null) return new CreateDoctorResponse(false, "Someone already uses this email.");

            var pw = GeneratePassword();
            var pwHash = Encoding.UTF8.GetString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(pw)));

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"http://localhost:5002/Office/GetById/{doctor.OfficeId}");
            string responseBody = await response.Content.ReadAsStringAsync();
            dynamic office = JsonConvert.DeserializeObject(responseBody);
            if (office is null) return new CreateDoctorResponse(false, $"Wrong Office: {office}");

            Account newAccount = new Account()
            {
                Email = doctor.Email,
                PasswordHash = pwHash,
                PhoneNumber = "",
                IsEmailVerified = false,
                PhotoUrl = doctor.PhotoUrl,
                CreatedBy = receptionistName??"Undefined",
                CreatedAt = DateTime.Now
            };
            await _db.Accounts.AddAsync(newAccount);
            await _db.SaveChangesAsync();
            Doctor newDoctor = new Doctor()
            {
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                MiddleName = doctor.MiddleName,
                DateOfBirth = doctor.DateOfBirth,
                Account = newAccount,
                SpecializationId = doctor.SpecializationId,
                OfficeId = doctor.OfficeId,
                OfficeAddress = office.address,
                CareerStartYear = doctor.CareerStartYear,
                Status = doctor.Status,
                RegistryPhoneNumber = office.registryPhoneNumber
            };
            await _db.Doctors.AddAsync(newDoctor);
            await _db.SaveChangesAsync();
            await _emailService.SendCredentialsToEmail(doctor.Email, pw);
            return new CreateDoctorResponse(true, "Doctor created.");
        }

        //helper methods

        private string GeneratePassword()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var result = new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return result;
        }
    }
}
