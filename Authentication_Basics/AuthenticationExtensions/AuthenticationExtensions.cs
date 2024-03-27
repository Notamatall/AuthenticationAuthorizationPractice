using Authentication_Basics.AuthenticationHandler;
using Authentication_Basics.Mocks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;

namespace Authentication_Basics.AuthenticationExtensions
{
    public static class AuthenticationExtensions
    {
        public static AuthenticationBuilder AddCookieAuthentication(this AuthenticationBuilder authBuilder)
        {
            return authBuilder.AddCookie("cookie", o =>
                      {
                          o.LoginPath = "/api/authentication/CreateCookie";
                      });
        }

        public static AuthenticationBuilder AddJWTAuthentication(this AuthenticationBuilder authBuilder, IConfiguration configuration)
        {
            return authBuilder.AddJwtBearer(x =>
            {

                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    //moving the clock for time validation
                    ClockSkew = TimeSpan.Zero,

                    RequireExpirationTime = true,
                    ValidateLifetime = true,

                    //validates the key neccessary for validating token
                    ValidateIssuerSigningKey = true,

                    //encripted key neccessary for validation of signature in token
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(configuration["Authentication:AuthSecret"])),

                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                // events called during authentication of request context
                x.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            context.Response.Headers.Append("Token-Expired", "true");

                        return Task.CompletedTask;
                    }
                };

            });
        }

        public static AuthenticationBuilder AddBasicAuthentication(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            return services.AddAuthentication()
                    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);
        }
    }
}
