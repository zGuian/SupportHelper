using System.Text.Json.Serialization;

namespace SupportHelper.Communication.Dtos.CouchDbDto
{
    public class RowDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("key")]
        public string Key { get; set; } = string.Empty;

        [JsonPropertyName("value")]
        public ValueDto Value { get; set; } = new();

        public RowDto(string id, string key, ValueDto value)
        {
            Id = id;
            Key = key;
            Value = value;
        }
    }
}
