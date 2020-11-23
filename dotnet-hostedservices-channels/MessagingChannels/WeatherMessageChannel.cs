using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace dotnet_hostedservices_channels.MessagingChannels
{
    public class WeatherMessageChannel : IWeatherMessageChannel
    {
        private readonly ILogger<WeatherMessageChannel> logger;
        private readonly Channel<string> channel;
        private const int MaxMessagesInChannel = 100;

        public WeatherMessageChannel(ILogger<WeatherMessageChannel> logger)
        {
            var options = new BoundedChannelOptions(MaxMessagesInChannel)
            {
                SingleReader = true,
                SingleWriter = false
            };

            channel = Channel.CreateBounded<string>(options);

            this.logger = logger;
        }

        public async Task FetchWeather(string position)
        {
            await channel.Writer.WaitToWriteAsync();
            
            if (channel.Writer.TryWrite(position))
            {
                logger.LogInformation($"Fetch weather for position {position}.");
            }
        }

        public async Task ReadWeatherForecast(CancellationToken stoppingToken)
        {
            while (await channel.Reader.WaitToReadAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
            {
                if (channel.Reader.TryRead(out var position))
                {
                    logger.LogInformation($"Received position {position} to fetch.");
                }
            }
        }
    }
}
