using Microsoft.Extensions.Logging;
using SupportHelper.Application.Interfaces;
using SupportHelper.Communication.Requests;
using SupportHelper.Domain.Interfaces.MQServices;

namespace SupportHelper.Application.UseCases.MachineUC
{
    public class RequestMachineInformationUseCase : IRequestMachineInformationUseCase
    {
        private readonly ILogger<RequestMachineInformationUseCase> _logger;
        private readonly IMachineMQServices _machineMQServices;

        public RequestMachineInformationUseCase(ILogger<RequestMachineInformationUseCase> logger,
            IMachineMQServices machineMQServices)
        {
            _logger = logger;
            _machineMQServices = machineMQServices;
        }

        public async Task ExecuteAsync(MachineInformationRequest request)
        {
            await PublishRabbitMQ(request);
            _logger.LogInformation("Publicado mensagem. Retornando OK para controller");
        }

        private async Task PublishRabbitMQ(MachineInformationRequest request) =>
            await _machineMQServices.PublishMessageAsync(request);
    }
}