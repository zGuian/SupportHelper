using Microsoft.Extensions.Logging;
using SupportHelper.Application.Interfaces;
using SupportHelper.Communication.Requests;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Entities;
using SupportHelper.Domain.Interfaces.MQServices;
using SupportHelper.Exceptions;
using SupportHelper.Exceptions.ExceptionsBase;
using System.Text.Json;

namespace SupportHelper.Application.UseCases.MachineUC
{
    public class GetMachineInformation : IGetMachineInformation
    {
        private readonly ILogger<GetMachineInformation> _logger;
        private readonly IMachineMQServices _machineMQ;

        public GetMachineInformation(ILogger<GetMachineInformation> logger, IMachineMQServices machineMQ)
        {
            _logger = logger;
            _machineMQ = machineMQ;
        }

        public async Task<MachineInformationResponse> ExecuteAsync(MachineInformationRequest request)
        {
            var message = JsonSerializer.Serialize(request);
            var machine = await _machineMQ.GetInformationOnlyMachineAsync(request.Exchange,
                request.ReplyToQueueName, message) ?? throw new GenericErrorException([ResourceMessagesException.GENERIC_ERROR]);
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