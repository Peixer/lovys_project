using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("candidates")]
    public class CandidateController : ControllerBase
    {
        [HttpPost]
        public string Post()
        {
            //Add candidate availabilities
            return "sucess";
        }
        [HttpGet]
        public string Get()
        {
            //Get collection of possible interview
            return "sucess";
        }
    }
}
