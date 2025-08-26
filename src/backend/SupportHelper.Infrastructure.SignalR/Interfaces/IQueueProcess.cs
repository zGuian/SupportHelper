namespace SupportHelper.Infrastructure.SignalR.Interfaces
{
    public interface IQueueProcess
    {
        Task<(string requestId, string response)?> DequeueAsync();
        void Dequeue(out string requestId, out string response);
        void Enqueue(string requestId, string response);
    }
}
