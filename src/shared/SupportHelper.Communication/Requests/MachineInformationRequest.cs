using System.Text.Json.Serialization;

namespace SupportHelper.Communication.Requests
{
    public sealed record MachineInformationRequest
    {
        public Guid Id { get; init; }
        public string Command { get; init; }
        public RabbitMQRequest RabbitMQRequest { get; init; }

        public MachineInformationRequest(string command, RabbitMQRequest rabbitMQRequest)
        {
            Command = command;
            RabbitMQRequest = rabbitMQRequest;
        }

        [JsonConstructor]
        public MachineInformationRequest(Guid id, string command, RabbitMQRequest rabbitMQRequest)
        {
            Id = id;
            Command = command;
            RabbitMQRequest = rabbitMQRequest;
        }
    }
}
