using admin_portal.Controllers;
using Microsoft.Extensions.Logging.Abstractions;

namespace admin_portal.Tests;

public class WeatherForecastControllerTests
{
    [Fact]
    public void Get_ReturnsWeatherForecasts()
    {
        // Arrange
        var logger = NullLogger<WeatherForecastController>.Instance;
        var controller = new WeatherForecastController(logger);

        // Act
        var result = controller.Get();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Count());
    }
}
