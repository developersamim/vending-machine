using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using product.api.Services;

namespace product.api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "seller")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IHttpContextAccessorService httpContextAccessorService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpContextAccessorService httpContextAccessorService)
    {
        _logger = logger;
        this.httpContextAccessorService = httpContextAccessorService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        var accessToken = httpContextAccessorService.GetAccessToken();
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
