using HashCraft.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HashCraft.Infrastructure.MessageServices.RabbitMQ
{
    public class RabbitMqPublisherFactory : IPublisherFactory
    {
        private readonly InfrastructureConfig _config;
        private readonly ILogger<RabbitMqPublisher> _logger;
        public RabbitMqPublisherFactory(
            ILogger<RabbitMqPublisher> logger,
            IOptions<InfrastructureConfig> options)
        {
            _config = options.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IPublisher> CreateAsync(CancellationToken ct = default)
        {
            var publisher = new RabbitMqPublisher(_logger);

            await publisher.InitializeAsync(
                _config.MessageService.ConnectionString,
                _config.MessageService.ConcurrencyLevel,
                ct);

            return publisher;
        }
    }
}
