using Newtonsoft.Json;
using ProfilesAPI.Contracts.Responses;
using System.Net.Http;

namespace ProfilesAPI.Services
{
    public interface IGetOfficeService
    {
        public Task<GetOfficeResponse?> GetById(int id);
    }


    public class GetOfficeService : IGetOfficeService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GetOfficeService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GetOfficeResponse?> GetById(int id)
        {
            var client = _httpClientFactory.CreateClient();
            string url = $"http://localhost:5002/Office/GetById/{id}";
            HttpResponseMessage response = await client.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();
            GetOfficeResponse office = JsonConvert.DeserializeObject<GetOfficeResponse>(responseBody);
            //dynamic? office = JsonConvert.DeserializeObject(responseBody);
            if (office is null) return null;
            return office;
        }
    }
}
