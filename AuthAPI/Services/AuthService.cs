using AuthAPI.Models;
using AuthAPI.Contracts.Requests;
using AuthAPI.Contracts.Responses;
//using HotelingLibrary.Messages;
//using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthAPI.Services
{
    public interface IAuthService
    {
        Task<RegistrationResponse> Register(RegisterRequest request);
        Task<AuthenticationResponse> Login(LoginRequest request);
        Task<AuthenticationResponse> Refresh(RefreshTokenRequest request);
        Task<string> GetRole(string userToken);
        Task ConsumeUserChangedMessage(ConsumeContext<UserDataChangedMessage> consumeContext);
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

        //email password Role
        public async Task<RegistrationResponse> Register(RegisterRequest req)
        {
            var userExists = await _db.Users.FirstOrDefaultAsync(x => x.Email == req.Email);
            if (userExists != null)
            {
                return new RegistrationResponse(false, "Someone already uses this email.");
            }
            var pwHash = Encoding.UTF8.GetString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(req.Password)));
            var refreshToken = _tokenService.GenerateRefreshToken();

            var newUser = new User()
            {
                Email = req.Email,
                PasswordHash = pwHash,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = DateTime.Now.AddDays(7),
                Role = req.Role //req?.Role ?? Role.User
            };

            await _db.Users.AddAsync(newUser);
            await _db.SaveChangesAsync();
            return new RegistrationResponse(true, "User created.");
        }

        //email password
        public async Task<AuthenticationResponse> Login(LoginRequest req)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == req.Email);
            if (user is null)
                return new AuthenticationResponse
                {
                    Success = false,
                    ErrorMessage = "User with this email doesn't exist."
                };

            var pwHash = Encoding.UTF8.GetString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(req.Password)));
            
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
            return new AuthenticationResponse
            {
                Success = true,
                Token = accessToken,
                RefreshToken = refreshToken
            };
        }

        //access refresh
        public async Task<AuthenticationResponse> Refresh(RefreshTokenRequest req)
        {
            string accessToken = req.AccessToken;
            string refreshToken = req.RefreshToken;
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var email = principal.Identity.Name;

            var user = await _db.Users.SingleOrDefaultAsync(x => x.Email == email);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return new AuthenticationResponse()
                {
                    Success = false,
                    ErrorMessage = "Invalid client request"
                };
            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            await _db.SaveChangesAsync();

            return new AuthenticationResponse()
            {
                Success = true,
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task<string> GetRole(string userToken)
        {
            return _tokenService.GetRole(userToken);
        }

        public async Task ConsumeUserChangedMessage(ConsumeContext<UserDataChangedMessage> consumeContext)
        {
            var user = _db.Users.First(x => x.Id == consumeContext.Message.EntityId);
            user.Email = consumeContext.Message.Email;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }
    }
}