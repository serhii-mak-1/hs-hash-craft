using HashCraft.Infrastructure.MessageServices;
using HashCraft.Storage.Services;
using System.Text.Json;

public class HashRecieverHostedService : BackgroundService
{
    private readonly ILogger<HashRecieverHostedService> _logger;
    private readonly ISubscriberFactory _subscriberFactory;
    private readonly HashStorageService _hashService;
    private ISubscriber _subscriber;

    public HashRecieverHostedService(
        ILogger<HashRecieverHostedService> logger,
        ISubscriberFactory subscriberFactory,
        HashStorageService hashService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _subscriberFactory = subscriberFactory ?? throw new ArgumentNullException(nameof(subscriberFactory));
        _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        try
        {
            _subscriber = await _subscriberFactory.CreateAsync();
            await _subscriber.SubscribeAsync("hash_batch", ct);
            _subscriber.OnMessage += ProcessMessagesAsync;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in the hosted service.");
        }
    }

    private async Task ProcessMessagesAsync(string messages, CancellationToken ct = default)
    {
        string[] msgObject = JsonSerializer.Deserialize<string[]>(messages);
        await _hashService.SaveHashesAsync(msgObject, ct);
    }

    public override void Dispose()
    {
        _subscriber.OnMessage -= ProcessMessagesAsync;
        _subscriber?.Dispose();
        base.Dispose();
    }
}