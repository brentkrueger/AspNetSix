
namespace WeatherAspNet.Services
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
            var response = await _httpClient.GetAsync(_httpClient.BaseAddress + "WeatherForecast");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                throw new Exception($"Problem retrieving weather forecasts. Response Status Code: {response.StatusCode}");
            }
            return await response.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>();
        }
    }
}
