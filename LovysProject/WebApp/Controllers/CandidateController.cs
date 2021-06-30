using System.Security.Claims;
using System.Threading.Tasks;
using Core.Calendar.Models;
using Core.Calendar.Repositories;
using Core.Calendar.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Validators;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("candidates")]
    public class CandidateController : ControllerBase
    {
        private readonly IAvailabilityRepository _availabilityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAvailabilityService _availabilityService;

        public CandidateController(IAvailabilityRepository availabilityRepository, IUserRepository userRepository,
            IAvailabilityService availabilityService)
        {
            _availabilityRepository = availabilityRepository;
            _userRepository = userRepository;
            _availabilityService = availabilityService;
        }

        [Authorize(Roles = "Candidate")]
        [HttpPost]
        public async Task<IActionResult> Post(Availability availability)
        {
            var validator = new AvailabilityValidator();
            var validRes = await validator.ValidateAsync(availability);
            if (!validRes.IsValid)
            {
                return new BadRequestObjectResult(validRes.ToString(","));
            }

            string username = User?.FindFirst(ClaimTypes.Name)?.Value;

            _availabilityService.InsertAvailability(availability, username);
            return Ok("sucess");
        }

        [HttpGet]
        public string Get()
        {
            //Get collection of possible interview
            return "sucess";
        }
    }
}