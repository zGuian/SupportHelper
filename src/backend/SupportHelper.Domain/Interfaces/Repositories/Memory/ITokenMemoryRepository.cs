namespace SupportHelper.Domain.Interfaces.Repositories.Memory
{
    public interface ITokenMemoryRepository
    {
        (string user, string token) GetToken(string userId);
        void RegisterToken(string userId, string token);
        void RemoveToken(string userId);
    }
}
