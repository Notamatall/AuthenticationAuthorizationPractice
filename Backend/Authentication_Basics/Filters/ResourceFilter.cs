using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using System.Diagnostics;


namespace API.Filters
{
    /// <summary>
    /// <see cref="https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-8.0#resource-filters">link</see>
    /// <list type="bullet">
    /// <item>
    /// <term>Used to short circuit the pipeline</term>
    ///     <description>Could be used with cache to return cached data.</description>
    /// </item>
    /// <item>
    /// <term>DisableFormValueModelBindingAttribute</term>
    ///     <description>
    ///         Used for large file uploads to prevent the form data from being read into memory.
    ///         Prevents model binding from accessing the form data.
    ///     </description>
    /// </item>
    /// </list>
    /// </summary>
    public class ResourceFilterAttribute : Attribute, IResourceFilter
    {
        private readonly bool isShortCircuit;

        public ResourceFilterAttribute(bool isShortCircuit = true)
        {
            this.isShortCircuit = isShortCircuit;
        }
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            Debug.WriteLine("OnResourceExecuting");

            if (isShortCircuit)
                ShortCircuitTheRequestPipeline(context);
            else
                DisableFormValueModelBinding(context);
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            Debug.WriteLine("OnResourceExecuted");
        }

        private void ShortCircuitTheRequestPipeline(ResourceExecutingContext context)
        {
            context.Result = new ContentResult
            {
                Content = nameof(ResourceFilterAttribute)
            };
        }

        private void DisableFormValueModelBinding(ResourceExecutingContext context)
        {
            var formValueProviderFactory = context.ValueProviderFactories
                   .OfType<FormValueProviderFactory>()
                   .FirstOrDefault();

            if (formValueProviderFactory != null)
            {
                context.ValueProviderFactories.Remove(formValueProviderFactory);
            }

            var jqueryFormValueProviderFactory = context.ValueProviderFactories
                .OfType<JQueryFormValueProviderFactory>()
                .FirstOrDefault();

            if (jqueryFormValueProviderFactory != null)
            {
                context.ValueProviderFactories.Remove(jqueryFormValueProviderFactory);
            }
        }

    }
}
