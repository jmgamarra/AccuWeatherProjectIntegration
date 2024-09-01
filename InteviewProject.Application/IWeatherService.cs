using InterviewProject.Application.Pagination;
using InterviewProject.Domain.Forecast;
using InterviewProject.Domain.Location;

namespace InteviewProject.Application
{
    public interface IWeatherService
    {
        Task<PaginatedResponse<Location>> GetLocationsAsync(string location,int page,int size);
        Task<ForecastCollection> Get5DailyForecastsAsync(string selectedKeyLocation);
    }
}
