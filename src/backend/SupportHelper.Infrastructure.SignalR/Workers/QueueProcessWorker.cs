using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SupportHelper.Infrastructure.SignalR.Interfaces;

namespace SupportHelper.Infrastructure.SignalR.Workers
{
    public class QueueProcessWorker : BackgroundService
    {
        private readonly ILogger<QueueProcessWorker> _logger;
        private readonly ITaskClientResponses _tcs;
        private readonly IQueueProcess _queue;

        public QueueProcessWorker(ITaskClientResponses tcs, IQueueProcess queue, ILogger<QueueProcessWorker> logger)
        {
            _tcs = tcs;
            _queue = queue;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var (requestId, response) = await _queue.DequeueAsync() ?? 
                        throw new Exception($"NÃO FOI POSSIVEL PEGAR O PRIMEIRO ITEM DA FILA.");
                    _tcs.FinalizeTask(requestId, response);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Ocorreu um erro: {message}", ex.Message);
                }
            }
        }
    }
}
