using System.Text.Json.Serialization;

namespace SupportHelper.Communication.Dtos.CouchDbDto
{
    public sealed class BaseDto
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("_rev")]
        public string Rev { get; set; } = string.Empty;

        [JsonPropertyName("Machine")]
        public MachineDto Machine { get; set; } = new();

        [JsonPropertyName("SignalR")]
        public SignalRDto SignalR { get; set; } = new();
    }
}
