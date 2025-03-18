namespace HashCraft.Infrastructure.MessageServices.RabbitMQ.Exceptions
{
    public class SubscriberSubscriptionException : Exception
    {
        public SubscriberSubscriptionException(Exception innerException)
            : base("Error during subscription on message receive. For more details see inner exception", innerException)
        {
        }
    }
}
