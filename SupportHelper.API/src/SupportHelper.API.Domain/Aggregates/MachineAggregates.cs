using SupportHelper.API.Domain.DTOs.Responses;
using SupportHelper.API.Domain.Entities;
using SupportHelper.API.Domain.Entities.ValueObjects;

namespace SupportHelper.API.Domain.Aggregates
{
    public sealed class MachineAggregates
    {
        public Machine Machine { get; private set; }
        public SignalR SignalR { get; private set; }

        private MachineAggregates(Machine machine, SignalR signalR)
        {
            Machine = machine;
            SignalR = signalR;
        }

        public static class Factories
        {
            public static MachineAggregates Create(Machine machine, SignalR signalR)
            {
                return new MachineAggregates(machine, signalR);
            }
        }

        public static MachineAggregates ToAggregates(Machine machine, string connId, bool isActive)
        {
            var signalR = new SignalR(connId, isActive);
            return new MachineAggregates(machine, signalR);
        }

        public void UpdateSignalR(string connId, bool isActive)
        {
            SignalR = new SignalR(connId, isActive);
        }
    }
}
