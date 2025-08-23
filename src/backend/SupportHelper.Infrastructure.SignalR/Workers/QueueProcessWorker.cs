using Microsoft.Extensions.Hosting;
using SupportHelper.Infrastructure.SignalR.Interfaces;

namespace SupportHelper.Infrastructure.SignalR.Workers
{
    public class QueueProcessWorker : BackgroundService
    {
        private readonly ITaskClientResponses _tcs;
        private readonly IQueueProcess _queue;

        public QueueProcessWorker(ITaskClientResponses tcs, IQueueProcess queue)
        {
            _tcs = tcs;
            _queue = queue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (!_queue.HasValue())
                {
                    continue;
                }

                await Task.Run(() =>
                {
                    _queue.Dequeue(out string requestId, out string response);
                    _tcs.FinalizeTask(requestId, response);
                }, stoppingToken);
            }
        }
    }
}
