
namespace AspNetSixExample.Services
{
    public class WebApiWeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;

        public WebApiWeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecastAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "WeatherForecast");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>();
            }

            throw new Exception("Problem retrieving weather forecasts");
        }
    }
}
