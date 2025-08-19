using SupportHelper.Domain.Aggregates;
using SupportHelper.Domain.Entities;
using SupportHelper.Domain.ValueObjects;
using System.Text.Json.Serialization;

namespace SupportHelper.Infrastructure.Data.CouchDB.Json
{
    internal class Doc
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("_rev")]
        public string Rev { get; set; } = string.Empty;

        [JsonPropertyName("Machine")]
        public Machine Machine { get; set; }

        [JsonPropertyName("SignalR")]
        public SignalR SignalR { get; set; }

        [JsonConstructor]
        public Doc(string id, string rev, Machine machine, SignalR signalR)
        {
            Id = id;
            Rev = rev;
            Machine = machine;
            Machine.AddNetworkBoard(machine.NetworkBoards.Select(nb => NetworkBoard.Create(nb.Description, nb.Ipv4, 
                nb.Ipv6, nb.MacAddress, nb.InUse)));
            SignalR = signalR;
        }

        public void Update(MachineSchemaJson machine)
        {
            Machine = machine.Machine;
            SignalR = machine.SignalR;
        }
    }
}
