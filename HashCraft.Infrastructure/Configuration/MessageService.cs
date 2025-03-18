namespace HashCraft.Infrastructure.Configuration
{
    public class MessageService
    {
        public string ConnectionString { get; set; }
        public ushort ConcurrencyLevel { get; set; }
    }
}
