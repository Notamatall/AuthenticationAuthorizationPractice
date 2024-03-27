using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication_Basics.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class JWTController : ControllerBase
    {
        [HttpGet("[action]")]
        public IActionResult GetIsAuthenticatedByJWT()
        {
            return Ok("authorized with jwt");
        }

    }
}
