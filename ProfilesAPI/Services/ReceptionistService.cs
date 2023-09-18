using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfilesAPI.Contracts.Requests;
using ProfilesAPI.Contracts.Responses;
using ProfilesAPI.Models;

namespace ProfilesAPI.Services
{
    public interface IReceptionistService
    {
        public Task<List<GetReceptionistsResponse>> GetAll();
        public Task<GetReceptionistResponse?> GetById(int id);
        public Task<CreateUpdateResponse> Create(string? creatorName, CreateReceptionistRequest receptionist);
        public Task<CreateUpdateResponse> Update(string? updatorName, UpdateReceptionistRequest newReceptionist);
        public Task<CreateUpdateResponse> Delete(int id);
    }

    public class ReceptionistService : IReceptionistService
    {
        private readonly ProfilesDbContext _db;
        private readonly IAccountService _accountService;
        private readonly IGetOfficeService _officeService;

        public ReceptionistService(ProfilesDbContext db, IAccountService accountService, IGetOfficeService officeService)
        {
            _db = db;
            _accountService = accountService;
            _officeService = officeService;
        }

        public async Task<List<GetReceptionistsResponse>> GetAll()
        {
            var receptionists = await _db.Receptionists.Select(r => new GetReceptionistsResponse
            {
                Id = r.Id,
                Fullname = $"{r.FirstName} {r.LastName} {r.MiddleName}",
                OfficeAddress = r.OfficeAddress
            }).ToListAsync();
            return receptionists;
        }

        public async Task<GetReceptionistResponse?> GetById(int id)
        {
            var receptionist = await _db.Receptionists.Include(r => r.Account).FirstOrDefaultAsync(r => r.Id == id);
            if (receptionist == null) return null;
            var result = new GetReceptionistResponse
            {
                Id = receptionist.Id,
                PhotoUrl = receptionist.Account.PhotoUrl,
                FirstName = receptionist.FirstName,
                LastName = receptionist.LastName,
                MiddleName = receptionist.MiddleName,
                OfficeId = receptionist.OfficeId,
                OfficeAddress = receptionist.OfficeAddress
            };
            return result;
        }


        public async Task<CreateUpdateResponse> Create(string creatorName, CreateReceptionistRequest receptionist)
        {
            var userExist = await _db.Accounts.FirstOrDefaultAsync(x => x.Email == receptionist.Email);
            if (userExist != null) return new CreateUpdateResponse(false, "Someone already uses this email.");

            var office = await _officeService.GetById(receptionist.OfficeId);
            if (office is null) return new CreateUpdateResponse(false, $"Wrong Office: {office}");

            var newAccount = await _accountService.Create(creatorName, receptionist.Email, receptionist.PhotoUrl, office.RegistryPhoneNumber);

            Receptionist newReceptionist = new Receptionist
            {
                FirstName = receptionist.FirstName,
                LastName = receptionist.LastName,
                MiddleName = receptionist.MiddleName,
                Account = newAccount,
                OfficeId = receptionist.OfficeId,
                OfficeAddress = office.Address,
                RegistryPhoneNumber = office.RegistryPhoneNumber
            };
            await _db.Receptionists.AddAsync(newReceptionist);
            await _db.SaveChangesAsync();
            return new CreateUpdateResponse(true, "Receptionist created.");
        }

        public async Task<CreateUpdateResponse> Update (string updatorName, UpdateReceptionistRequest newReceptionist)
        {
            var receptionist = await _db.Receptionists.FindAsync(newReceptionist.Id);
            if (receptionist == null) return new CreateUpdateResponse(false, $"Receptionist with id {newReceptionist.Id} not found.");

            var account = await _db.Accounts.FindAsync(receptionist.AccountId);
            if (account == null) return new CreateUpdateResponse(false, $"Account with id {receptionist.AccountId} not found.");

            var office = _officeService.GetById(newReceptionist.OfficeId);
            if (office == null) return new CreateUpdateResponse(false, $"Office with id {newReceptionist.Id} not found.");

            receptionist.FirstName = newReceptionist.FirstName;
            receptionist.LastName = newReceptionist.LastName;
            receptionist.MiddleName = newReceptionist.MiddleName;
            receptionist.OfficeId = office.Id;

            account.PhotoUrl = newReceptionist.PhotoUrl;
            account.UpdatedBy = updatorName ?? "Undefined";
            account.UpdatedAt = DateTime.Now;
            await _db.SaveChangesAsync();
            return new CreateUpdateResponse(true, "Receptionist updated.");
        }

        //удалить фото из БД !!!
        public async Task<CreateUpdateResponse> Delete (int id)
        {
            var receptionist = await _db.Receptionists.FindAsync(id);
            if (receptionist == null) return new CreateUpdateResponse(false, $"Receptionist with id {id} not found.");

            var account = await _db.Accounts.FindAsync(receptionist.AccountId);
            if (account == null) return new CreateUpdateResponse(false, $"Account with id {receptionist.AccountId} not found.");

            _db.Receptionists.Remove(receptionist);
            _db.Accounts.Remove(account);
            await _db.SaveChangesAsync();
            return new CreateUpdateResponse(true, "Receptionist deleted.");
        }
    }
}
