using CouchDB.Driver.Types;
using SupportHelper.Domain.Entities;
using SupportHelper.Domain.ValueObjects;

namespace SupportHelper.Infrastructure.Data.CouchDB.Models
{
    public class MachineModel : CouchDocument
    {
        public Machine? Machine { get; private set; }
        public SignalR SignalR { get; private set; }

        private MachineModel(string hostname, string connectionId) 
        {
            Machine = Machine.Create(hostname);
            SignalR = new SignalR(connectionId, DateTime.Now.ToString("dd/MM/yyyy"), true);
        }

        private MachineModel(Machine machine, SignalR signalR)
        {
            Machine = machine;
            SignalR = signalR;
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
    }
}
