namespace HashCraft.Infrastructure.MessageServices
{
    public interface IPublisherFactory
    {
        Task<IPublisher> CreateAsync(CancellationToken ct = default);
    }
}
