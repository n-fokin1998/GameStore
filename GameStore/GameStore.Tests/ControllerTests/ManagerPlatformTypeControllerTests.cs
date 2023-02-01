using System.Web;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Infrastructure;
using GameStore.Web.Areas.Manager.Controllers;
using GameStore.Web.Areas.Manager.ViewModels;
using Moq;
using Xunit;

namespace GameStore.Tests.ControllerTests
{
    public class ManagerPlatformTypeControllerTests
    {
        private readonly Mock<IPlatformTypeService> _platformTypeServiceMock;
        private readonly ManagerPlatformTypeController _controller;

        public ManagerPlatformTypeControllerTests()
        {
            _platformTypeServiceMock = new Mock<IPlatformTypeService>();
            var mapperMock = new Mock<IMapper>();
            _controller = new ManagerPlatformTypeController(_platformTypeServiceMock.Object, mapperMock.Object);
            mapperMock.Setup(m => m.Map<PlatformTypeDTO, PlatformTypeViewModel>(It.IsAny<PlatformTypeDTO>()))
                .Returns(new PlatformTypeViewModel());
            mapperMock.Setup(m => m.Map<PlatformTypeViewModel, PlatformTypeDTO>(It.IsAny<PlatformTypeViewModel>()))
                .Returns(new PlatformTypeDTO());
        }

        [Fact]
        public void CreatePlatformTypeGet_NewPlatformType_ReturnsViewResult()
        {
            var result = _controller.CreatePlatformType();

            Assert.NotNull(result);
        }

        [Fact]
        public void CreatePlatformType_ServiceError_ReturnsViewResult()
        {
            _platformTypeServiceMock.Setup(g => g.Add(It.IsAny<PlatformTypeDTO>()))
                .Returns(new OperationDetails(false));

            _controller.CreatePlatformType(new PlatformTypeViewModel { TypeEn = "p1" });

            _platformTypeServiceMock.Verify(srv => srv.Add(It.IsAny<PlatformTypeDTO>()), Times.Once);
        }

        [Fact]
        public void CreatePlatformType_NewPlatformType_ReturnsRedirectResult()
        {
            _platformTypeServiceMock.Setup(g => g.Add(It.IsAny<PlatformTypeDTO>()))
                .Returns(new OperationDetails(true));

            _controller.CreatePlatformType(new PlatformTypeViewModel { TypeEn = "p1" });

            _platformTypeServiceMock.Verify(srv => srv.Add(It.IsAny<PlatformTypeDTO>()), Times.Once);
        }

        [Fact]
        public void EditPlatformTypeGet_NonExistentId_ReturnsRedirectResult()
        {
            _platformTypeServiceMock.Setup(g => g.GetById(1)).Returns((PlatformTypeDTO)null);

            var ex = Assert.Throws<HttpException>(() => _controller.EditPlatformType(1));

            Assert.NotNull(ex);
        }

        [Fact]
        public void EditPlatformTypeGet_ExistentId_ReturnsViewResult()
        {
            _platformTypeServiceMock.Setup(g => g.GetById(1)).Returns(new PlatformTypeDTO() { Id = 1 });

            var result = _controller.EditPlatformType(1) as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void EditPlatformType_SomePlatformTypeForEdit_ReturnsRedirectResult()
        {
            _platformTypeServiceMock.Setup(g => g.Update(It.IsAny<PlatformTypeDTO>()))
                .Returns(new OperationDetails(true));

            _controller.EditPlatformType(new PlatformTypeViewModel());

            _platformTypeServiceMock.Verify(srv => srv.Update(It.IsAny<PlatformTypeDTO>()), Times.Once);
        }

        [Fact]
        public void EditPlatformType_BadPlatformTypeObject_ReturnsViewResult()
        {
            _platformTypeServiceMock.Setup(g => g.Update(It.IsAny<PlatformTypeDTO>()))
                .Returns(new OperationDetails(false, "Fail", null));

            _controller.EditPlatformType(new PlatformTypeViewModel());

            _platformTypeServiceMock.Verify(srv => srv.Update(It.IsAny<PlatformTypeDTO>()), Times.Once);
        }

        [Fact]
        public void DeletePlatformTypeGet_NonExistentId_ReturnsRedirectResult()
        {
            _platformTypeServiceMock.Setup(g => g.GetById(1)).Returns((PlatformTypeDTO)null);

            var ex = Assert.Throws<HttpException>(() => _controller.DeletePlatformType(1));

            Assert.NotNull(ex);
        }

        [Fact]
        public void DeletePlatformTypeGet_ExistentId_ReturnsViewResult()
        {
            _platformTypeServiceMock.Setup(g => g.GetById(1)).Returns(new PlatformTypeDTO { Id = 1 });

            var result = _controller.DeletePlatformType(1) as PartialViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void DeletePlatformType_SomePlatformTypeForDelete_ReturnsRedirectResult()
        {
            _platformTypeServiceMock.Setup(g => g.Delete(It.IsAny<PlatformTypeDTO>()))
                .Returns(new OperationDetails(true));

            _controller.DeletePlatformType(new DeletePlatformTypeViewModel { Id = 1 });

            _platformTypeServiceMock.Verify(srv => srv.Delete(It.IsAny<PlatformTypeDTO>()), Times.Once);
        }

        [Fact]
        public void DeletePlatformType_BadPlatformTypeObject_ReturnsRedirectResult()
        {
            _platformTypeServiceMock.Setup(g => g.Delete(It.IsAny<PlatformTypeDTO>()))
                .Returns(new OperationDetails(false, "Fail", null));

            _controller.DeletePlatformType(new DeletePlatformTypeViewModel { Id = 1 });

            _platformTypeServiceMock.Verify(srv => srv.Delete(It.IsAny<PlatformTypeDTO>()), Times.Once);
        }
    }
}