using System;
using System.Threading.Tasks;
using Lovys.Core.Calendar.Models;
using Lovys.Core.Calendar.Repositories;
using Lovys.Core.Calendar.Services;
using Lovys.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace Lovys.WebApp.Test
{
    public class InterviewerControllerTest
    {
        private Mock<IAvailabilityRepository> availabilityRepositoryMock;
        private Mock<IUserRepository> userRepositoryMock;
        private InterviewerController controller;

        [SetUp]
        public void Setup()
        {
            availabilityRepositoryMock = new Mock<IAvailabilityRepository>();
            userRepositoryMock = new Mock<IUserRepository>();

            userRepositoryMock.Setup(x => x.FindUserByUsername(It.IsAny<string>()))
                .Returns(Task.FromResult(new User()));
            availabilityRepositoryMock.Setup(x => x.Insert(It.IsAny<Availability>()))
                .Returns(Task.FromResult(true));

            controller = new InterviewerController(availabilityRepositoryMock.Object, new AvailabilityService(availabilityRepositoryMock.Object, userRepositoryMock.Object));
        }

        [Test]
        public async Task should_sucess()
        {
            ObjectResult result = (ObjectResult) await controller.Post(new Availability()
            {
                EndTime = "10pm", StartTime = "9pm", DayOfWeek = DayOfWeek.Monday
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
            result.Value.ToString().ShouldContain("Start time or end time is incorrect");
        }

        [Test]
        public async Task should_fail_startTime_less_than_endTime()
        {
            ObjectResult result = (ObjectResult) await controller.Post(new Availability()
            {
                EndTime = "10pm", StartTime = "12pm", DayOfWeek = DayOfWeek.Monday
            });

            result.StatusCode.ShouldBe(400);
            result.Value.ToString().ShouldContain("Start time or end time is incorrect");
        }
    }
}