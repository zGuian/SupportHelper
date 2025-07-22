using System.Text.Json.Serialization;

namespace SupportHelper.Communication.Dtos.CouchDbDto
{
    public sealed class ErrorBaseDto
    {
        [JsonPropertyName("error")]
        public string Error { get; set; } = string.Empty;

        [JsonPropertyName("reason")]
        public string Reason { get; set; } = string.Empty;
    }
}
