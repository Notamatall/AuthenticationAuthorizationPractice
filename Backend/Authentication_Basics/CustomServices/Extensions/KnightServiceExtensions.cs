using API.CustomServices.KnightService;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace API.Extensions
{
    public static class KnightServiceExtensions
    {
        public static IServiceCollection AddKnightService(this IServiceCollection services, Action<KnightOptions> knightOptions)
        {
            services.AddScoped<IKnightFactory, KnightFactory>();
            services.AddOptions<KnightOptions>();
            services.Configure(knightOptions);

            return services;
        }
    }
}
