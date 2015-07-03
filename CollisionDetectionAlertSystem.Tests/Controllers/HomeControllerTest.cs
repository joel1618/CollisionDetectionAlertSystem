using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CollisionDetectionAlertSystem;
using CollisionDetectionAlertSystem.Controllers;
using Moq;
using CollisionDetectionAlertSystem.Domain.Interface;

namespace CollisionDetectionAlertSystem.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        //private IMovingObjectService _
        [TestMethod]
        public void Index()
        {
            Mock<IMovingObjectService> mock = new Mock<IMovingObjectService>();
            // Arrange
            HomeController controller = new HomeController(mock.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
