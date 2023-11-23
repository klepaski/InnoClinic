using Newtonsoft.Json;
using System.Text;
using SharedModels;

namespace Orchestrator.Services
{
    public interface IAuthService
    {
        public Task<User?> CreateUser(RegisterRequest req);
        public Task<bool> CreateAccount(User user, RegisterRequest req);
    }

    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<User?> CreateUser(RegisterRequest req)
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                string userUrl = $"{Ports.GatewayAPI}/Auth/Register";
                var userReq = new CreateUserRequest(req.Email, req.Password, req.Role);
                string userJsonReq = JsonConvert.SerializeObject(userReq);
                HttpContent userReqContent = new StringContent(userJsonReq, Encoding.UTF8, "application/json");
                HttpResponseMessage userResponse = await client.PostAsync(userUrl, userReqContent);
                if (!userResponse.IsSuccessStatusCode) return null;
                string userResponseBody = await userResponse.Content.ReadAsStringAsync();
                User? user = JsonConvert.DeserializeObject<User>(userResponseBody);
                return user;
            }
        }

        public async Task<bool> CreateAccount(User user, RegisterRequest req)
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                try
                {
                    string accountUrl = $"{Ports.ProfilesAPI}/Account/Create";
                    var accountReq = new CreateAccountRequest(user.Id, req.Email, req.Password, req.Role, req.PhoneNumber);
                    string accountJsonReq = JsonConvert.SerializeObject(accountReq);
                    HttpContent accountReqContent = new StringContent(accountJsonReq, Encoding.UTF8, "application/json");
                    HttpResponseMessage accountResponse = await client.PostAsync(accountUrl, accountReqContent);
                    accountResponse.EnsureSuccessStatusCode();
                    return true;
                }
                catch (Exception ex)
                {
                    string deleteUserUrl = $"{Ports.GatewayAPI}/Auth/Delete/{req.Email}";
                    await client.DeleteAsync(deleteUserUrl);
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }
    }
}
