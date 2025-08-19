using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Entities;
using SupportHelper.Domain.ValueObjects;

namespace SupportHelper.Domain.Aggregates
{
    public sealed class MachineSchemaJson
    {
        public Machine Machine { get; private set; }
        public SignalR SignalR { get; private set; }

        private MachineSchemaJson(Machine machine, SignalR signalR)
        {
            Machine = machine;
            SignalR = signalR;
        }

        public static MachineSchemaJson Create(Machine machine, SignalR signalR)
        {
            return new MachineSchemaJson(machine, signalR);
        }

        public static MachineSchemaJson Create(ResponseStatusMachineJson responseStatusMachineJson, string connId)
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

            var signalR = new SignalR(connId, responseStatusMachineJson.LastUpdate);

            return new MachineSchemaJson(machine, signalR);
        }
    }
}
