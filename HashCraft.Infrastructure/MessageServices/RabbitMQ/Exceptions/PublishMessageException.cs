namespace HashCraft.Infrastructure.MessageServices.RabbitMQ.Exceptions
{
    public class PublishMessageException : Exception
    {
        public PublishMessageException(Exception ex)
            : base ("Error during publishing message", ex)
        {
        }
    }
}
