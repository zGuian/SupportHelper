using System.Text.Json.Serialization;

namespace SupportHelper.Communication.Dtos.CouchDbDto
{
    public sealed class MachineDto
    {
        [JsonPropertyName("Id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("Hostname")]
        public string Hostname { get; set; } = string.Empty;

        [JsonPropertyName("CurrentUsername")]
        public string CurrentUsername { get; set; } = string.Empty;

        [JsonPropertyName("DomainName")]
        public string DomainName { get; set; } = string.Empty;

        [JsonPropertyName("OperationalSystem")]
        public string OperationalSystem { get; set; } = string.Empty;

        [JsonPropertyName("NetworkBoard")]
        public ICollection<NetworkBoardDto> NetworkBoard { get; set; } = [];

        [JsonPropertyName("UpTime")]
        public string UpTime { get; set; } = string.Empty;

        [JsonPropertyName("LastUpdate")]
        public string LastUpdate { get; set; } = string.Empty;
    }
}
