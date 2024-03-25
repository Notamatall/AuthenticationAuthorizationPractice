using Authentication_Basics.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication_Basics.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("[action]")]
        [Authorize(Policy = PoliciesList.PolicyAvarageSecurityLevel)]
        public IActionResult TestMultipleRequirementAuthorizationHandler()
        {
            return Ok(" MultipleRequirementAuthorizationHandler");
        }

    }
}
