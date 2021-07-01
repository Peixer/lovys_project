using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Lovys.Core.Calendar.DTO;
using Lovys.Core.Calendar.Entities;
using Lovys.Core.Calendar.Repositories;
using Lovys.Core.Calendar.Services;
using Lovys.WebApp.Models;
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
        public async Task<IActionResult> FilterPeriods(string id, FilterPeriodsModel filter)
        {
            var userIdsCandidate = new List<string>() {id};
            var availabilitiesCandidate = await _availabilityService.GetAvailabilitiesByUserId(userIdsCandidate, filter.StartDate, filter.EndDate);
            var availabilitiesInterviewers = await _availabilityService.GetAvailabilitiesByUserId(filter.Interviewers, filter.StartDate, filter.EndDate);

            var hourAvailabilities = _availabilityService.GetHoursAvailabilities(availabilitiesCandidate, availabilitiesInterviewers);
            var matches = _availabilityService.GetMatchesFromAvailabilities(hourAvailabilities);
            
            return Ok(matches);
        }
    }
}