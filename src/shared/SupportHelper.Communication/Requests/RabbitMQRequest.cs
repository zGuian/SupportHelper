namespace SupportHelper.Communication.Requests
{
    public readonly struct RabbitMQRequest
    {
        public string Hostname { get; init; }
        public string? Exchange { get; init; }
        public string? ReplyToQueueName { get; init; }

        public RabbitMQRequest(string hostname, string? exchange = "supporthelper.exchange", string? replyToQueueName = null)
        {
            Hostname = hostname.ToLower();
            if (!string.IsNullOrWhiteSpace(exchange))
            {
                Exchange = exchange.ToLower();
            }
            if (!string.IsNullOrEmpty(replyToQueueName))
            {
                ReplyToQueueName = replyToQueueName.ToLower();
            }
        }
    }
}
