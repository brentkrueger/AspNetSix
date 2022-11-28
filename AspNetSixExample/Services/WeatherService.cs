using Microsoft.AspNetCore.Server.IIS;
using System.IO;
using System.Net.Http.Headers;

namespace AspNetSixExample.Services
{
    public class WebApiWeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;

        public WebApiWeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherForecast> GetWeatherAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "WeatherForecast");
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<WeatherForecast>? weatherForecast = await response.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>();

                return weatherForecast.First();
            }

            throw new Exception("Problem retrieving weather summary");
        }
    }
}
