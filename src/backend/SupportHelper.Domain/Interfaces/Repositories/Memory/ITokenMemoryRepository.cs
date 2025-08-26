namespace SupportHelper.Domain.Interfaces.Repositories.Memory
{
    public interface ITokenMemoryRepository
    {
        void GetToken(string userId, out string token);
        void RegisterToken(string userId, string token);
        void RemoveToken(string userId);
    }
}
