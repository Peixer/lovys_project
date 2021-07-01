using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lovys.Core.Calendar.DTO;
using Lovys.Core.Calendar.Entities;
using Lovys.Core.Calendar.Repositories;
using Lovys.Core.Calendar.Services;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace Lovys.Core.Test
{
    public class AvailabilityServiceTest
    {
        private Mock<IAvailabilityRepository> availabilityRepositoryMock;
        private Mock<IUserRepository> userRepositoryMock;
        private AvailabilityService availabilityService;

        [SetUp]
        public void Setup()
        {
            availabilityRepositoryMock = new Mock<IAvailabilityRepository>();
            userRepositoryMock = new Mock<IUserRepository>();

            availabilityRepositoryMock.Setup(x => x.Find(It.IsAny<List<string>>()))
                .Returns(Task.FromResult(new List<Availability>()
                {
                    new Availability(),
                    new Availability(),
                    new Availability()
                }));

            availabilityService = new AvailabilityService(availabilityRepositoryMock.Object,
                userRepositoryMock.Object);
        }

        [Test]
        public async Task should_return_availabilities()
        {
            var availabilities = await availabilityService.GetAvailabilitiesByUserId(new List<string>() {"abc123"});

            availabilities.Count.ShouldBeGreaterThan(0);
        }

        [Test]
        public async Task should_split_range_hours()
        {
            var hours = availabilityService.SplitRangeHours(
                new Availability() {EndTime = "5pm", StartTime = "6am"});

            hours.Any(x => x == "6am").ShouldBeTrue();
            hours.Any(x => x == "7am").ShouldBeTrue();
            hours.Any(x => x == "8am").ShouldBeTrue();
            hours.Any(x => x == "9am").ShouldBeTrue();
            hours.Any(x => x == "10am").ShouldBeTrue();
            hours.Any(x => x == "11am").ShouldBeTrue();
            hours.Any(x => x == "12pm").ShouldBeTrue();
            hours.Any(x => x == "1pm").ShouldBeTrue();
            hours.Any(x => x == "2pm").ShouldBeTrue();
            hours.Any(x => x == "3pm").ShouldBeTrue();
            hours.Any(x => x == "4pm").ShouldBeTrue();
        }

        [Test]
        public async Task should_return_range_hours_availabilities()
        {
            User userInterviewer = new User() {Role = UserRole.Interviewer, Name = "Interview"};
            User userCandidate = new User() {Role = UserRole.Candidate, Name = "Candidate"};

            var availabilitiesInterviewers = new List<Availability>();
            availabilitiesInterviewers.Add(new Availability()
                {EndTime = "10pm", StartTime = "6am", User = userInterviewer});

            var availabilitiesCandidates = new List<Availability>();
            availabilitiesCandidates.Add(new Availability()
                {EndTime = "5pm", StartTime = "5am", User = userCandidate});

            var freeHours = availabilityService.GetHoursAvailabilities(availabilitiesCandidates,
                availabilitiesInterviewers);

            freeHours.Any(x => x.Hour == "6am" && x.User.Role == UserRole.Interviewer).ShouldBeTrue();
            freeHours.Any(x => x.Hour == "6am" && x.User.Role == UserRole.Candidate).ShouldBeTrue();
            freeHours.Any(x => x.Hour == "7am" && x.User.Role == UserRole.Interviewer).ShouldBeTrue();
            freeHours.Any(x => x.Hour == "7am" && x.User.Role == UserRole.Candidate).ShouldBeTrue();
            freeHours.Any(x => x.Hour == "9pm" && x.User.Role == UserRole.Interviewer).ShouldBeTrue();
            freeHours.Any(x => x.Hour == "5am" && x.User.Role == UserRole.Candidate).ShouldBeTrue();
        }

        [Test]
        public async Task should_return_matches_hours()
        {
            User userInterviewer = new User() {Role = UserRole.Interviewer, Name = "Interview"};
            User userCandidate = new User() {Role = UserRole.Candidate, Name = "Candidate"};

            var hoursAvailabilities = new List<HourAvailability>()
            {
                new HourAvailability()
                {
                    Hour = "7am",
                    User = userInterviewer,
                    DayOfWeek = DayOfWeek.Friday
                },
                new HourAvailability()
                {
                    Hour = "8am",
                    User = userInterviewer,
                    DayOfWeek = DayOfWeek.Friday
                },
                new HourAvailability()
                {
                    Hour = "7am",
                    User = userCandidate,
                    DayOfWeek = DayOfWeek.Friday
                },
                new HourAvailability()
                {
                    Hour = "8am",
                    User = userCandidate,
                    DayOfWeek = DayOfWeek.Monday
                }
            };
            var matchesFromAvailabilities = availabilityService.GetMatchesFromAvailabilities(hoursAvailabilities);
            matchesFromAvailabilities.Any(x => x.Hour == "7am" && x.DayOfWeek == DayOfWeek.Friday).ShouldBeTrue();
            matchesFromAvailabilities.Count.ShouldBe(1);
        }

        [Test]
        public async Task should_verify_interviewer_availabilities()
        {
            User userInterviewer = new User() {Role = UserRole.Interviewer, Name = "Interview"};
            User userCandidate = new User() {Role = UserRole.Candidate, Name = "Candidate"};

            var hoursAvailabilities = new List<HourAvailability>()
            {
                new HourAvailability()
                {
                    Hour = "7am",
                    User = userCandidate,
                    DayOfWeek = DayOfWeek.Friday
                },
                new HourAvailability()
                {
                    Hour = "8am",
                    User = userInterviewer,
                    DayOfWeek = DayOfWeek.Friday
                },
                new HourAvailability()
                {
                    Hour = "7am",
                    User = userCandidate,
                    DayOfWeek = DayOfWeek.Friday
                },
                new HourAvailability()
                {
                    Hour = "8am",
                    User = userCandidate,
                    DayOfWeek = DayOfWeek.Monday
                },
                new HourAvailability()
                {
                    Hour = "8am",
                    User = userCandidate,
                    DayOfWeek = DayOfWeek.Monday
                }
            };
            var matchesFromAvailabilities = availabilityService.GetMatchesFromAvailabilities(hoursAvailabilities);
            matchesFromAvailabilities.Count.ShouldBe(0);
        }

        [Test]
        public async Task should_verify_candidate_availabilities()
        {
            User userInterviewer = new User() {Role = UserRole.Interviewer, Name = "Interview"};
            User userCandidate = new User() {Role = UserRole.Candidate, Name = "Candidate"};

            var hoursAvailabilities = new List<HourAvailability>()
            {
                new HourAvailability()
                {
                    Hour = "7am",
                    User = userInterviewer,
                    DayOfWeek = DayOfWeek.Friday
                },
                new HourAvailability()
                {
                    Hour = "8am",
                    User = userCandidate,
                    DayOfWeek = DayOfWeek.Friday
                },
                new HourAvailability()
                {
                    Hour = "7am",
                    User = userInterviewer,
                    DayOfWeek = DayOfWeek.Friday
                },
                new HourAvailability()
                {
                    Hour = "8am",
                    User = userInterviewer,
                    DayOfWeek = DayOfWeek.Monday
                },
                new HourAvailability()
                {
                    Hour = "8am",
                    User = userInterviewer,
                    DayOfWeek = DayOfWeek.Monday
                }
            };
            var matchesFromAvailabilities = availabilityService.GetMatchesFromAvailabilities(hoursAvailabilities);
            matchesFromAvailabilities.Count.ShouldBe(0);
        }
    }
}