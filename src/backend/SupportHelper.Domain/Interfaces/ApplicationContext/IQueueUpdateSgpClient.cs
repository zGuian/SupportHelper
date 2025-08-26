using SupportHelper.Communication.Requests;

namespace SupportHelper.Domain.Interfaces.ApplicationContext
{
    public interface IQueueUpdateSgpClient
    {
        Queue<(RequestUpdateSgpClientJson, string)> QueueValues { get; }

        void Dequeue(out (RequestUpdateSgpClientJson request, string connId) request);
        void Enqueue(RequestUpdateSgpClientJson request, string connId);
    }
}
