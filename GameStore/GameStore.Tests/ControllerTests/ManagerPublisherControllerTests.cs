using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Infrastructure;
using GameStore.BusinessLogicLayer.Abstract.Auth;
using GameStore.BusinessLogicLayer.Services.Auth;
using GameStore.Domain.Abstract;
using GameStore.Web.Areas.Manager.Controllers;
using GameStore.Web.Areas.Manager.ViewModels;
using Moq;
using Xunit;

namespace GameStore.Tests.ControllerTests
{
    public class ManagerPublisherControllerTests
    {
        private readonly Mock<IPublisherService> _publisherServiceMock;
        private readonly ManagerPublisherController _controller;

        public ManagerPublisherControllerTests()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            _publisherServiceMock = new Mock<IPublisherService>();
            var mapperMock = new Mock<IMapper>();
            var authMock = new Mock<IAuthentication>();
            var identityMock = new Mock<IUserIdentity>();
            identityMock.Setup(i => i.User).Returns(new UserDTO());
            authMock.Setup(a => a.CurrentUser.Identity).Returns(
                new UserIndentity(unitOfWorkMock.Object, mapperMock.Object));
            _controller =
                new ManagerPublisherController(_publisherServiceMock.Object, mapperMock.Object)
                {
                    Auth = authMock.Object
                };
            mapperMock.Setup(m => m.Map<PublisherDTO, PublisherViewModel>(It.IsAny<PublisherDTO>())).Returns(
                new PublisherViewModel());
            mapperMock.Setup(m => m.Map<PublisherViewModel, PublisherDTO>(It.IsAny<PublisherViewModel>())).Returns(
                new PublisherDTO());
        }

        [Fact]
        public void CreatePublisherGet_NewPublisher_ReturnsViewResult()
        {
            var result = _controller.CreatePublisher();

            Assert.NotNull(result);
        }

        [Fact]
        public void CreatePublisher_ServiceError_ReturnsView()
        {
            _publisherServiceMock.Setup(g => g.Add(It.IsAny<PublisherDTO>()))
                .Returns(new OperationDetails(false));

            var result = _controller.CreatePublisher(new PublisherViewModel { CompanyName = "p1" }) as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void CreatePublisher_NewPublisher_ReturnsRedirectResult()
        {
            _publisherServiceMock.Setup(g => g.Add(It.IsAny<PublisherDTO>()))
                .Returns(new OperationDetails(true));

            var result = _controller.CreatePublisher(
                new PublisherViewModel { CompanyName = "p1" }) as RedirectToRouteResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void EditPublisherGet_NonExistentPublisher_ReturnsRedirectResult()
        {
            _publisherServiceMock.Setup(g => g.GetList())
                .Returns(new List<PublisherDTO>());

            var ex = Assert.Throws<HttpException>(() => _controller.EditPublisher("p1"));

            Assert.NotNull(ex);
        }

        [Fact]
        public void EditPublisherGet_ExistentPublisher_ReturnsRedirectResult()
        {
            SetData();

            var result = _controller.EditPublisher("p1") as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void EditPublisher_SomePublisherForEdit_ReturnsRedirectResult()
        {
            _publisherServiceMock.Setup(g => g.Update(It.IsAny<PublisherDTO>()))
                .Returns(new OperationDetails(true));

            var result = _controller.EditPublisher(new PublisherViewModel()) as RedirectToRouteResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void EditPublisher_BadPublisherObject_ReturnsViewResult()
        {
            _publisherServiceMock.Setup(g => g.Update(It.IsAny<PublisherDTO>()))
                .Returns(new OperationDetails(false, "Fail", null));

            var result = _controller.EditPublisher(new PublisherViewModel()) as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void DeletePublisherGet_NonExistentPublisher_ReturnsRedirectResult()
        {
            _publisherServiceMock.Setup(g => g.GetList())
                .Returns(new List<PublisherDTO>());

            var ex = Assert.Throws<HttpException>(() => _controller.DeletePublisher("p1"));

            Assert.NotNull(ex);
        }

        [Fact]
        public void DeletePublisherGet_ExistentPublisher_ReturnsRedirectResult()
        {
            SetData();

            var result = _controller.DeletePublisher("p1") as PartialViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void DeletePublisher_SomePublisherForDelete_ReturnsRedirectResult()
        {
            _publisherServiceMock.Setup(g => g.Delete(It.IsAny<PublisherDTO>()))
                .Returns(new OperationDetails(true));

            var result = _controller.DeletePublisher(
                new DeletePublisherViewModel { CompanyName = "p1" }) as RedirectToRouteResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void DeletePublisher_BadPublisherObject_ReturnsRedirectResult()
        {
            SetData();
            _publisherServiceMock.Setup(g => g.Delete(It.IsAny<PublisherDTO>()))
                .Returns(new OperationDetails(false, "Fail", null));

            var result = _controller.DeletePublisher(
                new DeletePublisherViewModel { CompanyName = "p1" }) as RedirectToRouteResult;

            Assert.NotNull(result);
        }

        private void SetData()
        {
            _publisherServiceMock.Setup(g => g.GetList())
                .Returns(new List<PublisherDTO>() { new PublisherDTO() { CompanyName = "p1" } });
        }
    }
}