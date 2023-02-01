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
    public class PlatformTypeControllerTests
    {
        private readonly Mock<IGameService> _gameServiceMock;
        private readonly Mock<IPlatformTypeService> _platformTypeServiceMock;
        private readonly PlatformTypeController _controller;

        public PlatformTypeControllerTests()
        {
            _gameServiceMock = new Mock<IGameService>();
            _platformTypeServiceMock = new Mock<IPlatformTypeService>();
            var mapperMock = new Mock<IMapper>();
            _controller = new PlatformTypeController(_gameServiceMock.Object, _platformTypeServiceMock.Object);
            mapperMock.Setup(m => m.Map<PlatformTypeDTO, PlatformTypeViewModel>(It.IsAny<PlatformTypeDTO>()))
                .Returns(new PlatformTypeViewModel());
            mapperMock.Setup(m => m.Map<PlatformTypeViewModel, PlatformTypeDTO>(It.IsAny<PlatformTypeViewModel>()))
                .Returns(new PlatformTypeDTO());
        }

        [Fact]
        public void GetPlatformTypesGet_SomeData_ReturnsViewResult()
        {
            var result = _controller.Index();

            Assert.NotNull(result);
        }

        [Fact]
        public void PlatformTypeDetails_NonExistentId_ReturnsRedirectResult()
        {
            var ex = Assert.Throws<HttpException>(() => _controller.PlatformTypeDetails(It.IsAny<int>()));

            Assert.NotNull(ex);
        }

        [Fact]
        public void PlatformTypeDetails_ExistentId_ReturnsViewResult()
        {
            _platformTypeServiceMock.Setup(g => g.GetById(1)).Returns(new PlatformTypeDTO() { Id = 1 });
            _gameServiceMock.Setup(g => g.GetByPlatformType(1)).Returns(Mock.Of<List<GameDTO>>());

            var result = _controller.PlatformTypeDetails(1) as ViewResult;
            var model = result.Model as PlatformTypeDTO;

            Assert.Equal(1, model.Id);
        }
    }
}