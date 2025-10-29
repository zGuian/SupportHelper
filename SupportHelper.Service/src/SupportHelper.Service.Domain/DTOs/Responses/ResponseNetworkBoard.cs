namespace SupportHelper.Service.Domain.DTOs.Responses
{
    public class ResponseNetworkBoard
    {
        public string Description { get; set; } = string.Empty;
        public string Ipv4 { get; set; } = string.Empty;
        public string? Ipv6 { get; set; } = string.Empty;
        public string MacAddress { get; set; } = string.Empty;
        public bool InUse { get; set; }

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
