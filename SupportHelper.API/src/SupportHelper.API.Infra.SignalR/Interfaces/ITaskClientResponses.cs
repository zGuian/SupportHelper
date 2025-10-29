namespace SupportHelper.API.Infra.SignalR.Interfaces
{
    public interface ITaskClientResponses
    {
        TimeSpan Time { get; }
        void FinalizeTask(string requestId, string data);
        void Register(string requestId, TaskCompletionSource<string> tcs);
        void Unregister(string requestId);
    }
}
