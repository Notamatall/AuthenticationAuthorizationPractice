using Authentication_Basics.AuthrorizationRequirements;
using Authentication_Basics.AuthrorizationRequirments;
using Authentication_Basics.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Authentication_Basics.AuthorizationExtensions
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            return services.AddAuthorization(config =>
              {
                  config.AddPolicy(PoliciesList.PolicyAdmin, b =>
                  {
                      b.RequireClaim(ClaimTypes.Role, "Admin");
                  });

                  config.AddPolicy(PoliciesList.PolicyDateBirth, b =>
                  {
                      b.AddRequirements(new CustomRequirementClaim(ClaimTypes.DateOfBirth));
                  });

                  config.AddPolicy(PoliciesList.PolicyAvarageSecurityLevel, b =>
                  {
                      b.AddRequirements(new SecurityLevelRequirement(5));
                  });
              })
             .AddAuthorizationHandlers();

        }
        private static IServiceCollection AddAuthorizationHandlers(this IServiceCollection services)
        {
            return services.AddScoped<IAuthorizationHandler, CustomAuthorizationHandler>()
                           .AddScoped<IAuthorizationHandler, SecurityLevelAuthorizationHandler>();
        }
    }


}
