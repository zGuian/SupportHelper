using SupportHelper.API.Domain.DTOs.Responses;
using SupportHelper.API.Domain.Entities.ValueObjects;
using System.Text.Json.Serialization;

namespace SupportHelper.API.Domain.DTOs.Client
{
    public class InfoMachineClient
    {
        public required bool IsConnected { get; set; }
        public required string Hostname { get; set; }
        public bool? SgpIsRunning { get; set; }
        public required string CurrentUsername { get; set; }
        public required string DomainName { get; set; }
        public required string OperationalSystem { get; set; }
        public IEnumerable<NetworkBoardJson> NetworkBoardsJ { get; set; } = [];
        public required string UpTime { get; set; }
        public SignalR SignalR { get; set; }

        [JsonConstructor]
        public InfoMachineClient(bool isConnected, string hostname, bool? sgpIsRunning, string currentUsername
            , string domainName, string operationalSystem, IEnumerable<NetworkBoardJson> networkBoards
            , string upTime, string connId, bool isActive)
        {
            IsConnected = isConnected;
            Hostname = hostname;
            SgpIsRunning = sgpIsRunning;
            CurrentUsername = currentUsername;
            DomainName = domainName;
            OperationalSystem = operationalSystem;
            NetworkBoardsJ = networkBoards;
            UpTime = upTime;
            SignalR = new SignalR(connId, isActive);
        }
    }
}
