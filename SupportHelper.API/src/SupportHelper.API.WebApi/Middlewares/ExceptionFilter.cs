using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SupportHelper.API.Domain.Exceptions;

namespace SupportHelper.API.WebApi.Middlewares
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is SupportHelperException supportHelperException)
                HandleProjectException(supportHelperException, context);
            else
                ThrowUnknowException(context);
        }

        private static void HandleProjectException(SupportHelperException supportHelperException, ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)supportHelperException.GetStatusCode();
            context.Result = new ObjectResult(new
            {
                supportHelperException.Message,
                supportHelperException.InnerException,
                supportHelperException.StackTrace
            });
        }

        private static void ThrowUnknowException(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(new
            {
                context.Exception.Message,
                context.Exception.InnerException,
                context.Exception.StackTrace
            });
        }
    }
}
