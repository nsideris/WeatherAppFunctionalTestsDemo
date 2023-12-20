using System.Net.Http.Json;
using HttpRecorder;
using HttpRecorder.Context;
using HttpRecorder.Matchers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using NpgsqlTypes;
using Shouldly;
using WeatherApp.Api;

namespace WeatherApp.FunctionalTests._3_TestContainer;

public class UnitTest1(WeatherApp weatherAppFactory) : IClassFixture<WeatherApp>
{
    [Fact]
    public async Task WhenWeatherForecastIsCalled_ShouldCorrectlyReturnFarenheitToCelcius()
    {
        using var context = new HttpRecorderContext(ConfigurationFactory);

        using (var scope = weatherAppFactory.Services.CreateScope())
        {
            var service = scope.ServiceProvider.GetRequiredService<WeatherContext>();
            await service.Database.EnsureCreatedAsync();
            service.Countries.Add(new Country()
            {
                Name = "ATHENS",
                Location = new NpgsqlPoint(37.983810, 23.727539)
            });
            await service.SaveChangesAsync();
        }


        var client = weatherAppFactory.CreateClient();
        var response = await client.GetFromJsonAsync<ListItems<WeatherForecast>>("/weatherforecast");
        response.Data.Single().Summary.ShouldBe("Athens", StringCompareShould.IgnoreCase);
        response.Data.Single().TemperatureC.ShouldBe(16);
        response.Data.Single().TemperatureF.ShouldBe(60);
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