namespace SupportHelper.Domain.ValueObjects
{
    public struct SignalR(string connectionId, string lastUpdate)
    {
        public string ConnectionId { get; set; } = connectionId;
        public string LastUpdate { get; set; } = lastUpdate;
    }
}
