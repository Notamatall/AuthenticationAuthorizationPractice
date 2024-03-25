using Api.Authentication;
using Elfo.Round.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services
               .AddAuthentication(auth =>
               {
                   auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                   auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
               }).AddJwtBearer(x =>
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
                       IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(Configuration["Authentication:AuthSecret"])),

                       ValidateIssuer = false,
                       ValidateAudience = false
                   };
                   // events called during authentication of request context
                   x.Events = new JwtBearerEvents
                   {

                       OnAuthenticationFailed = context =>
                       {
                           if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                               context.Response.Headers.Add("Token-Expired", "true");

                           return Task.CompletedTask;
                       }
                   };
               }).AddRoundIdentity(authOptions =>
               {
                   authOptions.HowToObtainUsernameFromIdentityName = identityName => identityName;
               });

            //   services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
            //options => Configuration.Bind("JwtSettings", options))
            //     .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
            //options => Configuration.Bind("CookieSettings", options));


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

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseSwaggerUI();

            app.UseCors("AllowSpecificOrigins");

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
