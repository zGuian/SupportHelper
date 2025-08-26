using CouchDB.Driver.Types;
using Newtonsoft.Json;
using SupportHelper.Domain.Entities;
using SupportHelper.Domain.Interfaces.Models;
using SupportHelper.Domain.ValueObjects;

namespace SupportHelper.Infrastructure.Data.CouchDB.Models
{
    public class MachineModel : CouchDocument, IMachineModel
    {
        
        [JsonProperty(nameof(Machine))]
        public Machine? Machine { get; private set; }

        [JsonProperty(nameof(SignalR))]
        public SignalR SignalR { get; private set; }

        [JsonConstructor]
        private MachineModel(Machine machine, SignalR signalR)
        {
            Machine = machine;
            SignalR = signalR;
        }

        private MachineModel(string hostname, string connectionId) 
        {
            Machine = Machine.Factories.CreateNullMachine(hostname);
            SignalR = new SignalR(connectionId, DateTime.Now.ToString("dd/MM/yyyy"), true);
        }

        public static class Factories
        {
            public static MachineModel CreateNullMachine(string hostname, string connectionId)
            {
                return new MachineModel(hostname, connectionId);
            }
        }

        public static class Converters
        {
            public static MachineModel ToModel(Machine machine, SignalR signalR)
            {
                return new MachineModel(machine, signalR);
            }
        }

        public void UpdateSignalR(string connectionId)
        {
            SignalR = new SignalR(connectionId, DateTime.Now.ToString("dd/MM/yyyy"), true);
        }

        public void Update(Machine machine, SignalR signalR)
        {
            Machine = machine;
            SignalR = signalR;
        }
    }
}
