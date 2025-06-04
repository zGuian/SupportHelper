using SupportHelper.RabbitMQ.Interfaces;
using System.Collections.Concurrent;

namespace SupportHelper.RabbitMQ.Implementation
{
    public class ReplyAwaiter<T> : IReplyAwaiter<T> where T : class
    {
        private readonly ConcurrentDictionary<string, TaskCompletionSource<T>> _pending = new();

        public Task<T> WaitForResponseAsync(Guid correlationId, TimeSpan? timeout = null)
        {
            var correlationIdStr = correlationId.ToString();
            var tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);

            if (!_pending.TryAdd(correlationIdStr, tcs))
                throw new InvalidOperationException($"Já existe uma espera ativa para CorrelationId: {correlationIdStr}");

            var task = tcs.Task;

            // Aplica timeout, se informado
            if (timeout.HasValue)
            {
                task = task.TimeoutAfter(timeout.Value);
            }

            return task;
        }

        public void SetResponse(string correlationId, T response)
        {
            if (_pending.TryRemove(correlationId, out var tcs))
            {
                tcs.TrySetResult(response);
            }
        }

        public void SetException(string correlationId, Exception ex)
        {
            if (_pending.TryRemove(correlationId, out var tcs))
            {
                tcs.TrySetException(ex);
            }
        }
    }
}
