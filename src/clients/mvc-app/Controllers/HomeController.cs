using System.Diagnostics;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_app.Models;
using mvc_app.Services;
using Newtonsoft.Json;

namespace mvc_app.Controllers;


public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ITokenService tokenService;

    public HomeController(ILogger<HomeController> logger, ITokenService tokenService)
    {
        _logger = logger;
        this.tokenService = tokenService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    public async Task<IActionResult> Weather()
    {
        var data = new List<WeatherData>();

        using(var client = new HttpClient())
        {
            var responseToken = await tokenService.GetToken("client_access");

            client.SetBearerToken(responseToken.AccessToken);
            //client.SetBearerToken("eyJhbGciOiJSUzI1NiIsImtpZCI6IjI1OENGRDVCQjg2MDA4RkEyQzFFREQ1RTc2MkYxMEU0IiwidHlwIjoiYXQrand0In0.eyJuYmYiOjE2NDcwOTA5OTEsImV4cCI6MTY0NzA5NDU5MSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSIsImF1ZCI6InByb2R1Y3RfcmVzb3VyY2UiLCJjbGllbnRfaWQiOiJwcm9kdWN0LmFwaSIsImp0aSI6IkMwQjg5RTJEMkZDN0U5Njg3NEY0OUFFN0YwRDkyRkQ5IiwiaWF0IjoxNjQ3MDkwOTkxLCJzY29wZSI6WyJzZXJ2ZXJfYWNjZXNzIl19.jqasnCueswvc2jUQKl6X6CpUHrFQ - HV1ngSADqJnpb5tTDetXnmOJeAXoj9KhFnuHVi1rgbXoZc - YGZpPOWsfTP - 2LuQTBFbEfs39ydhjlbEdqAb7yc9TpV8y4ltlyNdoK2CZu5zKoGwhXfX1qXmXG2WX0Opz05MVUnwFo4ky_69cT6UPTrTWwtiIJQ6MV5tW6wqX7KFXfttqQr9Xv4gRwpbI0Cgd9mORFJLmXuAXqnLyXiki02gvFkMCAxeB9xlV - r5CI_HEAiHC2OQXNLX8x6D7YfGQ2lEyax7jvGjoeo1jeIQkH9rC0jjrovFdOgy_evlrbjUzoIUs7nKuZKZhQ");

            var result = client.GetAsync("http://localhost:8000/weatherforecast").Result;
            if (result.IsSuccessStatusCode)
            {
                var stringData = result.Content.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject<List<WeatherData>>(stringData);

                return View(data);
            }
            else
            {
                throw new Exception("Unable to get content");
            }
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
