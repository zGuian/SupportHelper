using StackExchange.Redis;
using SupportHelper.API.Domain.DTOs.Generics;
using SupportHelper.API.Domain.Interfaces.Repositories;

namespace SupportHelper.API.Infra.Data.Repositories.Memory
{
    public class CacheTemp(IConnectionMultiplexer redis) : ICacheTemp
    {
        private readonly IDatabase _redis = redis.GetDatabase();

        public async Task StoreFileAsync(string key, byte[] bytes, string contentType, TimeSpan expireAfter)
        {
            await _redis.StringSetAsync(key, bytes, expireAfter);
            await _redis.StringSetAsync($"contentType-{key}", contentType, expireAfter);
        }

        public async Task<FileDataDto> GetFileAsync(string key)
        {
            try
            {
                var bytes = (byte[]?)await _redis.ExecuteAsync("GETDEL", key) ?? throw new Exception("STREAM IS NULL");
                var stream = new MemoryStream(bytes);

                var result2 = await _redis.ExecuteAsync("GETDEL", $"contentType-{key}");
                var contentType = result2.IsNull ? "application/zip" : (string)result2!;

                return new FileDataDto(this.ToString(), contentType, stream);
            }
            catch (Exception)
            {
                // ADD LOGGER
                return new FileDataDto();
            }
        }

        public async Task DeleteFileAsync(string key)
        {
            await _redis.KeyDeleteAsync(key);
        }

        public override string ToString()
        {
            var date = DateTimeOffset.Now.ToString("dd-MM-yyyy_HH-mm-ss-fff");
            return $"SGPClient_logs_{date}.zip";
        }
    }
}
