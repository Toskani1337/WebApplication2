using Microsoft.AspNetCore.Mvc;
using WebApplication.Controllers;
using WebApplication.Models;
using Xunit;
 
namespace UnitTestApp.Tests
{
    public class HomeControllerTests
    {

        [Fact]
        public void IndexViewDataMessage()
        {

            HomeTestController controller = new HomeTestController();
 
            // Act
            ViewResult result = controller.Index() as ViewResult;
 
            // Assert
            Assert.Equal("Hello world!", result?.ViewData["Message"]);
        }

        [Fact]
        public void IndexViewDataMessage2()
        {

            HomeTestController controller = new HomeTestController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.Equal("Test!", result?.ViewData["Message2"]);
        }

        [Fact]
        public void IndexViewDataMessage3()
        {

            HomeTestController controller = new HomeTestController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.Equal("SyperTest!", result?.ViewData["Message3"]);
        }

        [Fact]
        public void IndexViewDataMessage4()
        {

            HomeTestController controller = new HomeTestController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.Equal("MegaTest!", result?.ViewData["Message4"]);
        }

        [Fact]
        public void IndexViewDataMessage5()
        {

            HomeTestController controller = new HomeTestController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.Equal("TopTest!", result?.ViewData["Message5"]);
        }

        [Fact]
        public void IndexViewResultNotNull()
        {
            // Arrange
            HomeTestController controller = new HomeTestController();
            // Act
            ViewResult result = controller.Index() as ViewResult;
            // Assert
            Assert.NotNull(result);
        }
 
        [Fact]
        public void IndexViewNameEqualIndex()
        {
            // Arrange
            HomeTestController controller = new HomeTestController();
            // Act
            ViewResult result = controller.Index() as ViewResult;
            // Assert
            Assert.Equal("Index", result?.ViewName);
        }
    }
}