using System.Text.Json.Serialization;

namespace SupportHelper.Application.DTOs
{
    public sealed class SignalRDto
    {
        [JsonPropertyName("ConnectionId")]
        public string ConnectionId { get; set; } = string.Empty;

        [JsonPropertyName("LastUpdate")]
        public string LastUpdate { get; set; } = string.Empty;
    }
}
