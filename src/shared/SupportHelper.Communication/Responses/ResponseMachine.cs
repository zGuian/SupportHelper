namespace SupportHelper.Communication.Responses
{
    public record ResponseMachine
    {
        public required string Id { get; init; }
        public required bool IsConnected { get; init; }
        public required string Hostname { get; init; }
        public bool? SgpIsRunning { get; init; }
        public required string CurrentUsername { get; init; }
        public required string DomainName { get; init; }
        public required string OperationalSystem { get; init; }
        public required IEnumerable<NetworkBoardResponse> NetworkBoards { get; init; }
        public required string UpTime { get; init; }
        public required string LastUpdate { get; init; }
    }
}
