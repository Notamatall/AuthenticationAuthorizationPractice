using Authentication_Basics.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication_Basics.AuthrorizationRequirments
{
    public class CustomRequirementClaim : IAuthorizationRequirement
    {
        public CustomRequirementClaim()
        {

        }
    }

    public class CustomAuthorizationHandler : AuthorizationHandler<CustomRequirementClaim>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            CustomRequirementClaim requirement)
        {
            var userDisabled = context.User.Claims.FirstOrDefault(x => x.Type == CustomClaimType.ApplyNamespace("isEnabled"))?.Value?.ToLowerInvariant() == "false";

            if (userDisabled)
                context.Fail();
            else
                context.Succeed(requirement);


            return Task.CompletedTask;
        }
    }
}
