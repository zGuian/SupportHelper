namespace SupportHelper.FrontEnd.MVC.Models.ValueObjects
{
    public struct NetworkBoardVO
    {
        public string Description { get; private set; }
        public string Ipv4 { get; private set; }
        public string? Ipv6 { get; private set; }
        public string MacAddress { get; private set; }
        public bool InUse { get; private set; }

        private NetworkBoardVO(string description, string ipv4, string? ipv6, string macAddress, bool inUse)
        {
            Description = description;
            Ipv4 = ipv4;
            Ipv6 = ipv6;
            MacAddress = macAddress;
            InUse = inUse;

        }

        public static NetworkBoardVO Create(string description, string ipv4, string? ipv6, string macAddress, bool inUse)
        {
            return new NetworkBoardVO(description, ipv4, ipv6, macAddress, inUse);
        }
    }
}