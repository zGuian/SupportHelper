using System.Text.Json.Serialization;

namespace SupportHelper.Communication.Dtos.CouchDbDto
{
    public class ResponseBaseDto
    {
        [JsonPropertyName("ok")]
        public required bool Ok { get; set; }

        [JsonPropertyName("id")]
        public required string Id { get; set; }

        [JsonPropertyName("rev")]
        public required string Rev { get; set; }
    }
}
