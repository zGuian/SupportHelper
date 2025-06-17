namespace SupportHelper.Communication.Responses
{
    public record MachineInformationResponse
    {
        public required string Hostname { get; init; }
        public required string CurrentUsername { get; init; }
        public required string DomainName { get; init; }
        public string? OperationalSystem { get; init; }
        public NetworkBoardResponse[]? NetworkBoards { get; init; }

        public static MachineInformationResponse Create(string hostname, string currentUsername, string domainName,
            string operationalSystem, ICollection<NetworkBoardResponse> networkBoards)
        {
            return new MachineInformationResponse
            {
                Hostname = hostname,
                CurrentUsername = currentUsername,
                DomainName = domainName,
                OperationalSystem = operationalSystem,
                NetworkBoards = [.. networkBoards]
            };
        }
    }
}
