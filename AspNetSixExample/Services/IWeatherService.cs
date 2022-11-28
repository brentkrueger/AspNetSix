namespace AspNetSixExample.Services
{
    public interface IWeatherService
    {
        Task<WeatherForecast> GetWeatherAsync();
    }
}