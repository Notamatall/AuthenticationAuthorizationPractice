using Authentication_Basics.Controllers.DTOs;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Authentication_Basics.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemManagementController : ControllerBase
    {
        private readonly IDbConnection dapper;
        public SystemManagementController(IDbConnection dapper)
        {
            this.dapper = dapper;
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserCreationModel userCreationModel)
        {
            var sql = "insert into users_tb(username,password,languageCode,firstName,lastName,email,isEnabled)" +
                "values(@username,@password,@languageCode,@firstName,@lastName,@email,@isEnabled)";
            dapper.Execute(sql, userCreationModel);
            return Ok();
        }
    }
}
