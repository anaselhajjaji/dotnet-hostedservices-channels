using dotnet_hostedservices_channels.MessagingChannels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_hostedservices_channels.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> logger;
        private readonly IWeatherMessageChannel weatherMessageChannel;
        private static int position = 0;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherMessageChannel weatherMessageChannel)
        {
            this.logger = logger;
            this.weatherMessageChannel = weatherMessageChannel;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            this.weatherMessageChannel.FetchWeather($"Position {++position}");

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
