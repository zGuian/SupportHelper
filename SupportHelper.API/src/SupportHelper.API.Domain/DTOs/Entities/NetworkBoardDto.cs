namespace SupportHelper.API.Domain.DTOs.Entities
{
    public record NetworkBoardDto(string Description, string Ipv4, string? Ipv6, string MacAddress, bool InUse);
}
