using SupportHelper.API.Infra.SignalR.Interfaces;
using System.Collections.Concurrent;

namespace SupportHelper.API.Infra.SignalR.Tasks
{
    public sealed class TaskClientResponses : ITaskClientResponses
    {
        private ConcurrentDictionary<string, TaskCompletionSource<string>> _pendingClientResponses = new();
        private static readonly TimeSpan DefaultResponseTimeout = TimeSpan.FromSeconds(30);

        public TimeSpan Time => DefaultResponseTimeout;

        public void Register(string requestId, TaskCompletionSource<string> tcs)
        {
            if (!_pendingClientResponses.TryAdd(requestId, tcs))
            {
                throw new Exception($"Não foi possível adicionar o requestId {requestId}.");
            }
        }

        public void Unregister(string requestId) => _pendingClientResponses.TryRemove(requestId, out _);

        public void FinalizeTask(string requestId, string response)
        {
            if (_pendingClientResponses.TryRemove(requestId, out var tcs))
            {
                tcs.SetResult(response);
                return;
            }
            throw new Exception($"Não foi possivel finalizar Task: {requestId} \nResposta recebida: {response}");
        }
    }
}
