namespace SupportHelper.Communication.Responses
{
    public record NetworkBoardResponse
    {
        public string Description { get; init; }
        public string Ipv4 { get; init; }
        public string? Ipv6 { get; init; }
        public string MacAddress { get; init; }
        public bool InUse { get; init; }

        public NetworkBoardResponse(string description, string ipv4, string? ipv6, string macAddress,
            bool inUse)
        {
            Description = description;
            Ipv4 = ipv4;
            Ipv6 = ipv6;
            MacAddress = macAddress;
            InUse = inUse;
        }
    }
}
