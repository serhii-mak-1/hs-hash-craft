using HashCraft.Infrastructure.MessageServices.RabbitMQ.Exceptions;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace HashCraft.Infrastructure.MessageServices.RabbitMQ
{
    public class RabbitMqSubscriber : ISubscriber
    {
        private readonly ILogger<RabbitMqSubscriber> _logger;
        private ushort _concurrencyLevel;

        private IConnection _connection;
        private IChannel _channel;

        public event Func<string, CancellationToken, Task> OnMessage;

        public RabbitMqSubscriber(ILogger<RabbitMqSubscriber> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InitializeAsync(string connectionString, ushort concurrencyLevel, CancellationToken ct = default)
        {
            _concurrencyLevel = concurrencyLevel;

            try
            {
                var factory = new ConnectionFactory
                {
                    Uri = new Uri(connectionString),
                    ConsumerDispatchConcurrency = concurrencyLevel
                };

                _connection = await factory.CreateConnectionAsync(ct);
                _channel = await _connection.CreateChannelAsync(null, ct);
            }
            catch (Exception ex)
            {
                throw new SubscriberInitializationException(ex);
            }
        }

        public async Task SubscribeAsync(string routingKey, CancellationToken ct = default)
        {
            try
            {
                await _channel.QueueDeclareAsync(
                queue: routingKey,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                noWait: false,
                cancellationToken: ct);

                await _channel.BasicQosAsync(
                    prefetchSize: 0,
                    prefetchCount: _concurrencyLevel,
                    global: false,
                    cancellationToken: ct);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (model, ea) =>
                {
                    try
                    {
                        byte[] body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);

                        await OnMessage(message, ct);

                        await _channel.BasicAckAsync(
                            deliveryTag: ea.DeliveryTag,
                            multiple: false,
                            cancellationToken: ct);

                        _logger.LogDebug("Successfully received message from `{routingKey}`", routingKey);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error occurred on message processing");
                    }
                };

                await _channel.BasicConsumeAsync(routingKey, autoAck: false, consumer: consumer, cancellationToken: ct);
            }
            catch (Exception ex)
            {
                throw new SubscriberSubscriptionException(ex);
            }
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
