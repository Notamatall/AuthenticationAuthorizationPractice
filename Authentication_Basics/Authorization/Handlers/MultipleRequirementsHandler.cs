using Authentication_Basics.AuthrorizationRequirments;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
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
                    if (HasCustomRequirement(context, (CustomRequirementClaim)requirement))
                    {
                        context.Succeed(requirement);
                    }
                }
                else if (requirement is SecurityLevelRequirement)
                {
                    if (IsEnoughSecurityhLevelRequirement(context, (SecurityLevelRequirement)requirement))
                    {
                        context.Succeed(requirement);
                    }
                }
            }

            return Task.CompletedTask;
        }

        private static bool HasCustomRequirement(AuthorizationHandlerContext context, CustomRequirementClaim requirement)
        {
            return context.User.Claims.Any(x => x.Type == requirement.ClaimType);
        }

        private static bool IsEnoughSecurityhLevelRequirement(AuthorizationHandlerContext context, SecurityLevelRequirement requirement)
        {
            return requirement.Level <= Convert
                .ToInt32(context.User.Claims
                .FirstOrDefault(x => x.Type == SecurityLevelAuthorizationHandler.RootClaimType)?.Value ?? "0");
        }

    }
}
