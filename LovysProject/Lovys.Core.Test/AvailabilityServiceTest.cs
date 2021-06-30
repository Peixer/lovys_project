using System.Collections.Generic;
using System.Threading.Tasks;
using Lovys.Core.Calendar.Entities;
using Lovys.Core.Calendar.Repositories;
using Lovys.Core.Calendar.Services;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace Lovys.WebApp.Test
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
    }
}