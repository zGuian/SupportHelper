using SupportHelper.Communication.Requests;
using SupportHelper.Domain.Interfaces.ApplicationContext;
using SupportHelper.Exceptions.ExceptionsBase;

namespace SupportHelper.Application.Utils
{
    public class QueueUpdateSgpClient : IQueueUpdateSgpClient
    {
        private readonly Queue<(RequestUpdateSgpClientJson, string)> _queue = new();
        public Queue<(RequestUpdateSgpClientJson, string)> QueueValues => _queue;

        public void Enqueue(RequestUpdateSgpClientJson request, string connId)
        {
            _queue.Enqueue((request, connId));
        }

        public void Dequeue(out (RequestUpdateSgpClientJson, string) request)
        {
            if(_queue.TryDequeue(out var item))
            {
                request = item;
            }
            throw new NotFoundException([$"Não foi encontrado o objeto {item}"]);
        }
    }
}
