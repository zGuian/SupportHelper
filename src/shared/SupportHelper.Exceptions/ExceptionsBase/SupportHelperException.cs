using System.Net;

namespace SupportHelper.Exceptions.ExceptionsBase
{
    public abstract class SupportHelperException(string message) : SystemException(message)
    {
        public abstract string[] GetErrorsMessages();
        public abstract HttpStatusCode GetStatusCode();
    }
}
