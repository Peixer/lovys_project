using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
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

//Add free schedule