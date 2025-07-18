using System.Net;

namespace SupportHelper.Exceptions.ExceptionsBase
{
    public class NotFoundException : SupportHelperException
    {
        private readonly string[] _errorsMessages;

        public NotFoundException(string[] message) : base(string.Empty)
        {
            _errorsMessages = message;
        }

        public override string[] GetErrorMessages() => _errorsMessages;

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
    }
}
