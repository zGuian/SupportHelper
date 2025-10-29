using System.Text.Json.Serialization;

namespace SupportHelper.API.Domain.Entities
{
    public class NetworkBoard
    {
        public int Id { get; private set; }
        public string Description { get; private set; }
        public string Ipv4 { get; private set; }
        public string? Ipv6 { get; private set; }
        public string MacAddress { get; private set; }
        public bool InUse { get; private set; }
        public virtual Machine Machine { get; private set; } = null!;
        public int MachineId { get; private set; }

        [JsonConstructor]
        public NetworkBoard(int id, string description, string ipv4, string? ipv6, string macAddress, bool inUse)
        {
            Id = id;
            Description = description;
            Ipv4 = ipv4;
            Ipv6 = ipv6;
            MacAddress = macAddress;
            InUse = inUse;
        }

        public NetworkBoard(string description, string ipv4, string? ipv6, string macAddress, bool inUse)
        {
            Description = description;
            Ipv4 = ipv4;
            Ipv6 = ipv6;
            MacAddress = macAddress;
            InUse = inUse;
        }

        public static NetworkBoard Create(string description, string ipv4, string? ipv6, string macAddress, bool inUse)
        {
            return new NetworkBoard(description, ipv4, ipv6, macAddress, inUse);
        }
    }
}
