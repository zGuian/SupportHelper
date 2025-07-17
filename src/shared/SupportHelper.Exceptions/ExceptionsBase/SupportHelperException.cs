using System.Net;

namespace SupportHelper.Exceptions.ExceptionsBase
{
    public abstract class SupportHelperException(string message) : SystemException(message)
    {
        public abstract IList<string> GetErrorMessages();
        public abstract HttpStatusCode GetStatusCode();
    }
}
