﻿using Authentication_Basics.AuthrorizationRequirements;
using Authentication_Basics.AuthrorizationRequirments;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Authentication_Basics.AuthorizationExtensions
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            return services.AddAuthorization(options =>
                {
                    var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                        JwtBearerDefaults.AuthenticationScheme,
                        CookieAuthenticationDefaults.AuthenticationScheme);
                    defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                    options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();



                    var onlySecondJwtSchemePolicyBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
                    options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, onlySecondJwtSchemePolicyBuilder
                        .RequireAuthenticatedUser()
                        .Build());

                    var onlyCookieSchemePolicyBuilder = new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme);
                    options.AddPolicy(CookieAuthenticationDefaults.AuthenticationScheme, onlyCookieSchemePolicyBuilder
                        .RequireAuthenticatedUser()
                        .Build());

                });


            //services.AddAuthorization(options =>
            //  {
            //      var onlySecondJwtSchemePolicyBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
            //  options.AddPolicy($"Only{JwtBearerDefaults.AuthenticationScheme}", onlySecondJwtSchemePolicyBuilder
            //          .RequireAuthenticatedUser()
            //          .Build());

            //      var onlyCookieSchemePolicyBuilder = new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme);
            //      options.AddPolicy("OnlyCookieScheme", onlyCookieSchemePolicyBuilder
            //          .RequireAuthenticatedUser()
            //          .Build());


            //      //config.AddPolicy(PoliciesList.PolicyAdmin, b =>
            //      //{
            //      //    b.RequireClaim(ClaimTypes.Role, "Admin");
            //      //});

            //      //config.AddPolicy(PoliciesList.PolicyDateBirth, b =>
            //      //{
            //      //    b.AddRequirements(new CustomRequirementClaim(ClaimTypes.DateOfBirth));
            //      //});

            //      //config.AddPolicy(PoliciesList.PolicyAvarageSecurityLevel, b =>
            //      //{
            //      //    b.AddRequirements(new SecurityLevelRequirement(5));
            //      //    b.AddRequirements(new CustomRequirementClaim(ClaimTypes.DateOfBirth));
            //      //});
            //  });
            ////  .AddMultipleRequirementsAuthorizationHandler();

        }
        private static IServiceCollection AddAuthorizationHandlers(this IServiceCollection services)
        {
            return services.AddScoped<IAuthorizationHandler, CustomAuthorizationHandler>()
                           .AddScoped<IAuthorizationHandler, SecurityLevelAuthorizationHandler>();
        }

        private static IServiceCollection AddMultipleRequirementsAuthorizationHandler(this IServiceCollection services)
        {
            return services.AddScoped<IAuthorizationHandler, MultipleRequirementsHandler>();
        }
    }


}
