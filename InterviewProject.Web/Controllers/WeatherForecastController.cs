using InteviewProject.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace InterviewProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherService _weatherService;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        [HttpGet("locations")]
        public async Task<IActionResult> GetLocations(string location)
        {
            try
            {
                var locations = await _weatherService.GetLocationsAsync(location);
                return Ok(locations);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("forecasts")]
        public async Task<IActionResult> Get5DailyForecasts(string selectedKeyLocation)
        {
            try
            {
                var forecastList = await _weatherService.Get5DailyForecastsAsync(selectedKeyLocation);
                return Ok(forecastList);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
