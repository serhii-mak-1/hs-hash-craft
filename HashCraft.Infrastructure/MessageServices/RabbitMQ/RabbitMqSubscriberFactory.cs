using HashCraft.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HashCraft.Infrastructure.MessageServices.RabbitMQ
{
    public class RabbitMqSubscriberFactory : ISubscriberFactory
    {
        private readonly InfrastructureConfig _config;
        private readonly ILogger<RabbitMqSubscriber> _logger;
        public RabbitMqSubscriberFactory(
            ILogger<RabbitMqSubscriber> logger,
            IOptions<InfrastructureConfig> options
            )
        {
            _config = options.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ISubscriber> CreateAsync(CancellationToken ct = default)
        {
            var subscriber = new RabbitMqSubscriber(_logger);

            await subscriber.InitializeAsync(
                _config.MessageService.ConnectionString,
                _config.MessageService.ConcurrencyLevel,
                ct);

            return subscriber;
        }
    }
}
