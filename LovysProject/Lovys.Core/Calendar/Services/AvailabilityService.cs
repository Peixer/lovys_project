using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lovys.Core.Calendar.DTO;
using Lovys.Core.Calendar.Entities;
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
            var startTimeIsAM = IsAM(availability.StartTime);
            var endIsTimeAM = IsAM(availability.EndTime);
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

        private bool IsAM(string time)
        {
            return time.Contains("am");
        }

        private int GetHours(string time)
        {
            return Convert.ToInt16(Regex.Replace(time, @"\D", ""));
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

        public List<string> SplitRangeHours(Availability availability)
        {
            List<string> hoursToReturn = new List<string>();

            var startTimeIsAM = IsAM(availability.StartTime);
            var endIsTimeAM = IsAM(availability.EndTime);
            var hourStart = GetHours(availability.StartTime);
            var hourEnd = GetHours(availability.EndTime);
            if (!startTimeIsAM)
                hourStart += 12;
            if (!endIsTimeAM)
                hourEnd += 12;

            DateTime startDate = new DateTime(2020, 1, 1, hourStart, 00, 00, DateTimeKind.Local);
            DateTime endDate = new DateTime(2020, 1, 1, hourEnd, 00, 00, DateTimeKind.Local);

            while (startDate.Hour != endDate.Hour)
            {
                hoursToReturn.Add(startDate.ToString("htt").ToLower());
                startDate = startDate.AddHours(1);
            }

            return hoursToReturn;
        }

        public List<HourAvailability> GetHoursAvailabilities(List<Availability> availabilitiesCandidate,
            List<Availability> availabilitiesInterviewers)
        {
            List<HourAvailability> hoursFree = new List<HourAvailability>();
            availabilitiesCandidate.ForEach(availability =>
            {
                var hours = SplitRangeHours(availability);

                foreach (var hour in hours)
                {
                    hoursFree.Add(new HourAvailability()
                    {
                        Hour = hour, User = availability.User, DayOfWeek = availability.DayOfWeek
                    });
                }
            });

            availabilitiesInterviewers.ForEach(availability =>
            {
                var hours = SplitRangeHours(availability);

                foreach (var hour in hours)
                {
                    hoursFree.Add(new HourAvailability()
                    {
                        Hour = hour,
                        User = availability.User,
                        DayOfWeek = availability.DayOfWeek
                    });
                }
            });

            return hoursFree;
        }

        public List<HourAvailability> GetMatchesFromAvailabilities(List<HourAvailability> hoursAvailabilities)
        {
            var groupBy = hoursAvailabilities.GroupBy(x => x.KeyGroup).ToList();

            var matches = groupBy.Where(x =>
                    x.Count() > 1 && x.Any(y => y.User.Role == UserRole.Candidate) &&
                    x.Any(y => y.User.Role == UserRole.Interviewer))
                .Select(group => group.First())
                .ToList();

            return matches;
        }
    }
}