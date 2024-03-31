using Authentication_Basics.Authentication.JWT;
using Authentication_Basics.Controllers.Queries;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authentication_Basics.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private IConfiguration configuration;
        private JWTTokenFactory jwtTokenFactory;

        public AuthenticationController(IConfiguration configuration, JWTTokenFactory jwtTokenFactory)
        {
            this.configuration = configuration;
            this.jwtTokenFactory = jwtTokenFactory;
        }

        [HttpPost("[action]")]
        public IActionResult GenerateJWTToken([FromBody] GenerateJWTTokenModel model)
        {
            var token = jwtTokenFactory.GenerateToken(model.UserName);
            return Ok(token);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CreateCookie()
        {

            var grandmaClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Bob"),
            };

            var licenseClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.DateOfBirth,"11/11/2000"),
                new Claim("DrivingLicense","A+"),
            };

            var grandmaIdentity = new ClaimsIdentity(grandmaClaims, "Grandma Identity");
            var licenseIdentity = new ClaimsIdentity(licenseClaims, "Government");

            var userPrincipal = new ClaimsPrincipal([grandmaIdentity, licenseIdentity]);

            await HttpContext.SignInAsync("cookie", userPrincipal);

            return Ok();
        }

    }
}
