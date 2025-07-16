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
        public IEnumerable<NetworkBoard> NetworkBoards { get; private set; }
        public string UpTime { get; private set; }
        public string LastUpdate { get; private set; }

        [JsonConstructor]
        public Machine(string hostname, string currentUsername, string domainName,
            string operationalSystem, IEnumerable<NetworkBoard> networkBoards, string upTime, string lastUpdate)
        {
            Id = GenerateId();
            Hostname = hostname;
            CurrentUsername = currentUsername;
            DomainName = domainName;
            OperationalSystem = operationalSystem;
            NetworkBoards = networkBoards;
            UpTime = upTime;
            LastUpdate = lastUpdate;
        }

        public static Machine Create(string hostname, string currentUsername, string domainName,
            string operationalSystem, IEnumerable<NetworkBoard> networkBoards, string upTime, string lastUpdate)
        {
            return new Machine(hostname, currentUsername, domainName, operationalSystem, networkBoards, upTime, lastUpdate);
        }

        public static Machine Convert(ResponseStatusMachineJson response)
        {
            var networkBoards = new NetworkBoard[response.NetworkBoards.Count()];
            var array = response.NetworkBoards.ToArray();
            for (int i = 0; i < response.NetworkBoards.Count(); i++)
            {
                var item = array[i];
                networkBoards[i] = new NetworkBoard(item.Description, item.Ipv4, item.Ipv6, item.MacAddress, item.InUse);
            }
            return new Machine(response.Hostname, response.CurrentUsername, response.DomainName,
                response.OperationalSystem, networkBoards, response.UpTime, response.LastUpdate);
        }
    }
}
