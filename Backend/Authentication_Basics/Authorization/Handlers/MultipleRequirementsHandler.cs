using API.AuthrorizationRequirments;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.AuthrorizationRequirements
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

                    context.Succeed(requirement);

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


        private static bool IsEnoughSecurityhLevelRequirement(AuthorizationHandlerContext context, SecurityLevelRequirement requirement)
        {
            return requirement.Level <= Convert
                .ToInt32(context.User.Claims
                .FirstOrDefault(x => x.Type == SecurityLevelAuthorizationHandler.RootClaimType)?.Value ?? "0");
        }

    }
}
