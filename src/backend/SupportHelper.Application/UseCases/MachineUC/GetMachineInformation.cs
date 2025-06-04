using Microsoft.Extensions.Logging;
using SupportHelper.Application.Interfaces;
using SupportHelper.Communication.Requests;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Entities;
using SupportHelper.Domain.Interfaces.MQServices;
using SupportHelper.Domain.Interfaces.Repositories;

namespace SupportHelper.Application.UseCases.MachineUC
{
    public class GetMachineInformation : IGetMachineInformation
    {
        private readonly ILogger<GetMachineInformation> _logger;
        private readonly IMachineMQServices _machineMQServices;
        private readonly IMachineRepository _machineRepository;

        public GetMachineInformation(ILogger<GetMachineInformation> logger, IMachineMQServices machineMQServices,
            IMachineRepository machineRepository)
        {
            _logger = logger;
            _machineMQServices = machineMQServices;
            _machineRepository = machineRepository;
        }

        public async Task ExecuteAsync(MachineInformationRequest request)
        {
            var correlationId = await _machineMQServices.GetInformationPublishAsync(request);
            
        }
    }
}