using SupportHelper.Communication.Responses;
using SupportHelper.Infrastructure.SignalR.Interfaces;
using System.Collections.Concurrent;

namespace SupportHelper.Infrastructure.SignalR.Tasks
{
    public sealed class TaskClientResponses : ITaskClientResponses
    {
        private ConcurrentDictionary<string, TaskCompletionSource<ResponseStatusMachineJson>> _pendingClientResponses = new();
        private static readonly TimeSpan DefaultResponseTimeout = TimeSpan.FromSeconds(30);

        public TimeSpan Time => DefaultResponseTimeout;

        public void Register(string requestId, TaskCompletionSource<ResponseStatusMachineJson> tcs)
        {
            if (!_pendingClientResponses.TryAdd(requestId, tcs))
            {
                throw new InvalidOperationException($"Não foi possível adicionar o requestId {requestId}.");
            }
        }

        public void Unregister(string requestId) => _pendingClientResponses.TryRemove(requestId, out _);

        public bool FinalizeTask(string requestId, ResponseStatusMachineJson data)
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
