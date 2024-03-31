using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Authentication_Basics.Authentication.JWT
{
    public static class JWTExtensions
    {
        public static AuthenticationBuilder AddJWTTokenFactory(this AuthenticationBuilder authBuilder, Action<JWTTokenCreationOptions> options)
        {
            authBuilder.Services.AddOptions<JWTTokenCreationOptions>().Configure(options);
            authBuilder.Services.AddTransient<JWTTokenFactory>();

            return authBuilder;
        }
    }
}
