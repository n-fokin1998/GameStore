using System.Web.Mvc;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.Web.Controllers;
using Moq;
using Xunit;

namespace GameStore.Tests.ControllerTests
{
    public class ShipperControllerTests
    {
        private readonly ShipperController _controller;

        public ShipperControllerTests()
        {
            var mongoShipperServiceMock = new Mock<IShipperService>();
            _controller = new ShipperController(mongoShipperServiceMock.Object);
        }

        [Fact]
        public void GetAllShippers_SomeData_ReturnsViewResult()
        {
            var result = _controller.Index() as ViewResult;

            Assert.NotNull(result);
        }
    }
}