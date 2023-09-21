using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfilesAPI.Contracts.Requests;
using ProfilesAPI.Contracts.Responses;
using ProfilesAPI.Models;
using ProfilesAPI.Context;

namespace ProfilesAPI.Services
{
    public interface IDoctorService
    {
        public Task<List<GetDoctorResponse>> GetAll();
        public Task<GetDoctorResponse?> GetById(int id);
        public Task<GeneralResponse> Create(string creatorName, CreateDoctorRequest doctor);
        public Task<GeneralResponse> Update(string updatorName, UpdateDoctorRequest doctor);
        public Task<GeneralResponse> ChangeStatus(int id, string status);
        public Task<List<Doctor>> Search(string name);
        public Task<List<Doctor>> FilterBySpecialization(int specializationId);
        public Task<List<Doctor>> FilterByOffice(int officeId);
    }

    public class DoctorService : IDoctorService
    {
        private readonly ProfilesDbContext _db;
        private readonly IAccountService _accountService;
        private readonly IOfficeService _officeService;

        public DoctorService(ProfilesDbContext db, IAccountService accountService, IOfficeService officeService)
        {
            _db = db;
            _accountService = accountService;
            _officeService = officeService;
        }

        public async Task<GetDoctorResponse?> GetById(int id)
        {
            var doctor = await _db.Doctors
                .Include(x => x.Account)
                .Include(x => x.DoctorSpecialization)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (doctor is null) return null;
            return doctor.ToResponse();
        }

        public async Task<List<GetDoctorResponse>> GetAll()
        {
            return await _db.Doctors
                .Where(x => x.Status == Status.AtWork)
                .Include(x => x.DoctorSpecialization)
                .Include(x => x.Account)
                .Select(d => d.ToResponse())
                .ToListAsync();
        }

        public async Task<GeneralResponse> Create(string creatorName, CreateDoctorRequest req)
        {
            var emailExist = await _db.Accounts.FirstOrDefaultAsync(x => x.Email == req.Email);
            if (emailExist != null) return new GeneralResponse(false, "Someone already uses this email.");

            var office = await _officeService.GetById(req.OfficeId);
            if (office is null) return new GeneralResponse(false, $"Office with id {req.OfficeId} not found.");

            var newAccount = await _accountService.Create(creatorName, req.Email, req.PhotoUrl, office.RegistryPhoneNumber);
            Doctor newDoctor = new Doctor()
            {
                FirstName = req.FirstName,
                LastName = req.LastName,
                MiddleName = req.MiddleName,
                DateOfBirth = req.DateOfBirth,
                Account = newAccount,
                SpecializationId = req.SpecializationId,
                OfficeId = req.OfficeId,
                OfficeAddress = office.Address,
                CareerStartYear = req.CareerStartYear,
                Status = req.Status
            };
            await _db.Doctors.AddAsync(newDoctor);
            await _db.SaveChangesAsync();
            return new GeneralResponse(true, "Doctor created.");
        }

        public async Task<GeneralResponse> Update(string updatorName, UpdateDoctorRequest req)
        {
            var doctor = await _db.Doctors.Include(d => d.Account).FirstOrDefaultAsync(d => d.Id == req.Id);
            if (doctor == null) return new GeneralResponse(false, $"Doctor with id {req.Id} not found.");

            var office = await _officeService.GetById(req.OfficeId);
            if (office == null) return new GeneralResponse(false, $"Office with id {req.OfficeId} not found.");

            doctor.FirstName = req.FirstName;
            doctor.LastName = req.LastName;
            doctor.MiddleName = req.MiddleName;
            doctor.DateOfBirth = req.DateOfBirth;
            doctor.OfficeId = req.OfficeId;
            doctor.OfficeAddress = office.Address;
            doctor.SpecializationId = req.SpecializationId;
            doctor.CareerStartYear = req.CareerStartYear;
            doctor.Status = req.Status;
            doctor.Account.PhotoUrl = req.PhotoUrl;
            doctor.Account.UpdatedBy = updatorName ?? "Undefined";
            doctor.Account.UpdatedAt = DateTime.Now;
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

        public async Task<List<Doctor>> Search(string name)
        {
            return await _db.Doctors
                .Where(d => (d.FirstName + " " + d.LastName + " " + d.MiddleName).ToLower().Contains(name.ToLower()))
                .ToListAsync();
        }

        public async Task<List<Doctor>> FilterBySpecialization(int specializationId)
        {
            return await _db.Doctors
                .Where(d => d.SpecializationId == specializationId)
                .ToListAsync();
        }

        public async Task<List<Doctor>> FilterByOffice(int officeId)
        {
            return await _db.Doctors
                .Where(d => d.OfficeId == officeId)
                .ToListAsync();
        }
    }
}
