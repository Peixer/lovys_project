using System;
using System.Threading.Tasks;
using Core.Calendar.Models;
using Core.Calendar.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Shouldly;
using WebApp.Controllers;

namespace TestProject
{
    public class InterviewerControllerTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task should_sucess()
        {
            var availabilityRepositoryMock = new Mock<IAvailabilityRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var controller = new InterviewerController(availabilityRepositoryMock.Object, userRepositoryMock.Object);
            ObjectResult result = (ObjectResult) await controller.Post(new Availability()
            {
                EndTime = "10pm", StartTime = "9pm", DayOfWeek = DayOfWeek.Monday
            });

            result.Value.ToString().ShouldBe("sucess");
        }

        [Test]
        public async Task should_fail_must_insert_endTime()
        {
            var availabilityRepositoryMock = new Mock<IAvailabilityRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var controller = new InterviewerController(availabilityRepositoryMock.Object, userRepositoryMock.Object);
            ObjectResult result = (ObjectResult) await controller.Post(new Availability()
            {
                StartTime = "9pm", DayOfWeek = DayOfWeek.Monday
            });

            result.StatusCode.ShouldBe(400);
            result.Value.ToString().ShouldContain("'End Time' must not be empty.");
        }
    }
}