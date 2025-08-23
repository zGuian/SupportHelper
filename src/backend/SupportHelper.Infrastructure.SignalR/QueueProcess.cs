using SupportHelper.Infrastructure.SignalR.Interfaces;
using System.Collections.Concurrent;

namespace SupportHelper.Infrastructure.SignalR
{
    public class QueueProcess : IQueueProcess
    {
        private readonly ConcurrentQueue<(string requestId, string response)> _queue = new();

        public void Enqueue(string requestId, string response)
        {
            _queue.Enqueue((requestId, response));
        }

        public (string requestId, string response) Dequeue()
        {
            if (_queue.TryDequeue(out (string requestId, string response) result))
            {
                return (result.requestId, result.response);
            }
            throw new InvalidOperationException();
        }

        public void Dequeue(out string requestId, out string response)
        {
            if (_queue.TryDequeue(out (string qRequestId, string qResponse) result))
            {
                requestId = result.qRequestId;
                response = result.qResponse;
                return;
            }
            throw new InvalidOperationException();
        }

        public bool HasValue()
        {
            if (_queue.IsEmpty)
            {
                return false;
            }
            return true;
        }
    }
}
