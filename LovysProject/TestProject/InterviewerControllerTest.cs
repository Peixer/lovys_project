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
        public void should_verify_post_method()
        {
            var controller = new InterviewerController();
            var result = controller.Post();

            result.ShouldBe("sucess");
        }
    }
}