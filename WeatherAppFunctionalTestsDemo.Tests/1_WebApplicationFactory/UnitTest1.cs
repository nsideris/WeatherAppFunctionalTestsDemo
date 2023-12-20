using System.Net.Http.Json;
using Shouldly;
using WeatherApp.Api;

namespace WeatherApp.FunctionalTests._1_WebApplicationFactory;

public class UnitTest1(WeatherApp weatherAppFactory) : IClassFixture<WeatherApp>
{
    private readonly HttpClient _httpClient = weatherAppFactory.CreateClient();

    [Fact]
    public async Task WhenWeatherForecastIsCalled_ShouldCorrectlyReturnFarenheitToCelcius()
    {
        var response =
            await _httpClient
                .GetFromJsonAsync<ListItems<WeatherForecast>>("/weatherforecast");

        response.Data.Single().Summary.ShouldBe("Athens", StringCompareShould.IgnoreCase);
        response.Data.Single().TemperatureC.ShouldBe(16);
        response.Data.Single().TemperatureF.ShouldBe(60);
    }
}