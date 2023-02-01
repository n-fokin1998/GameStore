using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.Web.Areas.Manager.ViewModels;
using GameStore.Web.Controllers;
using Moq;
using Xunit;

namespace GameStore.Tests.ControllerTests
{
    public class PublisherControllerTests
    {
        private readonly Mock<IPublisherService> _publisherServiceMock;
        private readonly PublisherController _controller;

        public PublisherControllerTests()
        {
            var gameServiceMock = new Mock<IGameService>();
            _publisherServiceMock = new Mock<IPublisherService>();
            var mapperMock = new Mock<IMapper>();
            _controller = new PublisherController(gameServiceMock.Object, _publisherServiceMock.Object);
            mapperMock.Setup(m => m.Map<PublisherDTO, PublisherViewModel>(It.IsAny<PublisherDTO>()))
                .Returns(new PublisherViewModel());
            mapperMock.Setup(m => m.Map<PublisherViewModel, PublisherDTO>(It.IsAny<PublisherViewModel>()))
                .Returns(new PublisherDTO());
        }

        [Fact]
        public void GetAllPublishers_SomeData_ReturnsViewResult()
        {
            SetData();

            var result = _controller.Index();
            var model = result.Model as IEnumerable<PublisherDTO>;

            Assert.NotEmpty(model);
        }

        [Fact]
        public void PublisherDetails_SomeData_ReturnsModel()
        {
            SetData();

            var result = _controller.PublisherDetails("p1") as ViewResult;

            Assert.NotNull(result.Model);
        }

        [Fact]
        public void PublisherDetails_NonExistentName_ReturnsRedirectResult()
        {
            SetData();

            var ex = Assert.Throws<HttpException>(() => _controller.PublisherDetails(It.IsAny<string>()));

            Assert.NotNull(ex);
        }

        private void SetData()
        {
            _publisherServiceMock.Setup(g => g.GetList())
                .Returns(new List<PublisherDTO>() { new PublisherDTO() { CompanyName = "p1" } });
        }
    }
}