using System.Threading.Tasks;
using Lovys.Core.Calendar.Models;
using Lovys.Core.Calendar.Repositories;
using Lovys.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace Lovys.WebApp.Test
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
            ObjectResult result =
                (ObjectResult) await controller.Post(new User() {Name = "Test", Role = UserRole.Candidate});

            result.Value.ShouldBe("sucess");
            result.StatusCode.ShouldBe(200);
        }

        [Test]
        public async Task should_fail_insert_new_user()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.Insert(It.IsAny<User>())).Returns(Task.FromResult(false));

            var controller = new UserController(userRepositoryMock.Object);
            ObjectResult result = (ObjectResult) await controller.Post(new User() {Role = UserRole.Candidate});

            result.StatusCode.ShouldBe(400);
            result.Value.ToString().ShouldContain("'Name' must not be empty.");
        }

        [Test]
        public async Task should_fail_login_username_or_password_incorrect()
        {
            var userRepositoryMock = new Mock<IUserRepository>();

            var controller = new UserController(userRepositoryMock.Object);
            ObjectResult result = (ObjectResult) await controller.Login(new User()
                {Username = "teste", Password = "teste", Role = UserRole.Candidate});

            result.StatusCode.ShouldBe(400);
            result.Value.ToString().ShouldContain("Username or password is incorrect");
        }
        
        [Test]
        public async Task should_fail_login_must_insert_username()
        {
            var userRepositoryMock = new Mock<IUserRepository>();

            var controller = new UserController(userRepositoryMock.Object);
            ObjectResult result = (ObjectResult) await controller.Login(new User() {Password = "teste2", Role = UserRole.Candidate});

            result.StatusCode.ShouldBe(400);
            result.Value.ToString().ShouldContain("'Username' must not be empty.");
        }

        [Test]
        public async Task should_sucess_login()
        {
            User user = new User() {Username = "teste", Password = "teste", Role = UserRole.Candidate};

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.FindUser(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(user));

            var controller = new UserController(userRepositoryMock.Object);
            ObjectResult result = (ObjectResult) await controller.Login(user);

            result.StatusCode.ShouldBe(200);
            result.Value.ToString().ShouldContain("token = ");
        }
    }
}