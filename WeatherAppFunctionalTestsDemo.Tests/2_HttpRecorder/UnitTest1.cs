using System.Net.Http.Json;
using HttpRecorder;
using HttpRecorder.Context;
using HttpRecorder.Matchers;
using Microsoft.Extensions.Http;
using Shouldly;
using WeatherApp.Api;

namespace WeatherApp.FunctionalTests._2_HttpRecorder;

public class UnitTest1(WeatherApp weatherAppFactory) : IClassFixture<WeatherApp>
{
    [Fact]
    public async Task WhenWeatherForecastIsCalled_ShouldCorrectlyReturnFarenheitToCelcius()
    {
        using var context = new HttpRecorderContext(ConfigurationFactory);
        var client = weatherAppFactory.CreateClient();
        var response = await client.GetFromJsonAsync<ListItems<WeatherForecast>>("/weatherforecast");
        response.Data.Single().Summary.ShouldBe("Athens", StringCompareShould.IgnoreCase);
        response.Data.Single().TemperatureC.ShouldBe(12);
        response.Data.Single().TemperatureF.ShouldBe(53);
    }

    private HttpRecorderConfiguration ConfigurationFactory(IServiceProvider serviceProvider,
        HttpMessageHandlerBuilder httpMessageHandlerBuilder)
    {
        return new HttpRecorderConfiguration
        {
            Matcher = RulesMatcher.MatchMultiple.ByHttpMethod().ByRequestUri(UriPartial.Path),
            Mode = HttpRecorderMode.Auto
        };
    }
}