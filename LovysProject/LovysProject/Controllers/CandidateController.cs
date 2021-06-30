using Microsoft.AspNetCore.Mvc;

namespace LovysProject.Controllers
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