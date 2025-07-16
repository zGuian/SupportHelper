using SupportHelper.Domain.Interfaces.Repositories.Memory;

namespace SupportHelper.Infrastructure.Data.Repositories.Memory
{
    public sealed class TokenMemoryRepository : ITokenMemoryRepository
    {
        private readonly Dictionary<string, string> _tokens = [];

        public void RegisterToken(string userId, string token)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("Token and UserId cannot be null or empty.");
            }
            _tokens[userId] = token;
        }

        public (string user, string token) GetToken(string userId)
        {
            if (!_tokens.TryGetValue(userId, out var token))
            {
                throw new KeyNotFoundException($"Token for user '{userId}' not found.");
            }
            return (userId, token);
        }

        public void RemoveToken(string userId)
        {
            _tokens.Remove(userId);
        }
    }
}
