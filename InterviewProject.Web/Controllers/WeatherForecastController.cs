using InteviewProject.Application;
using InteviewProject.Application.Validations.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
        public async Task<IActionResult> GetLocations([FromQuery] GetLocationsCommand command)
        {
            try
            {
                var locations = await _weatherService.GetLocationsAsync(command.Location, command.Page, command.Size);
                return Ok(locations);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("forecasts")]
        public async Task<IActionResult> Get5DailyForecasts([FromQuery] GetForecastsCommand command)
        {
            try
            {
                var forecastList = await _weatherService.Get5DailyForecastsAsync(command.SelectedKeyLocation);
                return Ok(forecastList);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
