
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;

namespace Authentication_Basics.Filters
{
    public class AuthorizationFilterAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string[] claimValuesToMatch = [];

        public AuthorizationFilterAttribute() { }

        public AuthorizationFilterAttribute(params string[] claimValues) =>
            claimValuesToMatch = claimValues;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (IsUnauthorized(context.HttpContext))
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                return;
            }

            if (IsForbidden(context.HttpContext, claimValuesToMatch))
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                return;
            }
        }

        public static bool IsUnauthorized(HttpContext context)
        {
            return context.User.Claims.Any(c => c.Type == ClaimTypes.Name) == false;
        }


        public static bool IsForbidden(HttpContext context, ICollection<string> claimValuesToMatch)
        {
            return context.User.Claims.Any(c => claimValuesToMatch.Contains(c.Value)) == false;
        }

    }
}
