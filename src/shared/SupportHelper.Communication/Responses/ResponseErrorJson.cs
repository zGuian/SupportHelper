namespace SupportHelper.Communication.Responses
{
    public sealed class ResponseErrorJson
    {
        public IList<string> Errors { get; set; }
        public string Message { get; set; }
        public bool TokenIsExpired { get; set; }

        public ResponseErrorJson(IList<string> errors, string message)
        {
            Errors = errors;
            Message = message;
            TokenIsExpired = false;
        }

        public ResponseErrorJson(string error)
        {
            Errors = [error];
            Message = string.Empty;
        }

        public ResponseErrorJson(string error, string message)
        {
            Errors = [error];
            Message = message;
        }
    }
}
