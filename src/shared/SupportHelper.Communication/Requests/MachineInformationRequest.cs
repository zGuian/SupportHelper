namespace SupportHelper.Communication.Requests
{
    public sealed record MachineInformationRequest
    {
        public Guid Id { get; init; }
        public string Command { get; init; }
        public RabbitMQRequest RabbitMQRequest { get; init; }

        public MachineInformationRequest(RabbitMQRequest mqRequest)
        {
            RabbitMQRequest = mqRequest;
        }
    }
}
