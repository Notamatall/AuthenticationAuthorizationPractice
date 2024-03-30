using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Authentication_Basics.LoggerExtensions
{
    public static class LoggerExtensions
    {
        public static IServiceCollection AddSerilog(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddSingleton<Serilog.ILogger>(sp =>
             {
                 return new LoggerConfiguration()
                        .ReadFrom.Configuration(configuration)
                        .CreateLogger();
             });
        }
    }
}
