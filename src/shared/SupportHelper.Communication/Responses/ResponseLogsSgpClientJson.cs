namespace SupportHelper.Communication.Responses
{
    public record ResponseLogsSgpClientJson
    {
        public required bool IsSuccess { get; init; }
        public string? Message { get; init; }
        public required string Path { get; init; }
    }
}
