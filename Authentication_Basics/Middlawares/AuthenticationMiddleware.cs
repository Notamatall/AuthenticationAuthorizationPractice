using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Authentication_Basics.Middlawares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate next;

        public AuthenticationMiddleware(RequestDelegate next) =>
            this.next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated)
                FinishAuthentication(context);

            await next(context);
        }

        private static void FinishAuthentication(HttpContext context)
        {
     
        }
    }
}
