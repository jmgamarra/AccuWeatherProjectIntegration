using InterviewProject.Application.Pagination;
using InterviewProject.Controllers; 
using InterviewProject.Domain.Location;
using InteviewProject.Application;
using InteviewProject.Application.Validations.Commands;
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
            new Location { Key = "12345", LocalizedName = "Location 1" }
        };
        var response = new PaginatedResponse<Location>()
        {
            Data = locations,
            Page = 1,
            PageSize = 10,
            TotalCount = 10
        };

        var command = new GetLocationsCommand { Location = "lima", Page = 1, Size = 10 };

        var weatherServiceMock = new Mock<IWeatherService>();
        weatherServiceMock.Setup(x => x.GetLocationsAsync(It.IsAny<string>(), 1, 10))
            .ReturnsAsync(response);

        var logger = Mock.Of<ILogger<WeatherForecastController>>();
        var controller = new WeatherForecastController(logger, weatherServiceMock.Object);

        // Act
        var result = await controller.GetLocations(command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedResponse = Assert.IsType<PaginatedResponse<Location>>(okResult.Value);
        var returnedLocations = returnedResponse.Data;
        Assert.Single(returnedLocations);
        Assert.Equal("12345", returnedLocations[0].Key);
        Assert.Equal("Location 1", returnedLocations[0].LocalizedName);
    }

    [Fact]
    public async Task GetLocations_ReturnsBadRequest_WhenApiResponseIsUnsuccessful()
    {
        // Arrange
        var command = new GetLocationsCommand { Location = "lima", Page = 1, Size = 10 };

        var weatherServiceMock = new Mock<IWeatherService>();
        weatherServiceMock.Setup(service => service.GetLocationsAsync(It.IsAny<string>(), 1, 10))
            .ThrowsAsync(new HttpRequestException("Error retrieving location key."));

        var logger = Mock.Of<ILogger<WeatherForecastController>>();
        var controller = new WeatherForecastController(logger, weatherServiceMock.Object);

        // Act
        var result = await controller.GetLocations(command);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Error retrieving location key.", badRequestResult.Value);

        // Verifica que GetLocationsAsync fue llamado
        weatherServiceMock.Verify(x => x.GetLocationsAsync(It.IsAny<string>(), 1, 10), Times.Once);
    }

    [Fact]
    public async Task GetLocations_ReturnsOkResult_WithEmptyList_WhenApiResponseIsEmpty()
    {
        // Arrange
        var locations = new List<Location>();  //empty list
        var response = new PaginatedResponse<Location>()
        {
            Data = locations,
            Page = 1,
            PageSize = 10,
            TotalCount = 0
        };

        var command = new GetLocationsCommand { Location = "lima", Page = 1, Size = 10 };

        var weatherServiceMock = new Mock<IWeatherService>();
        // Mock with empty list
        weatherServiceMock.Setup(service => service.GetLocationsAsync(It.IsAny<string>(), 1, 10))
            .ReturnsAsync(response);

        var logger = Mock.Of<ILogger<WeatherForecastController>>();
        var controller = new WeatherForecastController(logger, weatherServiceMock.Object);

        // Act
        var result = await controller.GetLocations(command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedResponse = Assert.IsType<PaginatedResponse<Location>>(okResult.Value);
        Assert.Empty(returnedResponse.Data);  // empty list
        Assert.Equal(0, returnedResponse.TotalCount);  // TotalCount =0

        // Verifica que GetLocationAsync
        weatherServiceMock.Verify(x => x.GetLocationsAsync(It.IsAny<string>(), 1, 10), Times.Once);
    }
}
