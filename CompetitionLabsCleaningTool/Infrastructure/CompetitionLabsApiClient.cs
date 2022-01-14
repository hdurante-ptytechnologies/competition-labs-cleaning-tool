using CompetitionLabsCleaningTool.Core;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CompetitionLabsCleaningTool
{
    public class CompetitionLabsApiClient: IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfigurationRoot _configuration;

        public CompetitionLabsApiClient(HttpClient client, IConfigurationRoot configuration)
        {
            _httpClient = client;
            _configuration = configuration;
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void SetApiKey(string apiKey)
        {
            _httpClient.DefaultRequestHeaders.Remove("X-API-KEY");
            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
        }

        public async Task<bool> Delete(CompetiotionLabsSpace space, string id)
        {
            var uri = new Uri($"{_configuration["CompetitionLabsHttpClientSettings:baseUrl"]}/{space.Name}/products/{id}");
            var response = await _httpClient.DeleteAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<List<string>> Get(CompetiotionLabsSpace space, int limit, int skip)
        {
            var uri = new Uri($"{_configuration["CompetitionLabsHttpClientSettings:baseUrl"]}/{space.Name}/products?_limit={limit}&_skip={0}");
            var response = await _httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadAsStringAsync();
                JObject responceObject = JObject.Parse(res);
                JArray data = (JArray)responceObject["data"];
                List<string> idList = data.Select(c => (string)c["id"]).ToList();
                return idList;
            }

            return null;
        }

    }
}
