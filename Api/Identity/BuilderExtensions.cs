
using Api.Authentication;
using Api.Controllers.Managers;
using Api.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Elfo.Round.Identity
{
	public static class BuilderExtensions
	{
		public static IIdentityBuilder AddRoundIdentity(this AuthenticationBuilder authBuilder) =>
            AddRoundIdentity(authBuilder, o => { });

		public static IIdentityBuilder AddRoundIdentity(this AuthenticationBuilder authBuilder, Action<IdentityOptions> optionsAccessor)
		{

            authBuilder.Services.Configure(optionsAccessor);

            //authBuilder.Services.AddHttpContextAccessor();
            //authBuilder.Services.AddSingleton<IUserCacheManager, UserCacheManager>();

			authBuilder.Services.AddSingleton<IIdentityService, RoundIdentityService>();

            authBuilder.Services.AddTransient<IIdentityControllerManager, IdentityControllerManager>();

            authBuilder.Services.AddSingleton<IAuthorizationHandler, RoundAuthRequirementHandler>();

            //Have no idea why we need it
            //var identityConfigure = new IdentityConfigure();
            //authBuilder.Services.AddSingleton<IIdentityConfigure>(identityConfigure);

            return new IdentityBuilder(authBuilder);
		}

        public static IIdentityBuilder WithIdentityService<T>(this IIdentityBuilder identityBuilder, ServiceLifetime lifetime = ServiceLifetime.Singleton) where T : class, IIdentityService
        {
            identityBuilder.AuthBuilder.Services.Add(new ServiceDescriptor(typeof(IIdentityService), typeof(T), lifetime));
            return identityBuilder;
        }

        public static IIdentityBuilder WithIdentityService<T>(this IIdentityBuilder identityBuilder, Func<IServiceProvider, object> factory, ServiceLifetime lifetime = ServiceLifetime.Singleton) where T : class, IIdentityService
        {
            identityBuilder.AuthBuilder.Services.Add(new ServiceDescriptor(typeof(IIdentityService), factory, lifetime));
            return identityBuilder;
        }

        public static IApplicationBuilder UseRoundIdentity(this IApplicationBuilder builder)
		{
			builder.UseMiddleware<Api.Authentication.AuthenticationMiddleware>();

            var serviceProvider = builder.ApplicationServices.GetService<IServiceProvider>();
            //var identityConfigure = serviceProvider.GetService<IIdentityConfigure>();

            //foreach (var item in identityConfigure.ActionsToDo)
            //{
            //    item(builder);
            //}

            return builder;
		}
        public interface IIdentityBuilder
        {
            AuthenticationBuilder AuthBuilder { get; }
        }
    }
}
