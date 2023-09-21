﻿using Newtonsoft.Json;
using ProfilesAPI.Contracts.Responses;
using System.Net.Http;

namespace ProfilesAPI.Services
{
    public interface IOfficeService
    {
        public Task<GetOfficeResponse?> GetById(int id);
    }


    public class OfficeService : IOfficeService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OfficeService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GetOfficeResponse?> GetById(int id)
        {
            var client = _httpClientFactory.CreateClient();
            string url = $"http://localhost:5002/Office/GetById/{id}";
            HttpResponseMessage response = await client.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();
            GetOfficeResponse? office = JsonConvert.DeserializeObject<GetOfficeResponse>(responseBody);
            //dynamic? office = JsonConvert.DeserializeObject(responseBody);
            return office;
        }
    }
}
