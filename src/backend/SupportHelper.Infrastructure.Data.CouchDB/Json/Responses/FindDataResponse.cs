using System.Text.Json.Serialization;

namespace SupportHelper.Infrastructure.Data.CouchDB.Json.Responses
{
    internal class FindDataResponse
    {
        [JsonPropertyName("docs")]
        public IEnumerable<Doc> Docs { get; set; } = [];

        [JsonPropertyName("bookmark")]
        public string Bookmark { get; set; } = string.Empty;

        [JsonPropertyName("warning")]
        public string Warning { get; set; } = string.Empty;
    }
}
