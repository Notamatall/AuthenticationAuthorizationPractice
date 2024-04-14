using API.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.AuthrorizationRequirments
{
    public static class DynamicPolicies
    {
        public static IEnumerable<string> Get()
        {
            yield return SecurityLevel;
            yield return Rank;
        }
        public const string SecurityLevel = "SecurityLevel";
        public const string Rank = "Rank";
    }

    public class ClaimsTransformation : IClaimsTransformation
    {
        private IIdentityService identityService;
        private IMemoryCache memoryCache;
        public ClaimsTransformation(
            IIdentityService identityService,
            IMemoryCache memoryCache
            )
        {
            this.identityService = identityService;
            this.memoryCache = memoryCache;
        }
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal.Identity.IsAuthenticated)
            {
                DBUserModel user = null;
                if (memoryCache.TryGetValue(principal.Identity.Name, out var value))
                    user = (DBUserModel)value;
                else
                {
                    var identityUser = identityService.GetUserInformation(principal.Identity.Name);
                    memoryCache.Set(principal.Identity.Name, identityUser);
                    user = (DBUserModel)identityUser;

                }

                if (user != default)
                {
                    var userIdentity = identityService.CreateUserIdentity(user);
                    principal.AddIdentity(userIdentity);
                }
            }
            //var user = identityService.GetUserInformation(principal.Identity.Name);

            //if (user == default || !user.IsEnabled)
            //    principal = new ClaimsPrincipal(new ClaimsIdentity());
            //else
            //{
            //    var userIdentity = identityService.CreateUserIdentity(user);
            //    principal.AddIdentity(userIdentity);
            //}

            return Task.FromResult(principal);
        }
    }

    //public static class DynamicAuthorizationPolicyFactory
    //{
    //    public static AuthorizationPolicy Create(string policyName)
    //    {
    //        var parts = policyName.Split('.');
    //        var type = parts.First();
    //        var value = parts.Last();

    //        switch (type)
    //        {
    //            case DynamicPolicies.Rank:
    //                {
    //                    return new AuthorizationPolicyBuilder()
    //                        .RequireClaim("Rank", value)
    //                         .Build();
    //                }

    //            case DynamicPolicies.SecurityLevel:
    //                {
    //                    return new AuthorizationPolicyBuilder()
    //                        .AddRequirements(new SecurityLevelRequirement(Convert.ToInt32(value)))
    //                         .Build();
    //                }
    //            default:
    //                return null;
    //        }
    //    }
    //}


    ///// <summary>
    ///// DefaultAuthorizationPolicyProvider handle the defining which authorization policy will be used for particular attribute
    ///// In combination with AuthorizationPolicyBuilder allows to create policies dynamically and add claims to it
    ///// </summary>
    //public class CustomAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    //{
    //    IOptions<AuthorizationOptions> options1 { get; set; }
    //    public CustomAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
    //        : base(options)
    //    {
    //        this.options1 = options;
    //    }
    //    public override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    //    {
    //        foreach (var customPolicy in DynamicPolicies.Get())
    //        {
    //            if (policyName.StartsWith(customPolicy))
    //            {
    //                var policy = DynamicAuthorizationPolicyFactory.Create(policyName);
    //                return Task.FromResult(policy);
    //            }
    //        }
    //        return base.GetPolicyAsync(policyName);
    //    }
    //}
}
