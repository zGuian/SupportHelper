using SupportHelper.Communication.Requests;

namespace SupportHelper.Application.Interfaces
{
    public interface IRequestLogsSgpClientUseCase
    {
        Task ExecuteAsync(RequestBase<RequestMachine> request, RabbitMQRequest rabbitMQRequest);
    }
}
