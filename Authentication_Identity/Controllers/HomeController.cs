using Authentication_Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authentication_Identity.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailService _emailService;
        public HomeController(UserManager<IdentityUser> userManager
            , SignInManager<IdentityUser> signInManager
            , IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        [HttpGet("[action]")]
        public IActionResult Home()
        {
            return this.Ok("home");
        }


        [HttpGet("[action]")]
        [Authorize]
        public IActionResult Secret()
        {
           
            return this.Ok("secret");
        }

        //[HttpGet("[action]")]
        //public IActionResult Login()
        //{
        //    return RedirectToAction("Home");
        //}


        [HttpPost("[action]")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Home");
                }
            }
            return RedirectToAction("Home");
        }

        //[HttpGet("[action]")]
        //public IActionResult Register()
        //{
        //    return RedirectToAction("home");
        //}

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromQuery] string username, [FromQuery] string password)
        {
            //here we create data in db
            var user = new IdentityUser()
            {
                UserName = username,
                Email = "",

            };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var link = Url.ActionLink(nameof(VerifyEmail), "home", new { userid = user.Id, code = code }, Request.Scheme, Request.Host.ToString());
                //$"http://localhost:8080/loginPage?userId={user.Id}&code={code}";

                await _emailService.SendAsync("gyrskiyy@gmail.com", "email verify", $"<a href=\"{link}\">Verify Email</a>",true);

                return this.Ok();
            }
            return BadRequest("not registered");
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> VerifyEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return BadRequest();
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);

            return this.Redirect("http://localhost:8080/thanksforverify");
        }
        public IActionResult EmailVerification()
        {
            return this.Ok("please verify your email");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Home");
        }
    }
}
