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
    public class UserControllerTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task should_sucess_insert_new_user()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.Insert(It.IsAny<User>())).Returns(Task.FromResult(true));
            
            var controller = new UserController(userRepositoryMock.Object);
            ObjectResult result = (ObjectResult) await controller.Post(new User(){Name = "Test", Role = UserRole.Candidate});

            result.Value.ShouldBe("sucess");
            result.StatusCode.ShouldBe(200);
        }
        
        [Test]
        public async Task should_fail_insert_new_user()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.Insert(It.IsAny<User>())).Returns(Task.FromResult(false));
            
            var controller = new UserController(userRepositoryMock.Object);
            ObjectResult result = (ObjectResult) await controller.Post(new User(){Role = UserRole.Candidate});

            result.StatusCode.ShouldBe(400);
            result.Value.ToString().ShouldContain("'Name' must not be empty.");
        }
    }
}