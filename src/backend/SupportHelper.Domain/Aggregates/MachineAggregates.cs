using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Entities;
using SupportHelper.Domain.ValueObjects;

namespace SupportHelper.Domain.Aggregates
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

            public static MachineAggregates Create(ResponseStatusMachineJson responseStatusMachineJson, string connId)
            {
                var machine = new Machine(
                    responseStatusMachineJson.Id,
                    responseStatusMachineJson.Hostname.ToLower(),
                    responseStatusMachineJson.CurrentUsername.ToLower(),
                    responseStatusMachineJson.DomainName.ToLower(),
                    responseStatusMachineJson.OperationalSystem,
                    responseStatusMachineJson.NetworkBoards.Select(nb =>
                        NetworkBoard.Create(nb.Description, nb.Ipv4, nb.Ipv6, nb.MacAddress, nb.InUse)),
                    responseStatusMachineJson.IsConnected, responseStatusMachineJson.UpTime, responseStatusMachineJson.LastUpdate);

                var signalR = new SignalR(connId, responseStatusMachineJson.LastUpdate, true);

                return new MachineAggregates(machine, signalR);
            }
        }

        public static class Converters
        {
            public static MachineAggregates ToAggregate(Machine machine, SignalR signalR)
            {
                return new MachineAggregates(machine, signalR);
            }

            public static MachineAggregates ToAggregate(ResponseStatusMachineJson responseStatusMachineJson, string connId, bool isActive)
            {
                var machine = new Machine(
                    responseStatusMachineJson.Id,
                    responseStatusMachineJson.Hostname.ToLower(),
                    responseStatusMachineJson.CurrentUsername.ToLower(),
                    responseStatusMachineJson.DomainName.ToLower(),
                    responseStatusMachineJson.OperationalSystem,
                    responseStatusMachineJson.NetworkBoards.Select(nb =>
                        NetworkBoard.Create(nb.Description, nb.Ipv4, nb.Ipv6, nb.MacAddress, nb.InUse)),
                    responseStatusMachineJson.IsConnected, responseStatusMachineJson.UpTime, responseStatusMachineJson.LastUpdate);

                var signalR = new SignalR(connId, responseStatusMachineJson.LastUpdate, isActive);
                return new MachineAggregates(machine, signalR);
            }
        }

        public void UpdateSignalR(string connId, bool isActive)
        {
            SignalR = new SignalR(connId, isActive);
        }
    }
}
