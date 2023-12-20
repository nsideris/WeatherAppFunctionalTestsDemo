using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using WeatherApp.Api;

namespace WeatherApp.FunctionalTests._3_TestContainer;

public class WeatherApp : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, configureHostBuilder) =>
        {
            configureHostBuilder.AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string?>("ConnectionStrings:pgsqlDb",
                    _postgreSqlContainer.GetConnectionString())
            });
        });

        builder.ConfigureServices(services => { services.AddHttpRecorderContextSupport(); });
    }

    public Task InitializeAsync()
    {
        return _postgreSqlContainer.StartAsync();
    }

    public Task DisposeAsync()
    {
        return _postgreSqlContainer.DisposeAsync().AsTask();
    }
}