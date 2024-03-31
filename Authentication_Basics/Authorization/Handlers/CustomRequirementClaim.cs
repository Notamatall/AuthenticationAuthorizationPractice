using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication_Basics.AuthrorizationRequirments
{
    public class CustomRequirementClaim : IAuthorizationRequirement
    {
        public CustomRequirementClaim(string claimType)
        {
            ClaimType = claimType;
        }
        public string ClaimType { get; set; }
    }

    public class CustomAuthorizationHandler : AuthorizationHandler<CustomRequirementClaim>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            CustomRequirementClaim requirement)
        {
            var hasClaims = context.User.Claims.Any(x => x.Type == requirement.ClaimType);

            if (hasClaims)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
