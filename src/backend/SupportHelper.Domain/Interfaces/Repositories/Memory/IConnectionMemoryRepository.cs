namespace SupportHelper.Domain.Interfaces.Repositories.Memory
{
    public interface IConnectionMemoryRepository
    {
        string? GetConnectionId(string hostname);
        void Register(string equipmentId, string connectionId);
        void Remove(string equipmentId);
    }
}
