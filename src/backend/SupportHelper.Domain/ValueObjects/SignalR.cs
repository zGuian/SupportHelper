namespace SupportHelper.Domain.ValueObjects
{
    public readonly struct SignalR(string connectionId, string lastUpdate)
    {
        public string ConnectionId { get; init; } = connectionId;
        public string LastUpdate { get; init; } = lastUpdate;
    }
}
