using Authentication_Basics.AuthenticationExtension;
using Authentication_Basics.AuthorizationExtensions;
using Authentication_Basics.ExtensionMethods;
using Authentication_Basics.SwaggerExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication_Basics
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddAuthentication()
                    .AddCookieAuthentication()
                    .AddJWTAuthentication(Configuration);

            services.AddGlobalAuthWithControllers();
            services.AddSwagger(Configuration);

            services.AddCustomAuthorization();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", builder =>
                {
                    builder
                    .WithOrigins(Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? new string[] { })
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithExposedHeaders(Configuration.GetSection("Cors:ExposedHeaders").Get<string[]>() ?? new string[] { });
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseCors("AllowSpecificOrigins");

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger(Configuration);

        }
    }
}
