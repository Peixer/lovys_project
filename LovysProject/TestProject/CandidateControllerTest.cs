using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Calendar.Models;
using Core.Calendar.Repositories;
using Core.Calendar.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Shouldly;
using WebApp.Controllers;

namespace TestProject
{
    public class CandidateControllerTest
    {
        private Mock<IAvailabilityRepository> availabilityRepositoryMock;
        private Mock<IUserRepository> userRepositoryMock;
        private CandidateController controller;

        [SetUp]
        public void Setup()
        {
            availabilityRepositoryMock = new Mock<IAvailabilityRepository>();
            userRepositoryMock = new Mock<IUserRepository>();
            var availabilityServiceMock = new Mock<IAvailabilityService>();

            userRepositoryMock.Setup(x => x.FindUserByUsername(It.IsAny<string>()))
                .Returns(Task.FromResult(new User()));
            availabilityRepositoryMock.Setup(x => x.Insert(It.IsAny<Availability>()))
                .Returns(Task.FromResult(true));

            controller = new CandidateController(availabilityRepositoryMock.Object, userRepositoryMock.Object,
                availabilityServiceMock.Object);
        }

        [Test]
        public async Task should_sucess()
        {
            ObjectResult result = (ObjectResult) await controller.Post(new Availability()
            {
                EndTime = "10pm", StartTime = "09pm", DayOfWeek = DayOfWeek.Monday
            });

            result.Value.ToString().ShouldBe("sucess");
        }

        [Test]
        public async Task should_fail_must_insert_endTime()
        {
            ObjectResult result = (ObjectResult) await controller.Post(new Availability()
            {
                StartTime = "9pm", DayOfWeek = DayOfWeek.Monday
            });

            result.StatusCode.ShouldBe(400);
            result.Value.ToString().ShouldContain("'End Time' must not be empty.");
        }

        [Test]
        public async Task should_fail_wrong_format_endTime()
        {
            ObjectResult result = (ObjectResult) await controller.Post(new Availability()
            {
                EndTime = "19pm", StartTime = "11pm", DayOfWeek = DayOfWeek.Monday
            });

            result.StatusCode.ShouldBe(400);
            result.Value.ToString().ShouldContain("'End Time' is not in the correct format.");
        }

        [Test]
        public async Task should_fail_startTime_less_than_endTime()
        {
            ObjectResult result = (ObjectResult) await controller.Post(new Availability()
            {
                EndTime = "10pm", StartTime = "12pm", DayOfWeek = DayOfWeek.Monday
            });

            result.StatusCode.ShouldBe(400);
            result.Value.ToString().ShouldContain("'Start Time' must be less than '10pm'.");
        }
    }
}