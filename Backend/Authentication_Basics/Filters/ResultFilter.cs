using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Diagnostics;

namespace Authentication_Basics.Filters
{
    /// <summary>
    /// Short circuiting the result in case of need using <b>Cancel = true</b>
    /// <list type="bullet">
    /// <item>
    /// <description>
    ///     Run immediately before and after the execution of action results.
    /// </description>
    /// </item>
    ///  <item>
    /// <description>
    ///     Run only when the action method executes successfully.
    /// </description>
    /// </item>
    ///  <item>
    /// <description>
    ///     Are useful for logic that must surround view or formatter execution.
    /// </description>
    /// </item>
    /// 
    /// </list>
    /// </summary>
    public class ResultFilter : Attribute, IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            Debug.WriteLine("OnResultExecuting");
            context.Result = new ContentResult
            {
                Content = nameof(ResultFilter)
            };
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            Debug.WriteLine("OnResultExecuted");
        }
    }
}
