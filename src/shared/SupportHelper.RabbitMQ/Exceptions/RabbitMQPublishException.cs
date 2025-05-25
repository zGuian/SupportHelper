namespace SupportHelper.RabbitMQ.Exceptions
{
    public class RabbitMQPublishException : Exception
    {
        public RabbitMQPublishException(string message, Exception innerException) 
            : base(message, innerException) { }

        public RabbitMQPublishException(string message) : base(message) { }
    }
}
