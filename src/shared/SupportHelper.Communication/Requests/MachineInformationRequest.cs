namespace SupportHelper.Communication.Requests
{
    public sealed record MachineInformationRequest
    {
        public Guid Id { get; init; }
        public string Command { get; init; }
        public RabbitMQRequest RabbitMQRequest { get; init; }

        public MachineInformationRequest(string command, RabbitMQRequest mqRequest)
        {
            Command = command;
            RabbitMQRequest = mqRequest;
        }
    }
}
