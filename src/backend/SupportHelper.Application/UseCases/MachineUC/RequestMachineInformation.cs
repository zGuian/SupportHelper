using Microsoft.Extensions.Logging;
using SupportHelper.Application.Interfaces;
using SupportHelper.Communication.Requests;
using SupportHelper.Domain.Interfaces.MQServices;
using SupportHelper.Domain.Interfaces.Repositories;

namespace SupportHelper.Application.UseCases.MachineUC
{
    public class RequestMachineInformation : IRequestMachineInformation
    {
        private readonly ILogger<RequestMachineInformation> _logger;
        private readonly IMachineMQServices _machineMQServices;
        private readonly IMachineRepository _machineRepository;

        public RequestMachineInformation(ILogger<RequestMachineInformation> logger, IMachineMQServices machineMQServices,
            IMachineRepository machineRepository)
        {
            _logger = logger;
            _machineMQServices = machineMQServices;
            _machineRepository = machineRepository;
        }

        public async Task ExecuteAsync(MachineInformationRequest request)
        {
            await PublishRabbitMQ(request);
        }

        private async Task PublishRabbitMQ(MachineInformationRequest request) =>
            await _machineMQServices.PublishGetInformationAsync(request);
    }
}