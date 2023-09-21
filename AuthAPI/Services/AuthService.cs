using AuthAPI.Models;
using AuthAPI.Contracts.Requests;
using AuthAPI.Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthAPI.Services
{
    public interface IAuthService
    {
        Task<RegisterResponse> Register(RegisterRequest req);
        Task<LoginResponse> Login(LoginRequest req);
        Task<LoginResponse> Refresh(RefreshTokenRequest req);
        Task Delete(string email);
    }

    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly AuthDbContext _db;

        public AuthService(AuthDbContext db, ITokenService tokenService)
        {
            _db = db;
            _tokenService = tokenService;
        }

        public async Task<RegisterResponse> Register(RegisterRequest req)
        {
            var userExists = await _db.Users.FirstOrDefaultAsync(x => x.Email == req.Email);
            var pwHash = Encoding.UTF8.GetString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(req.Password)));
            if (userExists != null) return new RegisterResponse
            {
                Success = false,
                Message = "Someone already uses this email."
            };
            var newUser = new User()
            {
                Email = req.Email,
                PasswordHash = pwHash,
                Role = req?.Role ?? Role.Patient
            };
            await _db.Users.AddAsync(newUser);
            await _db.SaveChangesAsync();
            return new RegisterResponse
            {
                Success = true,
                NewUser = newUser
            };
        }

        public async Task<LoginResponse> Login(LoginRequest req)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == req.Email);
            if (user is null)
            {
                return new LoginResponse(false, "User with this email doesn't exist.");
            }
            var pwHash = Encoding.UTF8.GetString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(req.Password)));
            if (user.PasswordHash != pwHash)
            {
                return new LoginResponse(false, "Wrong password.");
            }
            var claims = new List<Claim>
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Email, req.Email),
                new Claim("Role", user.Role.ToString()),
            };
            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _db.SaveChangesAsync();
            return new LoginResponse(true, accessToken, refreshToken);
        }

        public async Task<LoginResponse> Refresh(RefreshTokenRequest req)
        {
            var principal = _tokenService.GetPrincipalFromToken(req.AccessToken);
            var userId = principal.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            var user = await _db.Users.FindAsync(Int32.Parse(userId));

            if (user is null || user.RefreshToken != req.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return new LoginResponse(false, "Invalid request.");
            }
            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _db.SaveChangesAsync();
            return new LoginResponse(true, newAccessToken, newRefreshToken);
        }

        public async Task Delete(string email)
        {
            var userToDelete = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (userToDelete != null) _db.Users.Remove(userToDelete);
            await _db.SaveChangesAsync();
        }
    }
}