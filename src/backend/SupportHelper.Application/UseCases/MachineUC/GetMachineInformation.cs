using SupportHelper.Application.Interfaces;
using SupportHelper.Communication.Requests;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Entities;
using SupportHelper.Domain.Interfaces.MessageBrokerServices;
using System.Text.Json;

namespace SupportHelper.Application.UseCases.MachineUC
{
    public class GetMachineInformation : IGetMachineInformation
    {
        private readonly IProducerServices _producer;
        private readonly IConsumeServices _consume;

        public GetMachineInformation(IProducerServices producer, IConsumeServices consume)
        {
            _producer = producer;
            _consume = consume;
        }

        public async Task<MachineInformationResponse> ExecuteAsync(MachineInformationRequest request)
        {
            var message = JsonSerializer.Serialize(request);
            await _producer.PublishMessage(request.Hostname, message);
            if (request.ReplyToQueueName != null)
            {
                var machine = await _consume.ConsumeMessageAsync(request.ReplyToQueueName, true);
                var net = ConvertTo(machine);
                return new MachineInformationResponse(machine.Hostname, Guid.NewGuid().ToString(), net);
            }
            return JsonSerializer.Deserialize<MachineInformationResponse>(message)
                ?? throw new ArgumentNullException(message);
        }

        private static NetworkBoad[] ConvertTo(Machine machine)
        {
            var networkBoard = new List<NetworkBoad>();
            if (machine.NetworkBoard == null)
            {
                throw new Exception();
            }
            foreach (var item in machine.NetworkBoard)
            {
                networkBoard.Add(new NetworkBoad(item.Ipv4, item.Ipv6, item.MacAddress));
            }
            return networkBoard.ToArray();
        }
    }
}