using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Diagnostics;

namespace Authentication_Basics.Filters
{
    /// <summary>
    /// 
    /// <list type="bullet">
    /// <item>
    /// <see href="https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-8.0#action-filters">Action filter</see>
    /// </item>
    /// <item>
    ///     <term>Throwing an exception in an action method</term>
    ///     <description>
    ///         Prevents running of subsequent filters.
    ///         Unlike setting Result, is treated as a failure instead of a successful result.
    ///     </description>
    /// </item>
    ///<item>
    /// <term>Filter used to</term>
    ///  <description>
    ///    Validate model state.
    ///    Return an error if the state is invalid.
    ///  </description>
    /// </item>
    /// </list>
    /// </summary>
    public class ActionFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Debug.WriteLine("OnActionExecuting");
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Debug.WriteLine("OnActionExecuted - isCanceled: {0}, ExceptionHandled: {1}", context.Canceled, context.ExceptionHandled);
        }

    }
}
