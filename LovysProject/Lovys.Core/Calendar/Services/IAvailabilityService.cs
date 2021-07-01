using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lovys.Core.Calendar.DTO;
using Lovys.Core.Calendar.Entities;

namespace Lovys.Core.Calendar.Services
{
    public interface IAvailabilityService
    {
        void InsertAvailability(Availability availability, string username);
        bool IsValidSlotTime(Availability availability);
        Task<List<Availability>> GetAvailabilitiesByUserId(List<string> userIds, DateTime startDate, DateTime endDate);
        List<string> SplitRangeHours(Availability availability);
        List<HourAvailability> GetHoursAvailabilities(List<Availability> availabilitiesCandidate,
            List<Availability> availabilitiesInterviewers);

        List<HourAvailability> GetMatchesFromAvailabilities(List<HourAvailability> hourAvailabilities);
    }
}