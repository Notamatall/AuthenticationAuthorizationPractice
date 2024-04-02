using Authentication_Basics.Authentication.JWT;
using Authentication_Basics.Mocks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace Authentication_Basics.Authentication
{
    public static class AuthenticationExtensions
    {
        public static AuthenticationBuilder AddCookieAuthentication(this AuthenticationBuilder authBuilder)
        {
            return authBuilder.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
                      {
                          o.LoginPath = "/api/authentication/unauthorized";
                          o.AccessDeniedPath = "/api/authentication/forbidden";
               
                      });
        }
        public static AuthenticationBuilder AddMultiAuthorization(this AuthenticationBuilder builder)
        {
            return builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "MultiAuthSchemes";
                    options.DefaultChallengeScheme = "MultiAuthSchemes";
                })
        .AddCookie(options =>
        {
            options.LoginPath = "/auth/unauthorized";
            options.AccessDeniedPath = "/auth/forbidden";
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("Ig8BoyK5YZh5XONmvnM9Ig8BoyK5YZhs"))
            };
        })
        .AddJwtBearer("SecondJwtScheme", options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("Ig8BoyK5YZh5XONmvnM9Ig8BoyK5YZhs"))
            };
        })
        .AddPolicyScheme("MultiAuthSchemes", JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.ForwardDefaultSelector = context =>
            {
                string authorization = context.Request.Headers[HeaderNames.Authorization];
                if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                {
                    var token = authorization.Substring("Bearer ".Length).Trim();
                    var jwtHandler = new JwtSecurityTokenHandler();

                    return (jwtHandler.CanReadToken(token))
                        ? JwtBearerDefaults.AuthenticationScheme : "SecondJwtScheme";
                }

                return CookieAuthenticationDefaults.AuthenticationScheme;
            };
        })
        .AddJWTTokenFactory(o =>
        {
            o.Secret = "Ig8BoyK5YZh5XONmvnM9Ig8BoyK5YZhs";
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:AuthSecret"]!)),

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
            })
            .AddJWTTokenFactory(o =>
            {
                o.Secret = configuration["Authentication:AuthSecret"]!;
            });
        }

        public static AuthenticationBuilder AddBasicAuthentication(this AuthenticationBuilder authBuilder, IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            return authBuilder.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);
        }
    }
}
