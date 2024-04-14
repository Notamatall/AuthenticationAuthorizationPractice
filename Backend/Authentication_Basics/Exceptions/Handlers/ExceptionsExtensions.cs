using Microsoft.Extensions.DependencyInjection;

namespace API.ExceptionsHandlers
{
    public static class ExceptionsExtensions
    {
        public static IServiceCollection RegisterExceptionHandlers(this IServiceCollection services)
        {
            services.AddExceptionHandler<ValidationExceptionHandler>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            return services;
        }
    }
}
