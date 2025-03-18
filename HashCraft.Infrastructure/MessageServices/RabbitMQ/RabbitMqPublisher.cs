using HashCraft.Infrastructure.MessageServices.RabbitMQ.Exceptions;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace HashCraft.Infrastructure.MessageServices.RabbitMQ
{
    public class RabbitMqPublisher : IPublisher
    {
        private readonly ILogger<RabbitMqPublisher> _logger;

        private IConnection _connection;
        private IChannel _channel;

        public RabbitMqPublisher(ILogger<RabbitMqPublisher> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InitializeAsync(
            string connectionString,
            ushort concurrencyLevel,
            CancellationToken ct = default)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    Uri = new Uri(connectionString),
                    ConsumerDispatchConcurrency = concurrencyLevel
                };

                _connection = await factory.CreateConnectionAsync(ct);
                _channel = await _connection.CreateChannelAsync(null, ct);
            }
            catch (Exception ex)
            {
                throw new PublisherInitializationException(ex);
            }
        }

        public async Task PublishAsync(string routingKey, object message, CancellationToken ct = default)
        {
            try
            {
                await _channel.QueueDeclareAsync(
                    queue: routingKey,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null,
                    cancellationToken: ct);

                var jsonString = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(jsonString);

                await _channel.BasicPublishAsync(
                    exchange: string.Empty,
                    routingKey: routingKey,
                    body: body,
                    cancellationToken: ct);

                _logger.LogDebug("Message has been published in `{routingKey}`", routingKey);
            }
            catch (Exception ex)
            {
                throw new PublishMessageException(ex);
            }
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
