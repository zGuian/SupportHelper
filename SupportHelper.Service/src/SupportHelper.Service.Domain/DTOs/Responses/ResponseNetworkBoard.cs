namespace SupportHelper.Service.Domain.DTOs.Responses
{
    public record ResponseNetworkBoard
    {
        public required string Description { get; init; }
        public required string Ipv4 { get; init; }
        public string? Ipv6 { get; init; }
        public required string MacAddress { get; init; }
        public bool InUse { get; init; }

        public ResponseNetworkBoard() { }

        public static ResponseNetworkBoard Create(string description, string ipv4,
            string? ipv6, string macAddress, bool inUse)
        {
            return new ResponseNetworkBoard
            {
                Description = description,
                Ipv4 = ipv4,
                Ipv6 = ipv6,
                MacAddress = macAddress,
                InUse = inUse
            };
        }
    }
}
