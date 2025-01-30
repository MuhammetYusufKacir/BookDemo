using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using BookDemo.Core.Entities;
using BookDemo.Core.Models;

namespace BookDemo.Application.Services
{
    public class ExternalApiService
    {
        private readonly HttpClient _httpClient;

        public ExternalApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T>GetDataAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<TResponse> PostDataAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        public async Task<CartUpdateResponse> UpdateSoldStatusAsync(Cart cart)
        {
           
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7193/api/cartsale/update-sold", cart);

           // response.EnsureSuccessStatusCode();
            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                throw new Exception($"API Hatası: {response.StatusCode} - {errorMsg}");
            }

            var result = await response.Content.ReadFromJsonAsync<CartUpdateResponse>();
            return result;
        }
    }
}
