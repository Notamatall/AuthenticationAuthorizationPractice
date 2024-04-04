using Authentication_Basics.Authentication;
using Authentication_Basics.Authentication.Extensions;
using Authentication_Basics.AuthorizationExtensions;
using Authentication_Basics.AuthrorizationRequirments;
using Authentication_Basics.ExceptionsHandlers;
using Authentication_Basics.ExtensionMethods;
using Authentication_Basics.LoggerExtensions;
using Authentication_Basics.Middlewares;
using Authentication_Basics.SwaggerExtensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Authentication_Basics
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private const string MultiAuthSchemaName = "MultiAuthSchema";
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //     services.AddTransient<FactoryBasedAuthenticationMiddleware>();

            services.AddMemoryCache();
            services.RegisterExceptionHandlers();
            services.AddProblemDetails();
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

                //  o.DefaultChallengeScheme = MultiAuthSchemaName;
            })
            // .AddMultiAuthorization();
            // .AddBasicAuthentication(services)
            .AddCookieAuthentication()
            .AddJWTAuthentication(configuration)
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = configuration["Authentication:Google:ClientId"]!;
                googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"]!;
            });
            //  .AddPolicyScheme(MultiAuthSchemaName);


            services.AddScoped<IClaimsTransformation, ClaimsTransformation>();

            services.AddIdentityService();
            services.WithMsSqlServer(o =>
            {
                o.ConnectionString = configuration.GetConnectionString("Db");
            });
            services.WithDapper(configuration);

            services.AddSerilog(configuration);

            services.AddGlobalAuthWithControllers();

            services.AddSwagger(configuration);

            services.AddCustomAuthorization();
            services.AddAuthorizationHandlers();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", builder =>
                {
                    builder
                    .WithOrigins(configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [])
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithExposedHeaders(configuration.GetSection("Cors:ExposedHeaders").Get<string[]>() ?? Array.Empty<string>());
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler();

            app.UseSwagger(configuration);

            app.UseCors("AllowSpecificOrigins");

            app.UseAuthentication();

            //   app.UseConvetionalActivatedMiddleware();

            app.UseRouting();

            // app.UseFactoryActivatedMiddleware();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
