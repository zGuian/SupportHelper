using SupportHelper.Communication.Responses;
using SupportHelper.Domain.ValueObjects;
using System.Text;
using System.Text.Json.Serialization;

namespace SupportHelper.Domain.Entities
{
    public class Machine : EntityBase
    {
        public string Hostname { get; private set; }
        public string CurrentUsername { get; private set; }
        public string DomainName { get; private set; }
        public string OperationalSystem { get; private set; }
        public NetworkBoard[] NetworkBoards { get; private set; }

        [JsonConstructor]
        public Machine(string hostname, string currentUsername, string domainName, 
            string operationalSystem, NetworkBoard[] networkBoards)
        {
            Hostname = hostname;
            CurrentUsername = currentUsername;
            DomainName = domainName;
            OperationalSystem = operationalSystem;
            NetworkBoards = networkBoards;
        }

        public static Machine Create(string hostname, string currentUsername, string domainName,
            string operationalSystem, NetworkBoard[] networkBoards)
        {
            return new Machine(hostname, currentUsername, domainName, operationalSystem, networkBoards);
        }

        public static Machine Convert(MachineInformationResponse response)
        {
            List<NetworkBoard> networkBoards = [];
            foreach (var item in response.NetworkBoards)
            {
                networkBoards.Add(new NetworkBoard(item.Description, item.Ipv4, item.Ipv6, item.MacAddress, item.InUse));
            }
            return new Machine(response.Hostname, response.CurrentUsername, response.DomainName, response.OperationalSystem, 
                [.. networkBoards]);
        }

        public static Machine Convert(ResponseStatusMachineJson response)
        {
            List<NetworkBoard> networkBoards = [];
            foreach (var item in response.NetworkBoards)
            {
                networkBoards.Add(new NetworkBoard(item.Description, item.Ipv4, item.Ipv6, item.MacAddress, item.InUse));
            }
            return new Machine(response.Hostname, response.CurrentUsername, response.DomainName, response.OperationalSystem,
                [.. networkBoards]);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Dominio..... {DomainName}");
            sb.AppendLine($"Hostname do equipamento..... {Hostname}");
            sb.AppendLine($"Usuario atual..... {CurrentUsername}");
            sb.AppendLine($"Sistema Operacional..... {OperationalSystem}");
            foreach (var networkBoard in NetworkBoards)
            {
                sb.AppendLine($"Descrição..... {networkBoard.Description}");
                sb.AppendLine(string.Empty.PadLeft(networkBoard.Description.Length, '='));
                sb.AppendLine($"IPV4..... {networkBoard.Ipv4}");
                sb.AppendLine($"IPV6..... {networkBoard.Ipv6}");
                sb.AppendLine($"MacAddress..... {networkBoard.MacAddress}");
                sb.AppendLine($"Esta em uso..... {networkBoard.InUse}");
            }
            return sb.ToString();
        }
    }
}
