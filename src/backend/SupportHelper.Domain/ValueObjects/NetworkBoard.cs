namespace SupportHelper.Domain.ValueObjects
{
    public class NetworkBoard
    {
        public string Name { get; private set; }
        public string? Ipv4 { get; private set; }
        public string? Ipv6 { get; private set; }
        public string? MacAddress { get; private set; }
        public bool InUse { get; private set; }

        public NetworkBoard(string name, string? ipv4, string? ipv6, string? macAddress, bool inUse)
        {
            Name = name;
            Ipv4 = ipv4;
            Ipv6 = ipv6;
            MacAddress = macAddress;
            InUse = inUse;
        }
    }
}
