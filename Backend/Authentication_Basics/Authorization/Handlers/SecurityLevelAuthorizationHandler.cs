using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.AuthrorizationRequirements
{
    public class SecurityLevelRequirement : IAuthorizationRequirement
    {
        public int Level { get; }
        public SecurityLevelRequirement(int level)
        {
            Level = level;
        }
    }

    public class SecurityLevelAuthorizationHandler : AuthorizationHandler<SecurityLevelRequirement>
    {
        public const string RootClaimType = "SecurityLevel";
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            SecurityLevelRequirement requirement)
        {
            var claimValue = Convert
                .ToInt32(context.User.Claims
                .FirstOrDefault(x => x.Type == RootClaimType)?.Value ?? "0");

            if (requirement.Level <= claimValue)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
