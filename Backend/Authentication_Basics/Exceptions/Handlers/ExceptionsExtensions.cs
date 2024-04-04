using Microsoft.Extensions.DependencyInjection;

namespace Authentication_Basics.ExceptionsHandlers
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
