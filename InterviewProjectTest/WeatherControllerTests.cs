using InterviewProject.Controllers; // Ajusta el namespace
using InterviewProject.Domain.Location;
using InteviewProject.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

public class WeatherForecastControllerTests
{
    [Fact]
    public async Task GetLocations_ReturnsOkResult_WithListOfLocations()
    {
        // Arrange
        var locations = new List<Location>
        {
            new Location { Key = "12345", LocalizedName = "Sample Location" }
        };

        var weatherServiceMock = new Mock<IWeatherService>();
        weatherServiceMock.Setup(x => x.GetLocationsAsync(It.IsAny<string>())).ReturnsAsync(locations);

        var logger = Mock.Of<ILogger<WeatherForecastController>>();
        var controller = new WeatherForecastController(logger, weatherServiceMock.Object);

        // Act
        var result = await controller.GetLocations("SampleLocation");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedLocations = Assert.IsType<List<Location>>(okResult.Value);
        Assert.Single(returnedLocations);
        Assert.Equal("12345", returnedLocations[0].Key);
        Assert.Equal("Sample Location", returnedLocations[0].LocalizedName);
    }
    [Fact]
    public async Task GetLocations_ReturnsBadRequest_WhenApiResponseIsUnsuccessful()
    {
        // Arrange
        var weatherServiceMock = new Mock<IWeatherService>();

        // Configura el mock para simular un fallo en el servicio
        weatherServiceMock
            .Setup(service => service.GetLocationsAsync(It.IsAny<string>()))
            .ThrowsAsync(new HttpRequestException("Error retrieving location key."));

        var logger = Mock.Of<ILogger<WeatherForecastController>>();
        var controller = new WeatherForecastController(logger, weatherServiceMock.Object);

        // Act
        var result = await controller.GetLocations("SampleLocation");

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Error retrieving location key.", badRequestResult.Value);

        weatherServiceMock.Verify(); // Verifica que GetLocationsAsync fue llamado
    }
    [Fact]
    public async Task GetLocations_ReturnsOkResult_WithEmptyList_WhenApiResponseIsEmpty()
    {
        // Arrange
        var weatherServiceMock = new Mock<IWeatherService>();

        // Configura el mock para devolver una lista vacía
        weatherServiceMock
            .Setup(service => service.GetLocationsAsync(It.IsAny<string>()))
            .ReturnsAsync(new List<Location>());

        var logger = Mock.Of<ILogger<WeatherForecastController>>();
        var controller = new WeatherForecastController(logger,weatherServiceMock.Object);

        // Act
        var result = await controller.GetLocations("SampleLocation");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var locations = Assert.IsType<List<Location>>(okResult.Value);
        Assert.Empty(locations);

        weatherServiceMock.Verify(); // Verifica que GetLocationsAsync fue llamado
    }

}
