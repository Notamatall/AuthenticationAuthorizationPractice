using Authentication_Basics.AuthrorizationRequirments;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authentication_Basics.AuthrorizationRequirements
{

    public class MultipleRequirementsHandler : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            var pendingRequirements = context.PendingRequirements.ToList();

            foreach (var requirement in pendingRequirements)
            {
                if (requirement is CustomRequirementClaim)
                {
                    if (IsOwner(context.User, context.Resource)
                        || IsSponsor(context.User, context.Resource))
                    {
                        context.Succeed(requirement);
                    }
                }
                else if (requirement is SecurityLevelRequirement)
                {
                    if (IsOwner(context.User, context.Resource))
                    {
                        context.Succeed(requirement);
                    }
                }
            }

            return Task.CompletedTask;
        }

        private static bool IsOwner(ClaimsPrincipal user, object? resource)
        {
            // Code omitted for brevity
            return true;
        }

        private static bool IsSponsor(ClaimsPrincipal user, object? resource)
        {
            // Code omitted for brevity
            return true;
        }
    }
}
