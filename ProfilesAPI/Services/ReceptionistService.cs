using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfilesAPI.Context;
using ProfilesAPI.Contracts.Requests;
using ProfilesAPI.Contracts.Responses;
using ProfilesAPI.Models;

namespace ProfilesAPI.Services
{
    public interface IReceptionistService
    {
        public Task<List<GetReceptionistResponse>> GetAll();
        public Task<GetReceptionistResponse?> GetById(int id);
        public Task<GeneralResponse> Create(string? creatorName, CreateReceptionistRequest receptionist);
        public Task<GeneralResponse> Update(string? updatorName, UpdateReceptionistRequest newReceptionist);
        public Task<GeneralResponse> Delete(int id);
    }

    public class ReceptionistService : IReceptionistService
    {
        private readonly ProfilesDbContext _db;
        private readonly IAccountService _accountService;
        private readonly IOfficeService _officeService;

        public ReceptionistService(ProfilesDbContext db, IAccountService accountService, IOfficeService officeService)
        {
            _db = db;
            _accountService = accountService;
            _officeService = officeService;
        }

        public async Task<List<GetReceptionistResponse>> GetAll()
        {
            return await _db.Receptionists.Include(r => r.Account).Select(r => r.ToResponse()).ToListAsync();
        }

        public async Task<GetReceptionistResponse?> GetById(int id)
        {
            var receptionist = await _db.Receptionists.Include(r => r.Account).FirstOrDefaultAsync(r => r.Id == id);
            if (receptionist == null) return null;
            return receptionist.ToResponse();
        }


        public async Task<GeneralResponse> Create(string creatorName, CreateReceptionistRequest receptionist)
        {
            var userExist = await _db.Accounts.FirstOrDefaultAsync(x => x.Email == receptionist.Email);
            if (userExist != null) return new GeneralResponse(false, "Someone already uses this email.");

            var office = await _officeService.GetById(receptionist.OfficeId);
            if (office is null) return new GeneralResponse(false, $"Wrong Office: {office}");

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
            return new GeneralResponse(true, "Receptionist created.");
        }

        public async Task<GeneralResponse> Update (string updatorName, UpdateReceptionistRequest newReceptionist)
        {
            var receptionist = await _db.Receptionists.Include(r => r.Account).FirstOrDefaultAsync(r => r.Id == newReceptionist.Id);
            if (receptionist == null) return new GeneralResponse(false, $"Receptionist with id {newReceptionist.Id} not found.");

            var office = await _officeService.GetById(receptionist.OfficeId);
            if (office is null) return new GeneralResponse(false, $"Wrong Office: {office}");

            receptionist.FirstName = newReceptionist.FirstName;
            receptionist.LastName = newReceptionist.LastName;
            receptionist.MiddleName = newReceptionist.MiddleName;
            receptionist.OfficeId = newReceptionist.OfficeId;
            receptionist.OfficeAddress = office.Address;
            receptionist.RegistryPhoneNumber = office.RegistryPhoneNumber;
            receptionist.Account.PhotoUrl = newReceptionist.PhotoUrl;
            receptionist.Account.UpdatedBy = updatorName ?? "Undefined";
            receptionist.Account.UpdatedAt = DateTime.Now;
            await _db.SaveChangesAsync();
            return new GeneralResponse(true, "Receptionist updated.");
        }

        // delete photo !
        public async Task<GeneralResponse> Delete (int id)
        {
            var receptionist = await _db.Receptionists.Include(r => r.Account).FirstOrDefaultAsync(r => r.Id == id);
            if (receptionist == null) return new GeneralResponse(false, $"Receptionist with id {id} not found.");
            _db.Accounts.Remove(receptionist.Account);
            _db.Receptionists.Remove(receptionist);
            await _db.SaveChangesAsync();
            return new GeneralResponse(true, "Receptionist deleted.");
        }
    }
}
