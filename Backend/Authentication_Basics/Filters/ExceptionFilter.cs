using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace Authentication_Basics.Filters
{
    /// <summary>
    /// <b>Can be used to implement common error handling policies.</b>
    /// <br/>
    /// <b>Prefer middleware for exception handling. Use exception filters only where error handling differs based on which action method is called.</b>
    ///  <br/>
    /// <b>For example, an app might have action methods for both API endpoints and for views/HTML. </b>
    /// <br/>
    /// <b>The API endpoints could return error information as JSON, while the view-based actions could return an error page as HTML.</b>
    /// </summary>
    public class ExceptionFilter(ILogger logger) : ExceptionFilterAttribute
    {
        private readonly ILogger logger = logger;

        public override void OnException(ExceptionContext context)
        {
            LogException(context);
            HandleException(context);

            context.ExceptionHandled = true;
        }

        private void LogException(ExceptionContext context)
        {
            logger.Error(context.Exception, "Exception message: {0}", context.Exception.Message);
        }

        public void HandleException(ExceptionContext context)
        {
            var result = new { Message = "An error has occured during the current operation. Please try again." };
            context.Result = new ObjectResult(result) { StatusCode = StatusCodes.Status500InternalServerError };
        }

    }
}
