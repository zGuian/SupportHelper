namespace SupportHelper.Communication.Responses
{
    public record MachineInformationResponse
    {
        public string Hostname { get; init; }
        public string CurrentUsername { get; init; }
        public string DomainName { get; init; }
        public string OperationalSystem { get; init; }
        public NetworkBoardResponse[]? NetworkBoards { get; init; }

        private MachineInformationResponse()
        {
            Hostname = string.Empty;
            DomainName = string.Empty;
            CurrentUsername = string.Empty;
            OperationalSystem = string.Empty;
            NetworkBoards = [];
        }

        public MachineInformationResponse(string hostname, string currentUsername, string domainName, 
            string operationalSystem, ICollection<NetworkBoardResponse> networkBoards)
        {
            Hostname = hostname;
            CurrentUsername = currentUsername;
            DomainName = domainName;
            OperationalSystem = operationalSystem;
            NetworkBoards = [..networkBoards];
        }
    }
}
