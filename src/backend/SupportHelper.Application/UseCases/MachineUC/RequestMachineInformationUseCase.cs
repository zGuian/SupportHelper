using Microsoft.Extensions.Logging;
using SupportHelper.Application.Interfaces;
using SupportHelper.Communication.Requests;
using SupportHelper.Domain.Interfaces.MQServices;
using SupportHelper.Domain.Interfaces.SignalRContext;

namespace SupportHelper.Application.UseCases.MachineUC
{
    public class RequestMachineInformationUseCase : IRequestMachineInformationUseCase
    {
        private readonly ILogger<RequestMachineInformationUseCase> _logger;
        private readonly IMachineMQServices _machineMQServices;
        private readonly IMachineSignalRServices _machineSignalR;

        public RequestMachineInformationUseCase(ILogger<RequestMachineInformationUseCase> logger,
            IMachineMQServices machineMQServices, IMachineSignalRServices machineSignalR)
        {
            _logger = logger;
            _machineMQServices = machineMQServices;
            _machineSignalR = machineSignalR;
        }

        public async Task ExecuteAsync(MachineInformationRequest request)
        {
            _logger.LogInformation("ENVIADO SINAL VIA SIGNALR");
            await _machineSignalR.RequestStatusAsync(request.RabbitMQRequest.Hostname);
            //await PublishRabbitMQ(request);
        }

        private async Task PublishRabbitMQ(MachineInformationRequest request) =>
            await _machineMQServices.PublishMessageAsync(request);
    }
}