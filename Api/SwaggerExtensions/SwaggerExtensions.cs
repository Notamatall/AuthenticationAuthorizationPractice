using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Api.SwaggerExtensions
{
    public static class SwaggerExtensions
    {
        public static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                const string docVersion = "v1";
                c.SwaggerDoc(docVersion, new OpenApiInfo
                {
                    Title = configuration["ApiName"],
                    Version = docVersion                 
                });
 
                c.CustomSchemaIds(s => s.FullName);

                var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.XML");
                c.IncludeXmlComments(xmlPath, true);
                c.AddSwaggerAuthentication(configuration);
            });
        }

        public static void UseSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";       
            });

            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = configuration["ApiName"];
                c.RoutePrefix = "swagger";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Specification 1");
            });
        }
        public static void AddSwaggerAuthentication(this SwaggerGenOptions swaggerGenOptions, IConfiguration configuration)
        {

                    swaggerGenOptions.AddSwaggerJWTAuthentication();
       
        }

        private static void AddSwaggerJWTAuthentication(this SwaggerGenOptions swaggerGenOptions)
        {
            swaggerGenOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        }
    }
}
