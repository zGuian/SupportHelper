namespace SupportHelper.API.Infra.SignalR.Interfaces
{
    public interface IQueueProcess
    {
        Task<(string requestId, string response)?> DequeueResponsesAsync();
        void DequeueResponses(out string requestId, out string response);
        void EnqueueResponses(string requestId, string response);
        Task<(string connId, string hostname)?> DequeueConnectionsAsync();
        void EnqueueConnections(string connId, string hostname);
    }
}
