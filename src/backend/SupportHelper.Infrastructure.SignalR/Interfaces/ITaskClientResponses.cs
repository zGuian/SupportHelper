namespace SupportHelper.Infrastructure.SignalR.Interfaces
{
    public interface ITaskClientResponses
    {
        TimeSpan Time { get; }
        bool FinalizeTask(string requestId, string data);
        void Register(string requestId, TaskCompletionSource<string> tcs);
        void Unregister(string requestId);
    }
}
