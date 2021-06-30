using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LovysProject.Controllers
{
    [ApiController]
    [Route("interviewers")]
    public class InterviewerController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "sucess";
        }
    }
}