using Microsoft.AspNetCore.Http;
using System.Security.Principal;

namespace Authentication_Basics
{
    public static class HttpContextExtensions
    {
        public static bool TryGetIdentity(this HttpContext context, out IIdentity? identity)
        {
            identity = context.User.Identity;

            return identity != null;
        }
    }
}
