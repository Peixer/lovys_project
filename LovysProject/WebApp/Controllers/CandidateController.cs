using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("candidates")]
    public class CandidateController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "sucess";
        }
    }
}