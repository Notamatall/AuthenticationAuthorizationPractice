using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authentication_Basics.AuthrorizationRequirments
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
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var hasFriendClaim = principal.Claims.Any(x => x.Type == "Friend");
            if (!hasFriendClaim)
            {
                ((ClaimsIdentity)principal.Identity).AddClaim(new Claim("Friend", "Bad"));
            }
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
