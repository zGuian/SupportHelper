using System.Net;

namespace SupportHelper.Exceptions.ExceptionsBase
{
    public class FolderNotFoundException(string[] errorsMessages) : SupportHelperException(string.Empty)
    {
        private readonly string[] _errorsMessages = errorsMessages;

        public override IList<string> GetErrorMessages() =>
            _errorsMessages;

        public override HttpStatusCode GetStatusCode() =>
            HttpStatusCode.NotFound;
    }
}
