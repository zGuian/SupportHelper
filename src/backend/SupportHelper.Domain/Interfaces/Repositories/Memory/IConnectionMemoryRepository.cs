namespace SupportHelper.Domain.Interfaces.Repositories.Memory
{
    public interface IConnectionMemoryRepository
    {
        void GetConnectionId(string hostname, out string connId);
        void Register(string hostname, string connectionId);
        void Remove(string hostname);
    }
}
