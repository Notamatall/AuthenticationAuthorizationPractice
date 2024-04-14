using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace API.LoggerExtensions
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
