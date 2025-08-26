using SupportHelper.Domain.Entities;
using SupportHelper.Domain.ValueObjects;

namespace SupportHelper.Domain.Interfaces.Models
{
    public interface IMachineModel
    {
        string Id { get; }
        string Rev { get; }
        Machine? Machine { get; }
        SignalR SignalR { get; }

        void Update(Machine machine, SignalR signalR);
        void UpdateSignalR(string connectionId);
    }
}
