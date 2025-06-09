using SupportHelper.Domain.ValueObjects;
using System.Text;

namespace SupportHelper.Domain.Entities
{
    public class Machine : EntityBase
    {
        public string Hostname { get; private set; }
        public string CurrentUsername { get; private set; }
        public string DomainName { get; private set; }
        public string OperationalSystem { get; private set; }
        public NetworkBoard[] NetworkBoards { get; private set; }

        public Machine()
        {
            Id = GenerateId();
            Hostname = string.Empty;
            DomainName = string.Empty;
            CurrentUsername = string.Empty;
            OperationalSystem = string.Empty;
            NetworkBoards = [];
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
