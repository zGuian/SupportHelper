using System.Net;

namespace SupportHelper.API.Domain.Exceptions
{
    public abstract class SupportHelperException(string message) : SystemException(message)
    {
        public abstract HttpStatusCode GetStatusCode();
        public abstract string GetStackTrace();
        public abstract string GetInnerException();
    }
}
