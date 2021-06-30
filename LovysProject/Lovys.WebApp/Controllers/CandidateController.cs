using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Lovys.Core.Calendar.Models;
using Lovys.Core.Calendar.Repositories;
using Lovys.Core.Calendar.Services;
using Lovys.WebApp.DTO;
using Lovys.WebApp.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lovys.WebApp.Controllers
{
    [ApiController]
    [Route("candidates")]
    public class CandidateController : ControllerBase
    {
        private readonly IAvailabilityService _availabilityService;
        private readonly IUserRepository _userRepository;

        public CandidateController(IAvailabilityService availabilityService, IUserRepository userRepository)
        {
            _availabilityService = availabilityService;
            _userRepository = userRepository;
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

            if (!_availabilityService.IsValidSlotTime(availability))
            {
                return new BadRequestObjectResult("Start time or end time is incorrect");
            }

            string username = User?.FindFirst(ClaimTypes.Name)?.Value;

            _availabilityService.InsertAvailability(availability, username);
            return Ok("sucess");
        }

        [HttpPost("/{id}/filter")]
        public async Task<IActionResult> FilterPeriods(string id, FilterPeriodsDTO filter)
        {
            //availabilities interviewers
            //availabilities candidate
            //intersection between then

            var availabilitiesCandadite = await _availabilityService.GetAvailabilitiesByUserId(new List<string>() {id});
            var availabilitiesInterviewers = await _availabilityService.GetAvailabilitiesByUserId(filter.Interviewers);


            // create method to split range of hours
            // create new class to make free hour to candidate/interviewers
            // create list of period class
            // order list of period class
            // group items with same DayOfWeek + hour on list of period class 
            
            
            
            
            
            return Ok(await _userRepository.FindUserById(id));
        }
    }
}