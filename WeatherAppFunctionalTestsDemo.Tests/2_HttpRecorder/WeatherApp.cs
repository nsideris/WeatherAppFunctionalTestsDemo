using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using WeatherApp.Api;

namespace WeatherApp.FunctionalTests._2_HttpRecorder;

public class WeatherApp : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services => { services.AddHttpRecorderContextSupport(); });
    }
}