using Authentication_Basics.AuthrorizationRequirments;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authentication_Basics.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private IConfiguration configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet("[action]")]
        public async Task<string> GenerateJWTToken([FromQuery] string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Authentication:AuthSecret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(DynamicPolicies.SecurityLevel,"6")
                }),
                Expires = DateTime.UtcNow.Add(TimeSpan.FromDays(7)),
                Audience = "",
                Issuer = "",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CreateCookie()
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
                new Claim(DynamicPolicies.SecurityLevel,"7"),
                new Claim("Hello","hel"),
                new Claim(ClaimTypes.Role,"Admin"),
            };

            var grandmaIdentity = new ClaimsIdentity(grandmaClaims, "Grandma Identity");
            var licenseIdentity = new ClaimsIdentity(licenseClaims, "Government");

            var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity, licenseIdentity });


            await HttpContext.SignInAsync("cookie", userPrincipal);

            return Ok();
        }


    }
}
