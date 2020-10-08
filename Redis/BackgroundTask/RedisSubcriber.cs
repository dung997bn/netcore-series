using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Redis.BackgroundTask
{
    public class RedisSubcriber : BackgroundService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisSubcriber(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var subcriber = _connectionMultiplexer.GetSubscriber();
            return subcriber.SubscribeAsync("messages", ((chanel, value) =>
            {
                Console.WriteLine($"The message content was: {value}");
            }));
        }
    }
}
