using SupportHelper.Communication.Responses;

namespace SupportHelper.Infrastructure.SignalR.Interfaces
{
    public interface ITaskClientResponses
    {
        TimeSpan Time { get; }
        bool FinalizeTask(string requestId, ResponseStatusMachineJson data);
        void Register(string requestId, TaskCompletionSource<ResponseStatusMachineJson> tcs);
        void Unregister(string requestId);
    }
}
