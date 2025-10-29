namespace SupportHelper.API.Infra.SignalR.Interfaces
{
    public interface IConnectionService
    {
        string? GetConnectionId(string equipmentId);
        void Register(string equipmentId, string connectionId);
        void Remove(string equipmentId);
    }
}
