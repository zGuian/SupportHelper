using Microsoft.Extensions.Logging;
using SupportHelper.Application.Interfaces;
using SupportHelper.Communication.Requests;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Entities;
using SupportHelper.Domain.Interfaces.MQServices;

namespace SupportHelper.Application.UseCases.MachineUC
{
    public class GetMachineInformation : IGetMachineInformation
    {
        private readonly ILogger<GetMachineInformation> _logger;
        private readonly IMachineMQServices _machineMQServices;

        public GetMachineInformation(ILogger<GetMachineInformation> logger, IMachineMQServices machineMQServices)
        {
            _logger = logger;
            _machineMQServices = machineMQServices;
        }

        public async Task<MachineInformationResponse> ExecuteAsync(MachineInformationRequest request)
        {
            var correlationId = await _machineMQServices.PublishByRouteKey(request);

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