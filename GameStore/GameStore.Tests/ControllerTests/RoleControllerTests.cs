using System.Web.Mvc;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Infrastructure;
using GameStore.Web.Areas.Admin.Controllers;
using GameStore.Web.Areas.Admin.ViewModels;
using Moq;
using Xunit;

namespace GameStore.Tests.ControllerTests
{
    public class RoleControllerTests
    {
        private readonly Mock<IRoleService> _roleServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly RoleController _controller;

        public RoleControllerTests()
        {
            _roleServiceMock = new Mock<IRoleService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new RoleController(_roleServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public void RoleDetails_NonExistentId_ReturnsRedirectResult()
        {
            var result = _controller.RoleDetails(It.IsAny<int>()) as RedirectToRouteResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void RoleDetails_ExistentId_ReturnsViewResult()
        {
            _roleServiceMock.Setup(g => g.GetById(1)).Returns(new RoleDTO() { Id = 1 });

            var result = _controller.RoleDetails(1) as ViewResult;
            var model = result.Model as RoleDTO;

            Assert.Equal(1, model.Id);
        }

        [Fact]
        public void CreateRoleGet_NewRole_ReturnsViewResult()
        {
            var result = _controller.CreateRole();

            Assert.NotNull(result);
        }

        [Fact]
        public void CreateRole_ServiceError_ReturnsViewResult()
        {
            _roleServiceMock.Setup(g => g.Add(It.IsAny<RoleDTO>()))
                .Returns(new OperationDetails(false));

            _controller.CreateRole(new RoleViewModel { Name = "r1" });

            _roleServiceMock.Verify(srv => srv.Add(It.IsAny<RoleDTO>()), Times.Once);
        }

        [Fact]
        public void CreateRole_NewRole_ReturnsRedirectResult()
        {
            _roleServiceMock.Setup(g => g.Add(It.IsAny<RoleDTO>()))
                .Returns(new OperationDetails(true));

            _controller.CreateRole(new RoleViewModel { Name = "r1" });

            _roleServiceMock.Verify(srv => srv.Add(It.IsAny<RoleDTO>()), Times.Once);
        }

        [Fact]
        public void EditRoleGet_NonExistentId_ReturnsRedirectResult()
        {
            _roleServiceMock.Setup(g => g.GetById(1))
                .Returns((RoleDTO)null);

            var result = _controller.EditRole(1) as RedirectToRouteResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void EditRole_SomeRoleForEdit_ReturnsRedirectResult()
        {
            _mapperMock.Setup(m => m.Map<RoleViewModel, RoleDTO>(It.IsAny<RoleViewModel>())).Returns(new RoleDTO());
            _roleServiceMock.Setup(g => g.Update(It.IsAny<RoleDTO>()))
                .Returns(new OperationDetails(true));

            _controller.EditRole(new RoleViewModel());
        
            _roleServiceMock.Verify(srv => srv.Update(It.IsAny<RoleDTO>()), Times.Once);
        }

        [Fact]
        public void EditUser_BadUserObject_ReturnsViewResult()
        {
            _mapperMock.Setup(m => m.Map<RoleViewModel, RoleDTO>(It.IsAny<RoleViewModel>())).Returns(new RoleDTO());
            _roleServiceMock.Setup(g => g.Update(It.IsAny<RoleDTO>()))
                .Returns(new OperationDetails(false, "Fail", null));

            _controller.EditRole(new RoleViewModel());

            _roleServiceMock.Verify(srv => srv.Update(It.IsAny<RoleDTO>()), Times.Once);
        }

        [Fact]
        public void DeleteRoleGet_NonExistentId_ReturnsRedirectResult()
        {
            _roleServiceMock.Setup(g => g.GetById(1))
                .Returns((RoleDTO)null);

            var result = _controller.DeleteRole(1) as RedirectToRouteResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void DeleteRoleGet_ExistentId_ReturnsViewResult()
        {
            _roleServiceMock.Setup(g => g.GetById(1))
                .Returns(new RoleDTO { Id = 1 });

            var result = _controller.DeleteRole(1) as PartialViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void DeleteRole_SomeRoleForDelete_ReturnsRedirectResult()
        {
            _roleServiceMock.Setup(g => g.Delete(It.IsAny<RoleDTO>()))
                .Returns(new OperationDetails(true));

            _controller.DeleteRole(new DeleteViewModel { Id = 1 });

            _roleServiceMock.Verify(srv => srv.Delete(It.IsAny<RoleDTO>()), Times.Once);
        }

        [Fact]
        public void DeleteRole_BadRoleObject_ReturnsRedirectResult()
        {
            _roleServiceMock.Setup(g => g.Delete(It.IsAny<RoleDTO>()))
                .Returns(new OperationDetails(false, "Fail", null));

            _controller.DeleteRole(new DeleteViewModel { Id = 1 });

            _roleServiceMock.Verify(srv => srv.Delete(It.IsAny<RoleDTO>()), Times.Once);
        }
    }
}