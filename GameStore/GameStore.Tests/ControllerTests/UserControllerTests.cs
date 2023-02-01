using System.Collections.Generic;
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
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            var roleServiceMock = new Mock<IRoleService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new UserController(_userServiceMock.Object, roleServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public void UserDetails_NonExistentId_ReturnsRedirectResult()
        {
            var result = _controller.UserDetails(It.IsAny<int>()) as RedirectToRouteResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void UserDetails_ExistentId_ReturnsViewResult()
        {
            _userServiceMock.Setup(g => g.GetById(1)).Returns(new UserDTO() { Id = 1 });

            var result = _controller.UserDetails(1) as ViewResult;
            var model = result.Model as UserDTO;

            Assert.Equal(1, model.Id);
        }

        [Fact]
        public void EditUserGet_NonExistentId_ReturnsRedirectResult()
        {
            _userServiceMock.Setup(g => g.GetById(1))
                .Returns((UserDTO)null);

            var result = _controller.EditUser(1) as RedirectToRouteResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void EditUser_SomeUserForEdit_ReturnsRedirectResult()
        {
            _mapperMock.Setup(m => m.Map<UserViewModel, UserDTO>(It.IsAny<UserViewModel>()))
                .Returns(new UserDTO() { Roles = new List<RoleDTO>() });
            _userServiceMock.Setup(g => g.Update(It.IsAny<UserDTO>()))
                .Returns(new OperationDetails(true));

            _controller.EditUser(new UserViewModel());

            _userServiceMock.Verify(srv => srv.Update(It.IsAny<UserDTO>()), Times.Once);
        }

        [Fact]
        public void EditUser_BadUserObject_ReturnsViewResult()
        {
            _mapperMock.Setup(m => m.Map<UserViewModel, UserDTO>(It.IsAny<UserViewModel>()))
                .Returns(new UserDTO { Roles = new List<RoleDTO>() });
            _userServiceMock.Setup(g => g.Update(It.IsAny<UserDTO>()))
                .Returns(new OperationDetails(false, "Fail", null));

            _controller.EditUser(new UserViewModel());

            _userServiceMock.Verify(srv => srv.Update(It.IsAny<UserDTO>()), Times.Once);
        }

        [Fact]
        public void DeleteUserGet_NonExistentId_ReturnsRedirectResult()
        {
            _userServiceMock.Setup(g => g.GetById(1))
                .Returns((UserDTO)null);

            var result = _controller.DeleteUser(1) as RedirectToRouteResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void DeleteUserGet_ExistentId_ReturnsViewResult()
        {
            _userServiceMock.Setup(g => g.GetById(1))
                .Returns(new UserDTO { Id = 1 });

            var result = _controller.DeleteUser(1) as PartialViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void DeleteUser_SomeUserForDelete_ReturnsRedirectResult()
        {
            _userServiceMock.Setup(g => g.Delete(It.IsAny<UserDTO>()))
                .Returns(new OperationDetails(true));

            _controller.DeleteUser(new DeleteViewModel { Id = 1 });

            _userServiceMock.Verify(srv => srv.Delete(It.IsAny<UserDTO>()), Times.Once);
        }

        [Fact]
        public void DeleteUser_BadUserObject_ReturnsRedirectResult()
        {
            _userServiceMock.Setup(g => g.Delete(It.IsAny<UserDTO>()))
                .Returns(new OperationDetails(false, "Fail", null));

            _controller.DeleteUser(new DeleteViewModel { Id = 1 });

            _userServiceMock.Verify(srv => srv.Delete(It.IsAny<UserDTO>()), Times.Once);
        }
    }
}