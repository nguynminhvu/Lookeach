using Lookeach.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Lookeach.Exceptions
{
    public class ExceptionHandler : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                HandlerException(context);
                context.ExceptionHandled = true;
            }
        }

        private void HandlerException(ExceptionContext context)
        {
            ResponseViewModel responseView = new ResponseViewModel
            {
                Error = new ErrorViewModel
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Message = context.Exception.Message
                },
                IsFailure = true,
                IsSuccess = false,
                Value = null!
            };
            if (context.Exception is ArgumentException ||
                context.Exception is ArgumentNullException ||
                context.Exception is Exception)
            {
                responseView.Error.Code = 400;
            }
            if (context.Exception is UnauthorizedAccessException)
            {
                responseView.Error.Code = 401;
            }
            context.Result = new ObjectResult(responseView)
            {
                StatusCode = responseView.Error.Code
            };
        }
    }
}
