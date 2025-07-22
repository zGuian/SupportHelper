using System.Text.Json.Serialization;

namespace SupportHelper.Communication.Dtos.CouchDbDto
{
    public class AllDocsDto
    {
        [JsonPropertyName("total_rows")]
        public int? TotalRows { get; set; }

        [JsonPropertyName("offset")]
        public int? Offset { get; set; }

        [JsonPropertyName("rows")]
        public IEnumerable<RowDto> Rows { get; } = [];
    }
}
