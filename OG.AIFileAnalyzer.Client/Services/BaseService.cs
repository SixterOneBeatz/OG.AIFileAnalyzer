using System.Net.Http.Json;

namespace OG.AIFileAnalyzer.Client.Services
{
    public abstract class BaseService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<T>();
            }
            else
            {
                return default;
            }
        }

        public async Task<T> PostAsync<T, U>(string endpoint, U body)
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, body);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<T>();
            }
            else
            {
                return default;
            }
        }

        public async Task PostAsync<U>(string endpoint, U body)
        {
            await _httpClient.PostAsJsonAsync(endpoint, body);
        }

        public async Task<Stream> GetAsync(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStreamAsync();
            }
            else
            {
                return default;
            }
        }
    }
}
