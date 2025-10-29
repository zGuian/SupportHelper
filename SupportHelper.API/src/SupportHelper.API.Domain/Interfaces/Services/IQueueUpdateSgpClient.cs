using SupportHelper.API.Domain.DTOs.Requests;

namespace SupportHelper.API.Domain.Interfaces.Services
{
    public interface IQueueUpdateSgpClient
    {
        Queue<(RequestUpdateSgpClientJson, string)> QueueValues { get; }

        void Dequeue(out (RequestUpdateSgpClientJson, string) request);
        void Enqueue(RequestUpdateSgpClientJson request, string connId);
    }
}
