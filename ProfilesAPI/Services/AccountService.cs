using ProfilesAPI.Models;
using System.Security.Cryptography;
using System.Text;

namespace ProfilesAPI.Services
{
    public interface IAccountService
    {
        public Task<Account> Create(string receptionistName, string email, string? photoUrl, string phoneNumber);
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

        public async Task<Account> Create(string creatorName, string email, string? photoUrl, string phoneNumber)
        {
            var pw = GeneratePassword();
            var pwHash = Encoding.UTF8.GetString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(pw)));
            Account newAccount = new Account()
            {
                Email = email,
                PasswordHash = pwHash,
                IsEmailVerified = false,
                PhotoUrl = photoUrl,
                PhoneNumber = phoneNumber,
                CreatedBy = creatorName??"Undefined",
                CreatedAt = DateTime.Now
            };
            await _db.Accounts.AddAsync(newAccount);
            await _db.SaveChangesAsync();
            await _emailService.SendCredentialsToEmail(email, pw);
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
