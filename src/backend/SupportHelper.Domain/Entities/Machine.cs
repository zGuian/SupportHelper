using SupportHelper.Communication.Responses;
using SupportHelper.Domain.ValueObjects;
using System.Text.Json.Serialization;

namespace SupportHelper.Domain.Entities
{
    public class Machine : EntityBase
    {
        public string Hostname { get; private set; }
        public string CurrentUsername { get; private set; }
        public string DomainName { get; private set; }
        public string OperationalSystem { get; private set; }
        public bool SgpIsRunning { get; private set; }
        public IEnumerable<NetworkBoard> NetworkBoards { get; private set; }
        public string UpTime { get; private set; }
        public string LastUpdate { get; private set; }

        [JsonConstructor]
        public Machine(string id, string hostname, string currentUsername, string domainName,
            string operationalSystem, IEnumerable<NetworkBoard> networkBoards, bool isConnected,
            string upTime, string lastUpdate) : base(id, isConnected)
        {
            Hostname = hostname.ToLower();
            CurrentUsername = currentUsername;
            DomainName = domainName;
            OperationalSystem = operationalSystem;
            NetworkBoards = networkBoards;
            UpTime = upTime;
            LastUpdate = lastUpdate;
        }

        public static Machine Create(string hostname)
        {
            return new Machine(GenerateId(), hostname, string.Empty, string.Empty, string.Empty, [], false, string.Empty, string.Empty);
        }

        public static Machine Create(string id, string hostname, string currentUsername, string domainName,
            string operationalSystem, IEnumerable<NetworkBoard> networkBoards, bool isConnected, string upTime, string lastUpdate)
        {
            return new Machine(id, hostname, currentUsername, domainName, operationalSystem, networkBoards, isConnected, upTime, lastUpdate);
        }

        public void AddNetworkBoard(IEnumerable<NetworkBoard> networkBoards)
        {
            NetworkBoards = networkBoards;
        }

        public static Machine Convert(ResponseStatusMachineJson response)
        {
            var networkBoards = new NetworkBoard[response.NetworkBoards.Count()];
            var array = response.NetworkBoards.ToArray();
            for (int i = 0; i < response.NetworkBoards.Count(); i++)
            {
                var item = array[i];
                networkBoards[i] = NetworkBoard.Create(item.Description, item.Ipv4, item.Ipv6, item.MacAddress, item.InUse);
            }
            return new Machine(response.Id, response.Hostname, response.CurrentUsername, response.DomainName,
                response.OperationalSystem, networkBoards, response.IsConnected, response.UpTime, response.LastUpdate);
        }
    }
}
