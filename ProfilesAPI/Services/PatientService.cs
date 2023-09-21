using Azure.Core;
using Microsoft.EntityFrameworkCore;
using ProfilesAPI.Context;
using ProfilesAPI.Contracts.Requests;
using ProfilesAPI.Contracts.Responses;
using ProfilesAPI.Models;

namespace ProfilesAPI.Services
{
    public interface IPatientService
    {
        public Task<GetPatientResponse?> GetById(int id);
        public Task<List<GetPatientResponse>> GetAll();
        public Task<GeneralResponse> CreateByAdmin(CreatePatientByAdminRequest patient);
        public Task<CreatePatientResponse> Create(int accountId, CreatePatientRequest patient);
        public Task<CreatePatientResponse> CreatePatientForAccount(int accountId, CreatePatientRequest patient);
        public Task<GeneralResponse> LinkPatientToAccount(int accountId, int patientId);
        public Task<GeneralResponse> Delete(int id);
        public Task<GeneralResponse> Update(string updatorName, UpdatePatientRequest newPatient);
        public Task<List<Patient>> Search(string name);
    }

    public class PatientService : IPatientService
    {
        private readonly ProfilesDbContext _db;

        public PatientService(ProfilesDbContext db)
        {
            _db = db;
        }

        // добавить вывод Appointments пациента (для доктора и пациента)!!
        public async Task<GetPatientResponse?> GetById(int id)
        {
            var patient = await _db.Patients.Include(p => p.Account).FirstOrDefaultAsync(p => p.Id == id);
            if (patient == null) return null;
            return patient.ToResponse();
        }

        public async Task<List<GetPatientResponse>> GetAll()
        {
            return await _db.Patients.Include(p => p.Account).Select(p => p.ToResponse()).ToListAsync();
        }

        //without account
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

        public async Task<CreatePatientResponse> Create(int accountId, CreatePatientRequest patient)
        {
            var account = await _db.Accounts.FindAsync(accountId);
            if (account != null) account.IsEmailVerified = true;
            await _db.SaveChangesAsync();

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
            return await CreatePatientForAccount(accountId, patient);
        }

        public async Task<CreatePatientResponse> CreatePatientForAccount(int accountId, CreatePatientRequest patient)
        {
            var account = await _db.Accounts.FindAsync(accountId);
            if (account is null) return new CreatePatientResponse
            {
                Success = false,
                Message = $"Account with id {accountId} not found."
            };
            account.PhoneNumber = patient.PhoneNumber;
            account.PhotoUrl = patient.PhotoUrl;

            var newPatient = new Patient
            {
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                MiddleName = patient.MiddleName,
                DateOfBirth = patient.DateOfBirth,
                IsLinkedToAccount = true,
                AccountId = accountId
            };
            await _db.Patients.AddAsync(newPatient);
            await _db.SaveChangesAsync();
            return new CreatePatientResponse
            {
                Success = true,
                Message = "Patient created."
            };
        }

        public async Task<GeneralResponse> LinkPatientToAccount(int accountId, int patientId)
        {
            var patient = await _db.Patients.FindAsync(patientId);
            if (patient is null) return new GeneralResponse(false, $"Patient with id {patientId} not found.");
            patient.IsLinkedToAccount = true;
            patient.AccountId = accountId;
            await _db.SaveChangesAsync();
            return new GeneralResponse(true, "Patient is linked to the account.");
        }

        //delete photo, appointments, results !
        public async Task<GeneralResponse> Delete(int id)
        {
            var patient = await _db.Patients.FindAsync(id);
            if (patient == null) return new GeneralResponse(false, $"Patient with id {id} not found.");
            if (patient.IsLinkedToAccount)
            {
                var account = await _db.Accounts.FindAsync(patient.AccountId);
                if (account != null) _db.Accounts.Remove(account);
            }
            _db.Patients.Remove(patient);
            await _db.SaveChangesAsync();
            return new GeneralResponse(true, "User deleted.");
        }


        // Что делать, если у пациента нет аккаунта?? Значения PhoneNumber и PhotoUrl хранятся в Account, а не в Patient
        // Создать ему новый акк?? Тогда что указать в email??
        public async Task<GeneralResponse> Update(string updatorName, UpdatePatientRequest newPatient)
        {
            var patient = await _db.Patients.Include(p => p.Account).FirstOrDefaultAsync(p => p.Id == newPatient.Id);
            if (patient == null) return new GeneralResponse(false, $"Patient with id {newPatient.Id} not found.");
            patient.FirstName = newPatient.FirstName;
            patient.LastName = newPatient.LastName;
            patient.MiddleName = newPatient.MiddleName;
            patient.DateOfBirth = newPatient.DateOfBirth;
            if (patient.IsLinkedToAccount)
            {
                patient.Account.PhoneNumber = newPatient.PhoneNumber;
                patient.Account.PhotoUrl = newPatient.PhotoUrl;
                patient.Account.UpdatedBy = updatorName ?? "Undefined";
                patient.Account.UpdatedAt = DateTime.Now;
            }
            await _db.SaveChangesAsync();
            return new GeneralResponse(true, "Patient updated.");
        }

        public async Task<List<Patient>> Search(string name)
        {
            return await _db.Patients
                .Where(p => (p.FirstName + " " + p.LastName + " " + p.MiddleName).ToLower().Contains(name.ToLower()))
                //.FromSqlInterpolated($"SELECT * FROM Patients WHERE CONCAT(FirstName, ' ', LastName, ' ', COALESCE(MiddleName, '')) LIKE '%{name}%'")
                .ToListAsync();
        }
    }
}
