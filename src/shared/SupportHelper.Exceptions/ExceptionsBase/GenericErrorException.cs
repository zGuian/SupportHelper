using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SupportHelper.Exceptions.ExceptionsBase
{
    public class GenericErrorException : SupportHelperException
    {
        private readonly string[] _errorsMessages;

        public GenericErrorException(string[] message) : base(string.Empty)
        {
            _errorsMessages = message;
        }

        public override string[] GetErrorsMessages() => _errorsMessages;

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.InternalServerError;
    }
}
