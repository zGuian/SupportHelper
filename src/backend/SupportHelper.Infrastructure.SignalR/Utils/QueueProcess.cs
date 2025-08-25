using SupportHelper.Infrastructure.SignalR.Interfaces;
using System.Collections.Concurrent;

namespace SupportHelper.Infrastructure.SignalR.Utils
{
    public class QueueProcess : IQueueProcess
    {
        private readonly ConcurrentQueue<(string requestId, string response)> _queue = new();
        private readonly SemaphoreSlim _semaphore = new(0);

        public void Enqueue(string requestId, string response)
        {
            _semaphore.Release();
            _queue.Enqueue((requestId, response));
        }

        public async Task<(string requestId, string response)?> DequeueAsync()
        {
            await _semaphore.WaitAsync();
            var hasDequeue = _queue.TryDequeue(out var result);
            if (!hasDequeue)
            {
                return null;
            }
            return result;
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
    }
}
