using System.Security.Claims;
using System.Threading.Tasks;
using Lovys.Core.Calendar.Entities;
using Lovys.Core.Calendar.Repositories;
using Lovys.Core.Calendar.Services;
using Lovys.WebApp.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lovys.WebApp.Controllers
{
    [ApiController]
    [Authorize(Roles = "Interviewer")]
    [Route("api/interviewers")]
    public class InterviewerController : ControllerBase
    {
        private readonly IAvailabilityRepository _availabilityRepository;
        private readonly IAvailabilityService _availabilityService;

        public InterviewerController(IAvailabilityRepository availabilityRepository, IAvailabilityService availabilityService)
        {
            _availabilityRepository = availabilityRepository;
            _availabilityService = availabilityService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Availability availability)
        {
            var validator = new AvailabilityValidator();
            var validRes = await validator.ValidateAsync(availability);
            if (!validRes.IsValid)
            {
                return new BadRequestObjectResult(validRes.ToString(","));
            }
            if (!_availabilityService.IsValidSlotTime(availability))
            {
                return new BadRequestObjectResult("Start time or end time is incorrect");
            }

            string username = User?.FindFirst(ClaimTypes.Name)?.Value;

            _availabilityService.InsertAvailability(availability, username);
            return Ok("sucess");
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _availabilityRepository.Find());
        }
    }
}