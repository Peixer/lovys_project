using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("interviewers")]
    public class InterviewerController : ControllerBase
    {
        [Authorize(Roles = "Candidate")]
        [HttpPost]
        public string Post()
            {
            //Add interviewers availabilities
            return "sucess";
        }
    }
}
