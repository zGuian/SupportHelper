namespace SupportHelper.Communication.Requests
{
    public record RequestUpdateSgpClientJson
    {
        public required string Hostname { get; init; }
        public required string ProductionLine { get; init; }
    }
}
