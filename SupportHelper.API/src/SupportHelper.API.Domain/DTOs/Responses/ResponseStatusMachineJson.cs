namespace SupportHelper.API.Domain.DTOs.Responses
{
    public record ResponseStatusMachineJson
    {
        public required bool IsConnected { get; init; }
        public required string Hostname { get; init; }
        public bool? SgpIsRunning { get; init; }
        public required string CurrentUsername { get; init; }
        public required string DomainName { get; init; }
        public required string OperationalSystem { get; init; }
        public IEnumerable<NetworkBoardJson> NetworkBoards { get; init; } = [];
        public required string UpTime { get; init; }
    }
}
