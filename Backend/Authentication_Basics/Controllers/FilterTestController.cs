using API.Constants;
using API.Filters;
using API.Mocks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FilterTestController : ControllerBase
    {
        private readonly IUserService userService;
        public FilterTestController(IUserService userService)
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

        [HttpGet("[action]")]
        [TypeFilter<AuthorizationFilterAttribute>]
        public IActionResult TestAuthorizationFilter()
        {
            return Ok("Successfuly returned after AuthorizationFilter");
        }

        [HttpGet("[action]")]
        [TypeFilter<AuthorizationFilterAttribute>]
        [ResourceFilter]
        public IActionResult TestResourceFilter()
        {
            return Ok("Successfuly returned after ResourceFilter");
        }

        [HttpGet("[action]")]
        [ActionFilter]
        public IActionResult TestActionFilter()
        {
            return Ok("Successfuly returned after ActionFilter");
        }

        [HttpGet("[action]")]
        [TypeFilter<ExceptionFilter>]
        [ActionFilter]
        public IActionResult TestExceptionFilter()
        {
            throw new Exception("Exception in action with ExceptionFilter");
        }

        [HttpGet("[action]")]
        [TypeFilter<ExceptionFilter>]
        [ResourceFilter(false)]
        [ResultFilter]
        [ActionFilter]
        public IActionResult TestAllFilters()
        {
            return Ok("Successfuly returned after TestAllFilters");
        }
    }
}
