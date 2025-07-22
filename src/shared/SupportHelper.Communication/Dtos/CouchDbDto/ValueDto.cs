using System.Text.Json.Serialization;

namespace SupportHelper.Communication.Dtos.CouchDbDto
{
    public class ValueDto
    {
        [JsonPropertyName("rev")]
        public string Rev { get; set; } = string.Empty;
    }
}
