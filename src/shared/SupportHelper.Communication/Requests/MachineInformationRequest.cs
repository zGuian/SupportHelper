namespace SupportHelper.Communication.Requests
{
    public record MachineInformationRequest
    {
        public string Hostname { get; init; }
        public string ReplyToQueueName { get; init; }

        public MachineInformationRequest(string hostname, string replyToQueueName)
        {
            Hostname = hostname;
            ReplyToQueueName = replyToQueueName;
        }
    }
}
