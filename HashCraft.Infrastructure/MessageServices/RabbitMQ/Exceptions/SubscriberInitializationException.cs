namespace HashCraft.Infrastructure.MessageServices.RabbitMQ.Exceptions
{
    public class SubscriberInitializationException : Exception
    {
        public SubscriberInitializationException(Exception innerException)
            : base("Error during subscriber initialization. For more details see inner exception", innerException)
        {
        }
    }
}
