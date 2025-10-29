using System.Net;

namespace SupportHelper.API.Domain.Exceptions
{
    public class NotFoundException(string errorMessage) : SupportHelperException(errorMessage)
    {
        public override string GetInnerException()
        {
            if (InnerException != null)
            {
                return InnerException.ToString();
            }
            return string.Empty;
        }

        public override string GetStackTrace()
        {
            if (StackTrace != null)
            {
                return StackTrace;
            }
            return string.Empty;
        }

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
    }
}
