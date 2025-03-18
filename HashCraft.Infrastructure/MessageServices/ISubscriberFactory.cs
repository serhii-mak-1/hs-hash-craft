namespace HashCraft.Infrastructure.MessageServices
{
    public interface ISubscriberFactory
    {
        Task<ISubscriber> CreateAsync(CancellationToken ct = default);
    }
}
