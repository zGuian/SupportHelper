namespace SupportHelper.FrontEnd.MVC.ValueObjects
{
    public class NetworkBoardVO
    {
        public string Description { get; private set; }
        public string Ipv4 { get; private set; }
        public string? Ipv6 { get; private set; }
        public string MacAddress { get; private set; }
        public bool InUse { get; private set; }

        public NetworkBoardVO(string description, string ipv4, string? ipv6, string macAddress, bool inUse)
        {
            Description = description;
            Ipv4 = ipv4;
            Ipv6 = ipv6;
            MacAddress = macAddress;
            InUse = inUse;
        }
    }
}
