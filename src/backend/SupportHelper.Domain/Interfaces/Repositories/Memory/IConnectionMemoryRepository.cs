namespace SupportHelper.Domain.Interfaces.Repositories.Memory
{
    public interface IConnectionMemoryRepository
    {
        string GetConnectionId(string hostname);
        void Register(string hostname, string connectionId);
        void Remove(string hostname);
    }
}
