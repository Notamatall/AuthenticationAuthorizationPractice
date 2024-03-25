using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using System.Threading.Tasks;
using WebApi.Models;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;

namespace WebApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<IActionResult> AuthenticateCookie()
        {

            var grandmaClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Bob"),
                new Claim("Grandma.Says","Very nice boy"),
                new Claim(ClaimTypes.Email,"Bob@fmail.com")
            };

            var licenseClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Bob K Foo"),
                new Claim(ClaimTypes.DateOfBirth,"11/11/2000"),
                new Claim("DrivingLicense","A+"),

                new Claim("Hello","hel"),
                new Claim(ClaimTypes.Role,"Admin"),
            };

            var grandmaIdentity = new ClaimsIdentity(grandmaClaims, "Grandma Identity");
            var licenseIdentity = new ClaimsIdentity(licenseClaims, "Government");

            var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity, licenseIdentity });

            await HttpContext.SignInAsync(
                "cookie",
                userPrincipal
                );
            //await HttpContext.SignInAsync(userPrincipal, new AuthenticationProperties() { IsPersistent = true });

            return this.Ok();
        }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateModel model)
        {
            var user = await _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "cookie")]
        public async Task<IActionResult> GetAllCookie()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Windows")]
        public async Task<IActionResult> GetAllWindows()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

    }
}
