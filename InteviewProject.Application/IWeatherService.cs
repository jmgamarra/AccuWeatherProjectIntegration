using InterviewProject.Domain.Forecast;
using InterviewProject.Domain.Location;

namespace InteviewProject.Application
{
    public interface IWeatherService
    {
        Task<List<Location>> GetLocationsAsync(string location);
        Task<ForecastCollection> Get5DailyForecastsAsync(string selectedKeyLocation);
    }
}
