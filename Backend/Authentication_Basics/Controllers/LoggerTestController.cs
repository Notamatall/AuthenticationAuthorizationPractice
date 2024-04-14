using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Controllers
{
    public class LoggerTestController : ControllerBase
    {
        private readonly ILogger logger;

        public LoggerTestController(ILogger logger)
        {
            this.logger = logger;
        }

        [HttpGet("[action]")]
        public IActionResult TestLogExceptionInsideAction()
        {
            try
            {
                var list = new List<string>();
                list.First();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error.\n\r Message: {0} \n\rStacktrace: {1}", ex.Message, ex.StackTrace);
            }

            return Ok("TestLogExceptionDefault Ok result");
        }

        [HttpGet("[action]")]
        public IActionResult TestLogExceptionWithGlobalHandler()
        {
            var list = new List<string>();
            list.First();

            return Ok("TestLogExceptionDefault Ok result");
        }
    }
}
