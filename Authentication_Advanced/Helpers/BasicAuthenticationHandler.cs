using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApi.Entities;
using WebApi.Services;

namespace WebApi.Helpers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserService _userService;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserService userService)
            : base(options, logger, encoder, clock)
        {
            _userService = userService;
        }
        private static ControllerActionDescriptor GetControllerByUrl(HttpContext httpContext)
        {
            var pathElements = httpContext.Request.Path.ToString().Split("/").Where(m => m != "");
            string controllerName = (pathElements.ElementAtOrDefault(0) == "" ? null : pathElements.ElementAtOrDefault(0)) ?? "w";
            string actionName = (pathElements.ElementAtOrDefault(1) == "" ? null : pathElements.ElementAtOrDefault(1)) ?? "Index";

            var actionDescriptorsProvider = httpContext.RequestServices.GetRequiredService<IActionDescriptorCollectionProvider>();
            ControllerActionDescriptor controller = actionDescriptorsProvider.ActionDescriptors.Items
            .Where(s => s is ControllerActionDescriptor bb
                        && bb.ActionName.ToLower() == actionName.ToLower()
                        && bb.ControllerName.ToLower() == controllerName.ToLower()
                        && (bb.ActionConstraints == null
                            || (bb.ActionConstraints != null
                                && bb.ActionConstraints.Any(x => x is HttpMethodActionConstraint cc
                                && cc.HttpMethods.Any(m => m.ToLower() == httpContext.Request.Method.ToLower())))))
            .Select(s => s as ControllerActionDescriptor)
            .FirstOrDefault();
            return controller;
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // skip authentication if endpoint has [AllowAnonymous] attribute
            ControllerActionDescriptor controller = GetControllerByUrl(Context);
            var countryCodeAttribute = controller?.MethodInfo.GetCustomAttribute<AllowAnonymousAttribute>();

            if (countryCodeAttribute != null)
                return AuthenticateResult.NoResult();

            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            User user = null;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                var username = credentials[0];
                var password = credentials[1];
                user = await _userService.Authenticate(username, password);
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            if (user == null)
                return AuthenticateResult.Fail("Invalid Username or Password");

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier,"15"),
                new Claim(ClaimTypes.Name, "ivan"),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var identity2 = new ClaimsIdentity(claims, "Cookie auth");
            var principal = new ClaimsPrincipal(new List<ClaimsIdentity> { identity, identity2 });
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}