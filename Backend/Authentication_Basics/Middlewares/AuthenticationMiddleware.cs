using Authentication_Basics.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Authentication_Basics.Middlewares
{

    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate next;

        public AuthenticationMiddleware(RequestDelegate next) =>
            this.next = next;

        public async Task InvokeAsync(HttpContext context, IIdentityService identityService, IOptions<IdentityOptions> options)
        {
            if (context.User.Identity.IsAuthenticated)
                FinishAuthentication(context, identityService, options.Value);

            await next(context);
        }

        private static void FinishAuthentication(HttpContext context, IIdentityService identityService,
            IdentityOptions options)
        {
            var user = identityService.GetUserInformation(context.User.Identity.Name);

            if (user != default)
            {
                var userIdentity = identityService.CreateUserIdentity(user);
                context.User.AddIdentity(userIdentity);
            }
        }
    }

    public class FactoryBasedAuthenticationMiddleware : IMiddleware
    {
        private readonly IIdentityService identityService;

        public FactoryBasedAuthenticationMiddleware(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        private async Task FinishAuthentication(HttpContext context)
        {

            if (context.TryGetIdentity(out IIdentity? identity))
            {
                var user = identityService.GetUserInformation(identity!.Name!);

                if (user == default || !user.IsEnabled)
                {
                    context.User = new ClaimsPrincipal(new ClaimsIdentity());
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized");
                }
                else
                {
                    var userIdentity = identityService.CreateUserIdentity(user);
                    //   context.User.AddIdentity(userIdentity);
                }
            }

        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
                await FinishAuthentication(context);

            await next(context);
        }
    }

    //public class SampleAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    //{
    //    private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();
    //    private readonly IIdentityService identityService;
    //    public SampleAuthorizationMiddlewareResultHandler(IIdentityService identityService)
    //    {
    //        this.identityService = identityService;
    //    }
    //    public async Task HandleAsync(
    //        RequestDelegate next,
    //        HttpContext context,
    //        AuthorizationPolicy policy,
    //        PolicyAuthorizationResult authorizeResult)
    //    {
    //        if (context.TryGetIdentity(out IIdentity? identity))
    //        {
    //            var user = identityService.GetUserInformation(identity!.Name!);

    //            if (user == default || !user.IsEnabled)
    //                context.User = new ClaimsPrincipal(new ClaimsIdentity());
    //            else
    //            {
    //                var userIdentity = identityService.CreateUserIdentity(user);
    //                context.User.AddIdentity(userIdentity);
    //            }
    //        }


    //        // Fall back to the default implementation.
    //        await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    //    }
    //}

    public static class MiddlewareExtensions
    {

        public static IApplicationBuilder UseFactoryActivatedMiddleware(
            this IApplicationBuilder app)
            => app.UseMiddleware<FactoryBasedAuthenticationMiddleware>();

        public static IApplicationBuilder UseConvetionalActivatedMiddleware(
            this IApplicationBuilder app)
            => app.UseMiddleware<AuthenticationMiddleware>();
    }


}
