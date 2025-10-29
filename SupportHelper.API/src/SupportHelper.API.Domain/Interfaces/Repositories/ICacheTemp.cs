using SupportHelper.API.Domain.DTOs.Generics;

namespace SupportHelper.API.Domain.Interfaces.Repositories
{
    public interface ICacheTemp
    {
        Task DeleteFileAsync(string key);
        Task<FileDataDto> GetFileAsync(string key);
        Task StoreFileAsync(string key, byte[] bytes, string contentType, TimeSpan expireAfter);
    }
}
