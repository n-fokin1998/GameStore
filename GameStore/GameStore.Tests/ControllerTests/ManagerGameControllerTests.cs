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
    public class ManagerGameControllerTests
    {
        private readonly Mock<IGameService> _gameServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ManagerGameController _controller;

        public ManagerGameControllerTests()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            _gameServiceMock = new Mock<IGameService>();
            var genreServiceMock = new Mock<IGenreService>();
            var platformTypeServiceMock = new Mock<IPlatformTypeService>();
            var publisherServiceMock = new Mock<IPublisherService>();
            _mapperMock = new Mock<IMapper>();
            var authMock = new Mock<IAuthentication>();
            var identityMock = new Mock<IUserIdentity>();
            identityMock.Setup(i => i.User).Returns(new UserDTO());
            authMock.Setup(a => a.CurrentUser.Identity).Returns(new UserIndentity(unitOfWorkMock.Object, _mapperMock.Object));
            _controller = new ManagerGameController(
                _gameServiceMock.Object,
                platformTypeServiceMock.Object,
                genreServiceMock.Object,
                publisherServiceMock.Object,
                _mapperMock.Object)
            {
                Auth = authMock.Object
            };
            genreServiceMock.Setup(g => g.GetList()).Returns(Mock.Of<List<GenreDTO>>());
            platformTypeServiceMock.Setup(g => g.GetList()).Returns(Mock.Of<List<PlatformTypeDTO>>());
            publisherServiceMock.Setup(g => g.GetList()).Returns(Mock.Of<List<PublisherDTO>>());
        }

        [Fact]
        public void CreateGameGet_NewGame_ReturnsViewResult()
        {
            var result = _controller.CreateGame();

            Assert.NotNull(result);
        }

        [Fact]
        public void CreateGame_ServiceError_ReturnsViewResult()
        {
            _mapperMock.Setup(m => m.Map<AddGameViewModel, GameDTO>(It.IsAny<AddGameViewModel>())).Returns(
                new GameDTO());
            _gameServiceMock.Setup(g => g.Add(It.IsAny<GameDTO>()))
                .Returns(new OperationDetails(false));

            var result = _controller.CreateGame(new AddGameViewModel() { Key = "g1" }, null) as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void CreateGame_NewGame_ReturnsRedirectResult()
        {
            _mapperMock.Setup(m => m.Map<AddGameViewModel, GameDTO>(It.IsAny<AddGameViewModel>())).Returns(
                new GameDTO());
            _gameServiceMock.Setup(g => g.Add(It.IsAny<GameDTO>()))
                .Returns(new OperationDetails(true));

            _controller.CreateGame(new AddGameViewModel() { Key = "g1" }, null);

            _gameServiceMock.Verify(srv => srv.Add(It.IsAny<GameDTO>()), Times.Once);
        }

        [Fact]
        public void EditGameeGet_NonExistentKey_ReturnsRedirectResult()
        {
            _gameServiceMock.Setup(g => g.GetByKey("g1"))
                .Returns((GameDTO)null);

            var ex = Assert.Throws<HttpException>(() => _controller.EditGame("g1"));

            Assert.NotNull(ex);
        }

        [Fact]
        public void EditGameGet_ExistentKey_ReturnsViewResult()
        {
            _mapperMock.Setup(m => m.Map<GameDTO, AddGameViewModel>(It.IsAny<GameDTO>())).Returns(
                new AddGameViewModel());
            _gameServiceMock.Setup(g => g.GetByKey("g1"))
                .Returns(new GameDTO { Id = 1 });

            var result = _controller.EditGame("g1") as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void EditGame_SomeGameForEdit_ReturnsRedirectResult()
        {
            _mapperMock.Setup(m => m.Map<AddGameViewModel, GameDTO>(It.IsAny<AddGameViewModel>())).Returns(
                new GameDTO());
            _gameServiceMock.Setup(g => g.Update(It.IsAny<GameDTO>()))
                .Returns(new OperationDetails(true));

            _controller.EditGame(new AddGameViewModel(), null);

            _gameServiceMock.Verify(srv => srv.Update(It.IsAny<GameDTO>()), Times.Once);
        }

        [Fact]
        public void EditGame_BadGameObject_ReturnsViewResult()
        {
            _mapperMock.Setup(m => m.Map<AddGameViewModel, GameDTO>(It.IsAny<AddGameViewModel>())).Returns(
                new GameDTO());
            _gameServiceMock.Setup(g => g.Update(It.IsAny<GameDTO>()))
                .Returns(new OperationDetails(false, "Fail", null));

            var result = _controller.EditGame(new AddGameViewModel(), null) as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void DeleteGameGet_NonExistentKey_ReturnsRedirectResult()
        {
            _gameServiceMock.Setup(g => g.GetByKey("g1"))
                .Returns((GameDTO)null);

            var ex = Assert.Throws<HttpException>(() => _controller.DeleteGame("g1"));

            Assert.NotNull(ex);
        }

        [Fact]
        public void DeleteGameGet_ExistentKey_ReturnsViewResult()
        {
            _mapperMock.Setup(m => m.Map<GameDTO, DeleteGameViewModel>(It.IsAny<GameDTO>())).Returns(
                new DeleteGameViewModel());
            _gameServiceMock.Setup(g => g.GetByKey("g1"))
                .Returns(new GameDTO { Id = 1 });

            var result = _controller.DeleteGame("g1") as PartialViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void DeleteGame_SomeGameForDelete_ReturnsRedirectResult()
        {
            _gameServiceMock.Setup(g => g.Delete(It.IsAny<GameDTO>()))
                .Returns(new OperationDetails(true));

            _controller.DeleteGame(new DeleteGameViewModel { GameKey = "g1" });

            _gameServiceMock.Verify(srv => srv.Delete(It.IsAny<GameDTO>()), Times.Once);
        }

        [Fact]
        public void DeleteGame_BadGameObject_ReturnsRedirectResult()
        {
            _gameServiceMock.Setup(g => g.Delete(It.IsAny<GameDTO>()))
                .Returns(new OperationDetails(false, "Fail", null));

            var result = _controller.DeleteGame(new DeleteGameViewModel { GameKey = "g1" }) as RedirectToRouteResult;

            Assert.NotNull(result);
        }
    }
}