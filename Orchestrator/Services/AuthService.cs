using Newtonsoft.Json;
using Orchestrator.Contracts.Requests;
using Orchestrator.Contracts.Responses;
using System;
using System.Text;

namespace Orchestrator.Services
{
    public interface IAuthService
    {
        public Task<UserResponse?> CreateUser(RegisterRequest request);
        public Task<AccountResponse?> CreateAccount(RegisterRequest request, UserResponse user);
    }

    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<UserResponse?> CreateUser(RegisterRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            string url = $"http://localhost:5001/Auth/Register";
            var userReq = new CreateUserRequest
            {
                Email = request.Email,
                Password = request.Password,
                Role = request.Role
            };
            string userJsonReq = JsonConvert.SerializeObject(userReq);
            HttpContent userReqContent = new StringContent(userJsonReq, Encoding.UTF8, "application/json");
            HttpResponseMessage userResponse = await client.PostAsync(url, userReqContent);
            string userResponseBody = await userResponse.Content.ReadAsStringAsync();
            UserResponse? user = JsonConvert.DeserializeObject<UserResponse>(userResponseBody);
            return user;
        }

        public async Task<AccountResponse?> CreateAccount(RegisterRequest request, UserResponse user)
        {
            var client = _httpClientFactory.CreateClient();
            string url = $"http://localhost:5003/Account/Create";
            var accountReq = new CreateAccountRequest
            {
                UserId = user.Id,
                Email = request.Email,
                Password = request.Password,
                Role = request.Role,
                PhoneNumber = request.PhoneNumber
            };
            string accountJsonReq = JsonConvert.SerializeObject(accountReq);
            HttpContent accountReqContent = new StringContent(accountJsonReq, Encoding.UTF8, "application/json");
            HttpResponseMessage accountResponse = await client.PostAsync(url, accountReqContent);
            string accountResponseBody = await accountResponse.Content.ReadAsStringAsync();
            AccountResponse? account = JsonConvert.DeserializeObject<AccountResponse>(accountResponseBody);
            return account;
        }
    }
}
