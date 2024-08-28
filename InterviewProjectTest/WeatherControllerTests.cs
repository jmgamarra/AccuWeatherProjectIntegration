using InterviewProject.Common; // Asegúrate de usar System.Text.Json para deserializar JSON
using InterviewProject.Controllers; // Ajusta el namespace
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;

public class WeatherForecastControllerTests
{
    [Fact]
    public async Task GetLocations_ReturnsOkResult_WithListOfLocations()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        var requestUri = "/locations/v1/cities/autocomplete?apikey=MlbQLzAyldhmvGCQ4A90YirKmmHIcSW6&q=SampleLocation&language=en-us";
        var expectedResponse = "[{\"Key\": \"12345\", \"LocalizedName\": \"Sample Location\"}]";

        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri.PathAndQuery == requestUri),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedResponse)
            })
            .Verifiable();

        var httpClient = new HttpClient(mockHandler.Object)
        {
            BaseAddress = new Uri("http://localhost") // Configura la URI base
        };

        var httpClientFactory = new Mock<IHttpClientFactory>();
        httpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var logger = Mock.Of<ILogger<WeatherForecastController>>();
        var controller = new WeatherForecastController(logger, httpClientFactory.Object);

        // Act
        var result = await controller.GetLocations("SampleLocation");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var locations = Assert.IsType<List<Location>>(okResult.Value);
        Assert.Single(locations);
        Assert.Equal("12345", locations[0].Key);
        Assert.Equal("Sample Location", locations[0].LocalizedName);

        mockHandler.Verify(); // Verifica que SendAsync fue llamado
    }
    [Fact]
    public async Task GetLocations_ReturnsBadRequest_WhenApiResponseIsUnsuccessful()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        var requestUri = "/locations/v1/cities/autocomplete?apikey=MlbQLzAyldhmvGCQ4A90YirKmmHIcSW6&q=SampleLocation&language=en-us";
        var errorResponse = "Error retrieving location key.";

        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri.PathAndQuery == requestUri),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent(errorResponse)
            })
            .Verifiable();

        var httpClient = new HttpClient(mockHandler.Object)
        {
            BaseAddress = new Uri("http://localhost")
        };

        var httpClientFactory = new Mock<IHttpClientFactory>();
        httpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var logger = Mock.Of<ILogger<WeatherForecastController>>();
        var controller = new WeatherForecastController(logger, httpClientFactory.Object);

        // Act
        var result = await controller.GetLocations("SampleLocation");

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Error retrieving location key.", badRequestResult.Value);

        mockHandler.Verify(); // Verifica que SendAsync fue llamado
    }
    [Fact]
    public async Task GetLocations_ReturnsOkResult_WithEmptyList_WhenApiResponseIsEmpty()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        var requestUri = "/locations/v1/cities/autocomplete?apikey=MlbQLzAyldhmvGCQ4A90YirKmmHIcSW6&q=SampleLocation&language=en-us";
        var emptyResponse = "[]";

        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri.PathAndQuery == requestUri),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(emptyResponse)
            })
            .Verifiable();

        var httpClient = new HttpClient(mockHandler.Object)
        {
            BaseAddress = new Uri("http://localhost")
        };

        var httpClientFactory = new Mock<IHttpClientFactory>();
        httpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var logger = Mock.Of<ILogger<WeatherForecastController>>();
        var controller = new WeatherForecastController(logger, httpClientFactory.Object);

        // Act
        var result = await controller.GetLocations("SampleLocation");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var locations = Assert.IsType<List<Location>>(okResult.Value);
        Assert.Empty(locations);

        mockHandler.Verify(); // Verifica que SendAsync fue llamado
    }

}
