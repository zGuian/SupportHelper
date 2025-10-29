using System.Text.Json.Serialization;

namespace SupportHelper.API.Domain.Entities.ValueObjects
{
    public class SignalR
    {
        public string ConnectionId { get; set; }
        public bool IsActive { get; set; }

        [JsonConstructor]
        public SignalR(string connectionId, string lastUpdate, bool isActive)
        {

            this.ConnectionId = connectionId;
            this.IsActive = isActive;
        }

        public SignalR(string connectionId, bool isActive)
        {
            this.ConnectionId = connectionId;
            this.IsActive = isActive;
        }
    }
}
