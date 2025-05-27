using Microsoft.Extensions.Logging;
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
        private readonly ILogger<GetMachineInformation> _logger;

        public GetMachineInformation(IProducerServices producer, IConsumeServices consume,
            ILogger<GetMachineInformation> logger)
        {
            _producer = producer;
            _consume = consume;
            _logger = logger;
        }

        public async Task<MachineInformationResponse> ExecuteAsync(MachineInformationRequest request)
        {
            var message = JsonSerializer.Serialize(request);
            await _producer.PublishMessage(request.Hostname, message);
            var machine = await _consume.ConsumeMessageAsync(request.ReplyToQueueName, true);
            _logger.LogInformation("Lido os seguintes valores {}", machine.ToString());
            var networkBoardResponse = ConvertInNetworkBoardResponse(machine);
            return new MachineInformationResponse(machine.Hostname, machine.CurrentUsername,
                machine.DomainName, machine.OperationalSystem, networkBoardResponse);
        }

        private static HashSet<NetworkBoardResponse> ConvertInNetworkBoardResponse(Machine machine)
        {
            var networkBoard = new HashSet<NetworkBoardResponse>();
            if (machine.NetworkBoards == null) return [];
            foreach (var adpter in machine.NetworkBoards)
            {
                networkBoard.Add(new NetworkBoardResponse(adpter.Description, adpter.Ipv4, adpter.Ipv6,
                    adpter.MacAddress, adpter.InUse));
            }
            return networkBoard;
        }
    }
}