namespace AspNetSixExample.Services
{
    public interface IWeatherService
    {
        Task<IEnumerable<WeatherForecast>> GetWeatherForecastAsync();
    }
}