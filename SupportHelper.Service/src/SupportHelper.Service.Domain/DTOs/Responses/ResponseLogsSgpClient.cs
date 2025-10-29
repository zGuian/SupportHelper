namespace SupportHelper.Service.Domain.DTOs.Responses
{
    public record ResponseLogsSgpClient(bool HasSuccess, string? Message, string path);
}
