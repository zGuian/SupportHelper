namespace SupportHelper.Communication.Responses
{
    public record ResponseStatusMachineJson
    {
        public required bool IsConnected { get; init; }
        public required string Hostname { get; init; }
        public bool? SgpIsRunning { get; init; }
        public required string CurrentUsername { get; init; }
        public required string DomainName { get; init; }
        public string? OperationalSystem { get; init; }
        public NetworkBoardResponse[]? NetworkBoards { get; init; }

        public static ResponseStatusMachineJson Create(bool isConnected, string hostname, bool sgpIsRunning, string currentUsername,
            string domainName, string operationalSystem, NetworkBoardResponse[] networkBoardResponses)
        {
            return new ResponseStatusMachineJson
            {
                IsConnected = isConnected,
                Hostname = hostname,
                SgpIsRunning = sgpIsRunning,
                CurrentUsername = currentUsername,
                DomainName = domainName,
                OperationalSystem = operationalSystem,
                NetworkBoards = networkBoardResponses
            };
        }
    }
}
