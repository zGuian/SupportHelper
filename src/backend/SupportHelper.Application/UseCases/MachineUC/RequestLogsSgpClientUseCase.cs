using Microsoft.Extensions.Logging;
using SupportHelper.Application.Interfaces;
using SupportHelper.Communication.Requests;
using SupportHelper.Domain.Interfaces.MQServices;

namespace SupportHelper.Application.UseCases.MachineUC
{
    public class RequestLogsSgpClientUseCase : IRequestLogsSgpClientUseCase
    {
        private readonly ILogger<RequestLogsSgpClientUseCase> _logger;
        

        public RequestLogsSgpClientUseCase (ILogger<RequestLogsSgpClientUseCase> logger)
        {
            _logger = logger;
        }

        public async Task ExecuteAsync(RequestBase<RequestMachine> request, RabbitMQRequest rabbitMQRequest) =>
            throw new NotImplementedException("This method is not implemented yet.");
    }
}
