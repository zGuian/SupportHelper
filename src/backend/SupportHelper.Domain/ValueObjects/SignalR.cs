namespace SupportHelper.Domain.ValueObjects
{
    public struct SignalR(string connectionId, string lastUpdate, bool isActive)
    {
        public string ConnectionId { get; set; } = connectionId;
        public string LastUpdate { get; set; } = lastUpdate;
        public bool IsActive { get; set; } = isActive;
    }
}
