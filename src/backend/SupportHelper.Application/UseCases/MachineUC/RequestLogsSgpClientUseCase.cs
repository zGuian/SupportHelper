using Microsoft.Extensions.Logging;
using SupportHelper.Application.Interfaces;
using SupportHelper.Communication.Requests;
using SupportHelper.Domain.Interfaces.MQServices;

namespace SupportHelper.Application.UseCases.MachineUC
{
    public class RequestLogsSgpClientUseCase : IRequestLogsSgpClientUseCase
    {
        private readonly ILogger<RequestLogsSgpClientUseCase> _logger;
        private readonly IMachineMQServices _machineMQServices;

        public RequestLogsSgpClientUseCase(IMachineMQServices machineMQServices, ILogger<RequestLogsSgpClientUseCase> logger)
        {
            _machineMQServices = machineMQServices;
            _logger = logger;
        }

        public async Task ExecuteAsync(RequestBase<RequestMachine> request, RabbitMQRequest rabbitMQRequest) =>
            await _machineMQServices.PublishMessageAsync(request, rabbitMQRequest);
    }
}
