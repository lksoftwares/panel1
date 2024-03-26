using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using panel1;

namespace Panel1.Controllers
{
    //[EnableCors("ReactConnection")]

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

      
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WeatherForecastController( IHttpContextAccessor httpContextAccessor, ILogger<WeatherForecastController> logger)
        {
            _httpContextAccessor = httpContextAccessor;

            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            _logger.LogInformation(ipAddress);
            Console.WriteLine( ipAddress);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
