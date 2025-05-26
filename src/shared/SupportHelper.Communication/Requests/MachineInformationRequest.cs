namespace SupportHelper.Communication.Requests
{
    public record MachineInformationRequest
    {
        public string Hostname { get; init; }
        public string? Ipv4 { get; init; }
        public string? ReplyToQueueName { get; init; }

        public MachineInformationRequest(string hostname, string? ipv4, string? replyToQueueName)
        {
            Hostname = hostname;
            Ipv4 = ipv4;
            ReplyToQueueName = replyToQueueName;
        }
    }
}
