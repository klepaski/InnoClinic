using Azure.Core;
using Microsoft.EntityFrameworkCore;
using ProfilesAPI.Contracts.Requests;
using ProfilesAPI.Contracts.Responses;
using ProfilesAPI.Models;

namespace ProfilesAPI.Services
{
    public interface IPatientService
    {
        public Task<GetPatientResponse?> GetById(int id);
        public Task<List<GetAllPatientsResponse>> GetAll();
        public Task<GeneralResponse> CreateByAdmin(CreatePatientByAdminRequest patient);
        public Task<CreatePatientResponse> Create(CreatePatientRequest patient);
        public Task<CreatePatientResponse> CreatePatientWithNewAccount(CreatePatientRequest patient);
        public Task<CreatePatientResponse> LinkPatientToAccount(Patient patient, Account account);
        public Task<GeneralResponse> Delete(int id);
        public Task<GeneralResponse> Update(string updatorName, UpdatePatientRequest newPatient);
    }

    public class PatientService : IPatientService
    {
        private readonly ProfilesDbContext _db;
        private readonly IAccountService _accountService;

        public PatientService(ProfilesDbContext db, IAccountService accountService)
        {
            _db = db;
            _accountService = accountService;
        }

        // добавить вывод Appointments пациента (для доктора и пациента)!!

        public async Task<GetPatientResponse?> GetById(int id)
        {
            var patient = await _db.Patients.Include(p => p.Account).FirstOrDefaultAsync(p => p.Id == id);
            if (patient == null) return null;
            var result = new GetPatientResponse
            {
                Id = patient.Id,
                PhotoUrl = patient.Account.PhotoUrl,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                MiddleName = patient.MiddleName,
                PhoneNumber = patient.Account.PhoneNumber,
                DateOfBirth = patient.DateOfBirth
            };
            return result;
        }

        public async Task<List<GetAllPatientsResponse>> GetAll()
        {
            return await _db.Patients
                .Select(p => new GetAllPatientsResponse
                {
                    Id = p.Id,
                    FullName = $"{p.FirstName} {p.LastName} {p.MiddleName}",
                    PhoneNumber = p.Account.PhoneNumber
                }).ToListAsync();
        }

        public async Task<GeneralResponse> CreateByAdmin(CreatePatientByAdminRequest patient)
        {
            var newPatient = new Patient
            {
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                MiddleName = patient.MiddleName,
                DateOfBirth = patient.DateOfBirth,
                IsLinkedToAccount = false
            };
            await _db.AddAsync(newPatient);
            await _db.SaveChangesAsync();
            return new GeneralResponse(true, "Patient created.");
        }

        public async Task<CreatePatientResponse> Create(CreatePatientRequest patient)
        {
            int matchCoefficient = 0;
            var matchingPatients = await _db.Patients
                .Include(p => p.Account)
                .Where(p =>
                    (p.FirstName == patient.FirstName ||
                    p.LastName == patient.LastName ||
                    p.MiddleName == patient.MiddleName ||
                    p.DateOfBirth == patient.DateOfBirth) &&
                    p.IsLinkedToAccount == false
                ).ToListAsync();

            foreach (var p in matchingPatients)
            {
                matchCoefficient += 5 * (p.FirstName == patient.FirstName ? 1 : 0);
                matchCoefficient += 5 * (p.LastName == patient.LastName ? 1 : 0);
                matchCoefficient += 5 * (p.MiddleName == patient.MiddleName ? 1 : 0);
                matchCoefficient += 3 * (p.DateOfBirth == patient.DateOfBirth ? 1 : 0);
                if (matchCoefficient >= 13)
                    return new CreatePatientResponse
                    {
                        Success = false,
                        FoundPatient = p
                    };
            }
            return await CreatePatientWithNewAccount(patient);
        }

        public async Task<CreatePatientResponse> CreatePatientWithNewAccount(CreatePatientRequest patient)
        {
            var creatorName = $"{patient.FirstName} {patient.LastName} {patient.MiddleName}";
            var newAccount = await _accountService.Create(creatorName, patient.Email, patient.PhotoUrl, patient.PhoneNumber);
            var newPatient = new Patient
            {
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                MiddleName = patient.MiddleName,
                DateOfBirth = patient.DateOfBirth,
                IsLinkedToAccount = true,
                AccountId = newAccount.Id,
                Account = newAccount
            };
            await _db.Patients.AddAsync(newPatient);
            await _db.SaveChangesAsync();
            return new CreatePatientResponse
            {
                Success = true,
                Message = "Patient created."
            };
        }

        public async Task<CreatePatientResponse> LinkPatientToAccount(Patient patient, Account account)
        {
            patient.IsLinkedToAccount = true;
            patient.AccountId = account.Id;
            patient.Account = account;
            await _db.SaveChangesAsync();
            return new CreatePatientResponse
            {
                Success = true,
                Message = "Patient is linked to the account."
            };
        }

        public async Task<GeneralResponse> Delete(int id)
        {
            var patient = await _db.Patients.FindAsync(id);
            if (patient == null) return new GeneralResponse(false, $"Patient with id {id} not found.");
            _db.Patients.Remove(patient);
            await _db.SaveChangesAsync();
            return new GeneralResponse(true, "User deleted.");
        }

        public async Task<GeneralResponse> Update(string updatorName, UpdatePatientRequest newPatient)
        {
            var patient = await _db.Patients.FindAsync(newPatient.Id);
            if (patient == null) return new GeneralResponse(false, $"Patient with id {newPatient.Id} not found.");
            patient.FirstName = newPatient.FirstName;
            patient.LastName = newPatient.LastName;
            patient.MiddleName = newPatient.MiddleName;
            patient.DateOfBirth = newPatient.DateOfBirth;
            if (patient.Account != null)
            {
                patient.Account.PhoneNumber = newPatient.PhoneNumber;
                patient.Account.PhotoUrl = newPatient.PhotoUrl;
                patient.Account.UpdatedBy = updatorName ?? "Undefined";
                patient.Account.UpdatedAt = DateTime.Now;
            }
            await _db.SaveChangesAsync();
            return new GeneralResponse(true, "Patient updated.");
        }
    }
}
