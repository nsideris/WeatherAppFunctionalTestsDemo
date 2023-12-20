using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WeatherApp.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<MeteoService>(o => o.BaseAddress = new("https://api.open-meteo.com/"));

builder.Services.AddDbContext<WeatherContext>(opt =>
    opt
        .UseNpgsql(builder.Configuration.GetConnectionString("pgsqlDb"))
        .UseSnakeCaseNamingConvention()
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/weatherforecast", GetWeather)
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();


static async Task<Results<Ok<ListItems<WeatherForecast>>, BadRequest<string>>> GetWeather(
    WeatherContext context,
    MeteoService meteoService)
{
    var all = context.Countries.Select(x => x).ToList();
    var weather = await meteoService.GetMeteoItem(new Point(all.Single().Location.X, all.Single().Location.Y));
    return TypedResults.Ok(new ListItems<WeatherForecast>(new List<WeatherForecast>()
    {
        new(
            DateTime.Now,
            (int) weather.Current.Temperature2M,
            all.Single().Name)
    }));
}

namespace WeatherApp.Api
{
    public record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);
    }


    public record ListItems<TEntity>(IEnumerable<TEntity> Data) where TEntity : class
    {
    }

    public partial class Program
    {
    }
}