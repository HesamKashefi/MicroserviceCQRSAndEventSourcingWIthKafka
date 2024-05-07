using CQRS.Core.Consumers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Post.Query.Infrastructure.Consumers
{
    public class ConsumerHosterService(ILogger<ConsumerHosterService> logger, IServiceProvider serviceProvider) : IHostedService
    {
        private readonly ILogger<ConsumerHosterService> _logger = logger;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Consumer Hosted Service is running!");

            using (var scope = _serviceProvider.CreateScope())
            {
                var consumer = scope.ServiceProvider.GetRequiredService<IEventConsumer>();

                var kafkaTopic = Environment.GetEnvironmentVariable("KAFKA_TOPIC") ?? throw new InvalidOperationException("KAFKA_TOPIC Environment Variable was not found");

                Task.Run(() => consumer.Consume(kafkaTopic), cancellationToken);
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Consumer Hosted Service is stopped!");

            return Task.CompletedTask;
        }
    }
}