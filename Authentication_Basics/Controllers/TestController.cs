using Authentication_Basics.AuthrorizationRequirments;
using Authentication_Basics.Controllers.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authentication_Basics.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IAuthorizationService authorizationService;
        public TestController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }


    }
}
