using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lovys.Core.Calendar.Models;
using Lovys.Core.Calendar.Repositories;

namespace Lovys.Core.Calendar.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly IAvailabilityRepository _availabilityRepository;
        private readonly IUserRepository _userRepository;

        public AvailabilityService(IAvailabilityRepository availabilityRepository, IUserRepository userRepository)
        {
            _availabilityRepository = availabilityRepository;
            _userRepository = userRepository;
        }

        public async void InsertAvailability(Availability availability, string username)
        {
            availability.User = await _userRepository.FindUserByUsername(username);
            await _availabilityRepository.Insert(availability);
        }

        public bool IsValidSlotTime(Availability availability)
        {
            var startTimeIsAM = availability.StartTime.Contains("am");
            var endIsTimeAM = availability.EndTime.Contains("am");
            var hourStart = Convert.ToInt16(Regex.Replace(availability.StartTime, @"\D", ""));
            var hourEnd = Convert.ToInt16(Regex.Replace(availability.EndTime, @"\D", ""));

            if (StartTimeIsGreaterThanEndTime(startTimeIsAM, endIsTimeAM))
                return false;

            if (endIsTimeAM == startTimeIsAM)
            {
                if (hourEnd < hourStart)
                    return false;
            }

            if (TimeSlotsIsGreaterThanTwelveHour(hourEnd, hourStart))
                return false;

            return true;
        }

        private bool StartTimeIsGreaterThanEndTime(bool startTimeIsAM, bool endIsTimeAM)
        {
            return !startTimeIsAM && endIsTimeAM;
        }

        private bool TimeSlotsIsGreaterThanTwelveHour(int hourEnd, int hourStart)
        {
            return hourEnd > 12 || hourStart > 12;
        }

        public async Task<List<Availability>> GetAvailabilitiesByUserId(List<string> userIds)
        {
            return await _availabilityRepository.Find(userIds);
        }
    }
}