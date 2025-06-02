using MassTransit;
using SupportHelper.Communication.Requests;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Interfaces.MQServices;

namespace SupportHelper.Infrastructure.MQServices
{
    public class MachineMQService_MASSTRANSIT : IMachineMQServices_MASSTRANSIT
    {
        private readonly IRequestClient<MachineInformationRequest> _machineInfoClient;

        public MachineMQService_MASSTRANSIT(IRequestClient<MachineInformationRequest> machineInfoClient)
        {
            _machineInfoClient = machineInfoClient;
        }

        public async Task<MachineInformationResponse> GetMachineInfoAsync(MachineInformationRequest request)
        {
            var response = await _machineInfoClient.GetResponse<MachineInformationResponse>(request);
            return response.Message;
        }
    }
}
