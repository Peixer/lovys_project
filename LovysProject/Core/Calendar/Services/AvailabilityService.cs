using System;
using System.Text.RegularExpressions;
using Core.Calendar.Models;
using Core.Calendar.Repositories;

namespace Core.Calendar.Services
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
            var startIsAM = availability.StartTime.Contains("am");
            var endIsAM = availability.EndTime.Contains("am");
            var hourStart = Convert.ToInt16(Regex.Replace(availability.StartTime, @"\D", ""));
            var hourEnd = Convert.ToInt16(Regex.Replace(availability.EndTime, @"\D", ""));

            if (!startIsAM && endIsAM)
                return false;

            if (endIsAM == startIsAM)
            {
                if (hourEnd < hourStart)
                    return false;
            }

            if (hourEnd > 12 || hourStart > 12)
                return false;

            return true;
        }
    }
}