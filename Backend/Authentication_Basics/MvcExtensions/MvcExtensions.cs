using API.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;

using Microsoft.Extensions.DependencyInjection;

namespace API.ExtensionMethods
{
    public static class MvcExtensions
    {
        /// <summary>
        /// Add controllers.
        /// Adds authorization policy which requires each user inside each http request to be authorized.
        /// That means you do not need to specify [Authorize] attribute anymore over action or controller itself.
        /// </summary>
        public static IMvcBuilder AddGlobalAuthWithControllers(this IServiceCollection services, bool withGlobalAuth = false)
        {
            return services.AddHttpContextAccessor()
             .AddControllers(mvcOptions =>
             {
                 if (withGlobalAuth)
                 {
                     var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                     var defaultAuthPolicy = defaultAuthBuilder
                     .RequireAuthenticatedUser()
                     .Build();

                     mvcOptions.Filters.Add(new AuthorizeFilter(defaultAuthPolicy));
                 }
             });

        }

        public static IMvcBuilder AddGlobalExeptionHandlingFilter(this IMvcBuilder mvcBuilder)
        {
            return mvcBuilder.AddMvcOptions(o =>
             {
                 o.Filters.Add(typeof(TypeFilterAttribute<ExceptionFilter>));
             });
        }

    }
}
