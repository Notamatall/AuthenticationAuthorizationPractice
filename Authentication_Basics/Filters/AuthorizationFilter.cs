using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;

namespace Authentication_Basics.Filters
{
    /// <summary>
    /// 
    /// <list type="bullet">
    /// <item>
    /// <see href="https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-8.0#authorization-filters">Authorization filter</see>
    /// </item>
    /// <item>
    ///     <description>Are the first filters run in the filter pipeline.</description>
    /// </item>
    /// <item>
    ///     <description>Control access to action methods.</description>
    /// </item>
    /// <item>
    ///     <description>Have a before method, but no after method.</description>
    /// </item>
    /// <item>
    ///     <term>Do not throw the exception.</term>
    ///     <description>Exception filters will not handle the exception.</description>
    /// </item>
    /// </list>
    /// </summary>
    public class AuthorizationFilterAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string[] claimValuesToMatch = [];

        public AuthorizationFilterAttribute() { }
        public AuthorizationFilterAttribute(string[] claims)
        {
            claimValuesToMatch = claims;
        }

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
            if (claimValuesToMatch.IsNullOrEmpty())
                return false;
            return context.User.Claims.Any(c => claimValuesToMatch.Contains(c.Value)) == false;
        }

    }
}
