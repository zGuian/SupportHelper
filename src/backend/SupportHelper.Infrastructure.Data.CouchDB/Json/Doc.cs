using SupportHelper.Domain.Entities;
using SupportHelper.Domain.ValueObjects;
using System.Text.Json.Serialization;

namespace SupportHelper.Infrastructure.Data.CouchDB.Json
{
    internal class Doc
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("_rev")]
        public string Rev { get; set; } = string.Empty;

        [JsonPropertyName("Machine")]
        public Machine Machine { get; set; }

        [JsonPropertyName("SignalR")]
        public SignalR SignalR { get; set; }
    }
}
