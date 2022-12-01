namespace WeatherAspNet.Services
{
    public interface IWeatherService
    {
        Task<IEnumerable<WeatherForecast>> GetWeatherForecastAsync();
    }
}