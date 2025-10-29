using SupportHelper.API.Domain.Entities.ValueObjects;
using SupportHelper.API.Domain.Interfaces.Entities;
using System.Text.Json.Serialization;

namespace SupportHelper.API.Domain.Entities
{
    public class Machine : EntityBase
    {
        public string Hostname { get; private set; } = string.Empty;
        public bool IsConnected { get; private set; }
        public bool SgpIsRunning { get; private set; }
        public string CurrentUsername { get; private set; } = string.Empty;
        public string DomainName { get; private set; } = string.Empty;
        public string UpTime { get; private set; } = string.Empty;
        public string OperationalSystem { get; private set; } = string.Empty;
        public List<NetworkBoard> NetworkBoards { get; private set; } = [];
        public DateTime LastUpdate { get; private set; } = DateTimeOffset.Now.LocalDateTime;
        public SignalR SignalR { get; private set; } = null!;

        public Machine() : base()
        { }

        [JsonConstructor]
        public Machine(int id, string hostname, string currentUsername, string domainName,
            string operationalSystem, IEnumerable<NetworkBoard> networkBoards, bool isConnected,
            string upTime, SignalR signalR) : base(id)
        {
            Hostname = hostname.ToLower();
            CurrentUsername = currentUsername;
            DomainName = domainName;
            OperationalSystem = operationalSystem;
            NetworkBoards = [.. networkBoards];
            IsConnected = isConnected;
            UpTime = upTime;
            LastUpdate = DateTimeOffset.Now.LocalDateTime;
            SignalR = signalR;
        }

        public Machine(string hostname, string currentUsername, string domainName,
            string operationalSystem, IEnumerable<NetworkBoard> networkBoards, bool isConnected,
            string upTime, SignalR signalR)
        {
            Hostname = hostname.ToLower();
            CurrentUsername = currentUsername;
            DomainName = domainName;
            OperationalSystem = operationalSystem;
            NetworkBoards = [.. networkBoards];
            IsConnected = isConnected;
            UpTime = upTime;
            LastUpdate = DateTimeOffset.Now.LocalDateTime;
            SignalR = signalR;
        }

        public Machine SignalDesconnect()
        {
            SignalR.ConnectionId = "OFF";
            SignalR.IsActive = false;
            return this;
        }
    }
}
