using SupportHelper.API.Infra.SignalR.Interfaces;
using System.Collections.Concurrent;

namespace SupportHelper.API.Infra.SignalR.Utils
{
    // TODO: 
    // SUBSTITUIR TUPLA POR UM OBJETO ONDE POSSA ENCAPSULA OS DADOS E ADICIONAR UM CONTADOR DE TENTATIVAS. 
    public class QueueProcess : IQueueProcess
    {
        private readonly ConcurrentQueue<(string requestId, string response)> _queue = new();
        private readonly ConcurrentQueue<(string connId, string hostname)> _queueConnection = new();
        private readonly SemaphoreSlim _semaphoreResponse = new(0);
        private readonly SemaphoreSlim _semaphoreConnection = new(0);

        public void EnqueueResponses(string requestId, string response)
        {
            _semaphoreResponse.Release();
            _queue.Enqueue((requestId, response));
        }

        public async Task<(string requestId, string response)?> DequeueResponsesAsync()
        {
            await _semaphoreResponse.WaitAsync();
            var hasDequeue = _queue.TryDequeue(out var result);
            if (!hasDequeue)
            {
                //TODO: ADICIONA AO FINAL DA FILA E TENTA NOVAMENTE (MAX: 3 TENTATIVAS)
                return null;
            }
            return result;
        }

        public void DequeueResponses(out string requestId, out string response)
        {
            if (_queue.TryDequeue(out (string qRequestId, string qResponse) result))
            {
                requestId = result.qRequestId;
                response = result.qResponse;
                return;
            }
            throw new InvalidOperationException();
        }

        public void EnqueueConnections(string connId, string hostname)
        {
            _semaphoreConnection.Release();
            _queueConnection.Enqueue((connId, hostname));
        }

        public async Task<(string connId, string hostname)?> DequeueConnectionsAsync()
        {
            await _semaphoreConnection.WaitAsync();
            if (!_queueConnection.TryDequeue(out var result))
            {
                return null;
            }
            return result;
        }
    }
}
