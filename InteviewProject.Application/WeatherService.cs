using InterviewProject.Application.Pagination;
using InterviewProject.Domain.Forecast;
using InterviewProject.Domain.Location;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace InteviewProject.Application
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        public WeatherService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _apiKey = configuration["AccuWeatherApiKey"];
            _httpClient = httpClientFactory.CreateClient("AccuWeatherClient");
        }
        async Task<ForecastCollection> IWeatherService.Get5DailyForecastsAsync(string selectedKeyLocation)
        {
            var requestUrl = $"forecasts/v1/daily/5day/{selectedKeyLocation}?apikey={_apiKey}&language=en-us&details=false&metric=false";
            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            var forecastData = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ForecastCollection>(forecastData);
        }

        async Task<PaginatedResponse<Location>> IWeatherService.GetLocationsAsync(string location,int page = 1, int pageSize = 10)
        {
            var requestUrl = $"locations/v1/cities/autocomplete?apikey={_apiKey}&q={location}&language=en-us";
            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            var locationData = await response.Content.ReadAsStringAsync();
            var listLocations= JsonSerializer.Deserialize<List<Location>>(locationData);
            var listLocationsSize = listLocations.Count;
            var paginatedLocations = listLocations
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedResponse<Location>
            {
                Data = paginatedLocations,
                TotalCount = listLocationsSize,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
