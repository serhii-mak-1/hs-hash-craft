namespace HashCraft.Infrastructure.MessageServices
{
    public interface ISubscriber : IDisposable
    {
        Task SubscribeAsync(string routingKey, CancellationToken ct = default);

        event Func<string, CancellationToken, Task> OnMessage;
    }
}
