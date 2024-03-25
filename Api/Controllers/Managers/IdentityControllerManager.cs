using Api.Authentication;
using Microsoft.AspNetCore.Http;

namespace Api.Controllers.Managers
{

    /// <summary>
    /// Separates the request to data
    /// </summary>
    /// 
    public class IdentityControllerManager : IIdentityControllerManager
    {
        private readonly IIdentityService identityService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public IdentityControllerManager(IIdentityService identityService, IHttpContextAccessor httpContextAccessor)
        {
            this.identityService = identityService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public object GetUserInformation()
        {
            var result = new UserInformationResult();
            result.CurrentUser = identityService.GetUserInformation(this.httpContextAccessor.HttpContext.User.Identity.Name);
            return result;
        }
    }

    public interface IIdentityControllerManager
    {
        object GetUserInformation();
    }

    public class UserInformationResult
    {
        public IRoundUserModel CurrentUser { get; set; }
    }
}
