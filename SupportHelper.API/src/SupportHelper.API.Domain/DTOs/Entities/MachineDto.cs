namespace SupportHelper.API.Domain.DTOs.Entities
{
    public record MachineDto(string Hostname, bool IsConnected, bool SgpIsRunning, string CurrentUsername
        , string DomainName, string UpTime, string OperationalSystem
        , IEnumerable<NetworkBoardDto> NetworkBoards, DateTime LastUpdate);
}
