namespace SupportHelper.Domain.ValueObjects
{
    public struct SignalR
    {
        public string ConnectionId { get; set; }
        public string LastUpdate { get; set; }
        public bool IsActive { get; set; }

        public SignalR(string connectionId, bool isActive)
        {
            this.ConnectionId = connectionId;
            this.LastUpdate = DateTime.Now.ToString("dd/MM/yyyy hh:mm");
            this.IsActive = isActive;
        }

        public SignalR(string connectionId, string lastUpdate, bool isActive)
        {

            this.ConnectionId = connectionId;
            this.LastUpdate = lastUpdate;
            this.IsActive = isActive;
        }
    }
}
