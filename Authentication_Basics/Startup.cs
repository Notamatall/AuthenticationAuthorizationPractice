using Authentication_Basics.Authentication;
using Authentication_Basics.AuthorizationExtensions;
using Authentication_Basics.ExceptionsHandlers;
using Authentication_Basics.ExtensionMethods;
using Authentication_Basics.LoggerExtensions;
using Authentication_Basics.SwaggerExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Authentication_Basics
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterExceptionHandlers();
            services.AddProblemDetails();
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddBasicAuthentication(services)
            .AddCookieAuthentication()
            .AddJWTAuthentication(configuration);

            services.AddSerilog(configuration);

            services.AddGlobalAuthWithControllers();

            services.AddSwagger(configuration);

            services.AddCustomAuthorization();

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

            app.UseCors("AllowSpecificOrigins");

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger(configuration);

        }
    }
}
