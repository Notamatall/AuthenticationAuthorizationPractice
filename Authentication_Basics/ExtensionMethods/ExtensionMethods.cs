using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication_Basics.ExtensionMethods
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Add controllers.
        /// Adds authorization policy which requires each user inside each http request to be authorized.
        /// That means you do not need to specify [Authorize] attribute anymore over action or controller itself.
        /// </summary>
        public static IMvcBuilder AddGlobalAuthWithControllers(this IServiceCollection services, bool withGlobalAuth = false)
        {
            return services.AddHttpContextAccessor()
             .AddControllers(config =>
             {
                 if (withGlobalAuth)
                 {
                     var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                     var defaultAuthPolicy = defaultAuthBuilder
                     .RequireAuthenticatedUser()
                     .Build();

                     config.Filters.Add(new AuthorizeFilter(defaultAuthPolicy));
                 }
             });
        }
    }
}
