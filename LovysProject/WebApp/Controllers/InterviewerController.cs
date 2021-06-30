using System.Security.Claims;
using System.Threading.Tasks;
using Core.Calendar.Models;
using Core.Calendar.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Validators;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("interviewers")]
    public class InterviewerController : ControllerBase
    {
        private readonly IAvailabilityRepository _availabilityRepository;
        private readonly IUserRepository _userRepository;

        public InterviewerController(IAvailabilityRepository availabilityRepository, IUserRepository userRepository)
        {
            _availabilityRepository = availabilityRepository;
            _userRepository = userRepository;
        }

        [Authorize(Roles = "Interviewer")]
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

            availability.User = await this._userRepository.FindUserByUsername(username);
            await _availabilityRepository.Insert(availability);
            return Ok("sucess");
        }
        
        [Authorize(Roles = "Interviewer")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _availabilityRepository.Find());
        }
    }
}