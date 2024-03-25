using Authentication_Basics.Constants;
using Authentication_Basics.Mocks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Authentication_Basics.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IUserService userService;
        public TestController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("[action]")]
        [Authorize(Policy = PoliciesList.PolicyAvarageSecurityLevel)]
        public IActionResult TestMultipleRequirementAuthorizationHandler()
        {
            return Ok(" MultipleRequirementAuthorizationHandler");
        }

        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Basic")]
        public async Task<IActionResult> GetAll()
        {
            var users = await userService.GetAll();
            return Ok(users);
        }

    }
}
