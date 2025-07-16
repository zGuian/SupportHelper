using System.Text.Json.Serialization;

namespace SupportHelper.Communication.Dtos.CouchDbDto
{
    public sealed class NetworkBoardDto
    {
        [JsonPropertyName("Description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("Ipv4")]
        public string Ipv4 { get; set; } = string.Empty;

        [JsonPropertyName("Ipv6")]
        public string? Ipv6 { get; set; }

        [JsonPropertyName("MacAddress")]
        public string MacAddress { get; set; } = string.Empty;

        [JsonPropertyName("InUse")]
        public bool InUse { get; set; }
    }
}
