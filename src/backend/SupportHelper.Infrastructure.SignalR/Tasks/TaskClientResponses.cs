using SupportHelper.Communication.Responses;
using SupportHelper.Infrastructure.SignalR.Interfaces;
using System.Collections.Concurrent;

namespace SupportHelper.Infrastructure.SignalR.Tasks
{
    public sealed class TaskClientResponses<T> : ITaskClientResponses<T> where T : class
    {
        private ConcurrentDictionary<string, TaskCompletionSource<T>> _pendingClientResponses = new();
        private static readonly TimeSpan DefaultResponseTimeout = TimeSpan.FromSeconds(30);

        public TimeSpan Time => DefaultResponseTimeout;

        public void Register(string requestId, TaskCompletionSource<T> tcs)
        {
            if (!_pendingClientResponses.TryAdd(requestId, tcs))
            {
                throw new InvalidOperationException($"Não foi possível adicionar o requestId {requestId}.");
            }
        }

        public void Unregister(string requestId) => _pendingClientResponses.TryRemove(requestId, out _);

        public bool FinalizeTask(string requestId, T data)
        {
            if (_pendingClientResponses.TryRemove(requestId, out var tcs))
            {
                tcs.SetResult(data);
                return true;
            }
            return false;
        }
    }
}
