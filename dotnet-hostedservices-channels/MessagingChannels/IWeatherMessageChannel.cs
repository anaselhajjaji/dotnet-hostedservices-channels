using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace dotnet_hostedservices_channels.MessagingChannels
{
    public interface IWeatherMessageChannel
    {
        public Task FetchWeather(string position);

        public Task ReadWeatherForecast(CancellationToken stoppingToken);
    }
}