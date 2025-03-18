namespace HashCraft.Infrastructure.MessageServices.RabbitMQ.Exceptions
{
    public class PublisherInitializationException : Exception
    {
        public PublisherInitializationException(Exception innerException)
            : base("Error during publisher initialization. For more details see inner exception", innerException)
        {
        }
    }
}
