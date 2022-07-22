using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace WeatherApp;

public class ForecastFunction
{
    private readonly ILogger _logger;

    private readonly string[] WeatherEffects = new[]
    {
        "Clear skies", "Harsh sunlight", "Extremely harsh sunlight", "Rain",
        "Heavy rain", "Sandstorm", "Hail", "Fog", "Strong winds"
    };

    private readonly string[] Regions = new[]
    {
        "Kanto", "Johto", "Hoenn", "Unova", "Kalos", "Alola", "Galar"
    };

    public ForecastFunction(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ForecastFunction>();
    }

    [Function("Forecast")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var response = req.CreateResponse(HttpStatusCode.OK);        

        var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            Region = Regions[Random.Shared.Next(Regions.Length)],
            Outlook = WeatherEffects[Random.Shared.Next(WeatherEffects.Length)]
        });

        response.WriteString("World weather:");
        await response.WriteAsJsonAsync(forecasts);

        return response;
    }

    private readonly record struct WeatherForecast(DateTime Date, string Region, string Outlook);
}
