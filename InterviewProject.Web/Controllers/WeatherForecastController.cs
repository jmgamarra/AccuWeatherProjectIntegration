using InterviewProject.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace InterviewProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "MlbQLzAyldhmvGCQ4A90YirKmmHIcSW6";
        public WeatherForecastController(ILogger<WeatherForecastController> logger,IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("AccuWeatherClient");
        }

        [HttpGet("locations")]
        public async Task<IActionResult> GetLocations(string location)
        {
            //consuming API to get location key
            var requestUrl = $"locations/v1/cities/autocomplete?apikey={_apiKey}&q={location}&language=en-us";
            var objLocationListResponse = await _httpClient.GetAsync(requestUrl);
            if (!objLocationListResponse.IsSuccessStatusCode)
            {
                return BadRequest("Error retrieving location key.");
            }
            var locationData = await objLocationListResponse.Content.ReadAsStringAsync();
            var locations = JsonSerializer.Deserialize<List<Location>>(locationData);
            return Ok(locations);
        }
        [HttpGet("forecasts")]
        public async Task<IActionResult> Get5DailyForecasts(string selectedKeyLocation)
        {
            //consuming API to get 5 Daily forecast
            var requestUrl = $"forecasts/v1/daily/5day/{selectedKeyLocation}?apikey={_apiKey}&language=en-us&details=false&metric=false";
            var forecastListResponse = await _httpClient.GetAsync(requestUrl);
            if (!forecastListResponse.IsSuccessStatusCode)
            {
                return BadRequest("Error retrieving weather data.");
            }
            var forecastLocationsData = await forecastListResponse.Content.ReadAsStringAsync();
            var forecastList = JsonSerializer.Deserialize<ForecastCollection>(forecastLocationsData);
            return Ok(forecastList);
        }
    }
}
