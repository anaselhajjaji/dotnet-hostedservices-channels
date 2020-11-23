using dotnet_hostedservices_channels.MessagingChannels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace dotnet_hostedservices_channels.Services
{
    public class WeatherForecastService : BackgroundService
    {
        private readonly ILogger<WeatherForecastService> logger;
        private readonly IWeatherMessageChannel weatherMessageChannel;

        public WeatherForecastService(ILogger<WeatherForecastService> logger, IWeatherMessageChannel weatherMessageChannel)
        {
            this.logger = logger;
            this.weatherMessageChannel = weatherMessageChannel;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await this.weatherMessageChannel.ReadWeatherForecast(stoppingToken);
        }
    }
}
