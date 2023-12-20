using System.Text.Json.Serialization;

namespace WeatherApp.Api;

public class MeteoService(HttpClient httpClient)
{
    private readonly string remoteServiceBaseUrl = "v1/forecast";

    public Task<Temperatures?> GetMeteoItem(Point point)
    {
        var uri =
            $"{remoteServiceBaseUrl}?latitude={point.X}&longitude={point.Y}&current=temperature_2m,wind_speed_10m&hourly=temperature_2m,relative_humidity_2m,wind_speed_10m";
        return httpClient.GetFromJsonAsync<Temperatures>(uri);
    }
}

public record Point(double X, double Y);

public partial class Temperatures
{
    [JsonPropertyName("latitude")] public double? Latitude { get; set; }

    [JsonPropertyName("longitude")] public double? Longitude { get; set; }
    [JsonPropertyName("current")] public Current Current { get; set; }
}

public partial class Current
{
    [JsonPropertyName("time")] public string Time { get; set; }

    [JsonPropertyName("interval")] public int? Interval { get; set; }

    [JsonPropertyName("temperature_2m")] public double? Temperature2M { get; set; }

    [JsonPropertyName("wind_speed_10m")] public double? WindSpeed10M { get; set; }
}