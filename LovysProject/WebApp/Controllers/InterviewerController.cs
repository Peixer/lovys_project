using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("interviewers")]
    public class InterviewerController : ControllerBase
    {
        [HttpPost]
        public string Post()
        {
            //Add interviewers availabilities
            return "sucess";
        }
    }
}
