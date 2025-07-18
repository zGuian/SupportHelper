using System.Text.Json.Serialization;

namespace SupportHelper.Infrastructure.Data.CouchDB.Json.Responses
{
    internal sealed class InsertDataResponse
    {
        [JsonPropertyName("ok")]
        public bool Ok { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("rev")]
        public string Rev { get; set; } = string.Empty;
    }
}
