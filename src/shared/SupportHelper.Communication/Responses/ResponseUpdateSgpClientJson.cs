namespace SupportHelper.Communication.Responses
{
    public record ResponseUpdateSgpClientJson
    {
        public required string VersionToSgp { get; init; }
        public required string Directory { get; init; }
    }
}
