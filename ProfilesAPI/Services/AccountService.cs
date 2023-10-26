using Microsoft.EntityFrameworkCore;
using ProfilesAPI.Context;
using ProfilesAPI.Contracts.Responses;
using ProfilesAPI.Models;
using System.Security.Cryptography;
using System.Text;
using SharedModels;

namespace ProfilesAPI.Services
{
    public interface IAccountService
    {
        public Task<Account> Create(CreateAccountRequest account);
        public Task<Account> Create(string creatorName, string email, string? photoUrl, string phoneNumber);
        public Task<GeneralResponse> ConfirmEmail(int accountId);
    }

    public class AccountService : IAccountService
    {
        private readonly ProfilesDbContext _db;
        private readonly IEmailService _emailService;

        public AccountService(ProfilesDbContext db, IEmailService emailService)
        {
            _db = db;
            _emailService = emailService;
        }

        public async Task<GeneralResponse> ConfirmEmail(int accountId)
        {
            var account = await _db.Accounts.FindAsync(accountId);
            if (account is null) return new GeneralResponse(false, "Account not found.");
            account.IsEmailVerified = true;
            await _db.SaveChangesAsync();
            return new GeneralResponse(true, "Email conformed.");
        }

        public async Task<Account?> Create(CreateAccountRequest account)
        {
            var accountExist = await _db.Accounts.FirstOrDefaultAsync(a => a.Email == account.Email);
            if (accountExist != null) return null;

            var pwHash = Encoding.UTF8.GetString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(account.Password)));
            Account newAccount = new Account
            {
                UserId = account.UserId,
                Email = account.Email,
                PasswordHash = pwHash,
                PhoneNumber = account.PhoneNumber,
                IsEmailVerified = false,
                CreatedBy = "Owner",
                CreatedAt = DateTime.Now
            };
            await _db.Accounts.AddAsync(newAccount);
            await _db.SaveChangesAsync();
            await _emailService.SendConfirmationLink(newAccount.Email, newAccount.Id);
            return newAccount;
        }


        public async Task<Account> Create(string creatorName, string email, string? photoUrl, string phoneNumber)
        {
            var pw = GeneratePassword();
            var pwHash = Encoding.UTF8.GetString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(pw)));
            Account newAccount = new Account
            {
                Email = email,
                PasswordHash = pwHash,
                PhoneNumber = phoneNumber,
                IsEmailVerified = false,
                PhotoUrl = photoUrl,
                CreatedBy = creatorName ?? "Undefined",
                CreatedAt = DateTime.Now
            };
            await _emailService.SendCredentialsToEmail(email, pw);
            await _db.Accounts.AddAsync(newAccount);
            await _db.SaveChangesAsync();
            return newAccount;
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
