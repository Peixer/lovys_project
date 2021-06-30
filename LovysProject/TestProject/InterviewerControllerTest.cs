using NUnit.Framework;
using WebApp.Controllers;

namespace TestProject
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void should_verify_get_method()
        {
            var controller = new InterviewerController();
            var result = controller.Get();

            result.Equals("sucess");
        }
    }
}