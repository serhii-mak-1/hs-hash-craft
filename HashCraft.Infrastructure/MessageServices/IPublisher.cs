namespace HashCraft.Infrastructure.MessageServices
{
    public interface IPublisher : IDisposable
    {
        Task PublishAsync(string routingKey, object message, CancellationToken ct = default);
    }
}
