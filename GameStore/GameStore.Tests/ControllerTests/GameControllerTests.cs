using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.Domain;
using GameStore.BusinessLogicLayer.Domain.Enums;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.Web.Controllers;
using GameStore.Web.Infrastructure;
using GameStore.Web.ViewModels;
using GameStore.Web.ViewModels.Enums;
using Moq;
using Xunit;

namespace GameStore.Tests.ControllerTests
{
    public class GameControllerTests
    {
        private readonly Mock<IGameService> _gameServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GameController _controller;

        public GameControllerTests()
        {
            var gameRepositoryMock = new Mock<IRepository<Game>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            _gameServiceMock = new Mock<IGameService>();
            var genreServiceMock = new Mock<IGenreService>();
            var platformTypeServiceMock = new Mock<IPlatformTypeService>();
            var publisherServiceMock = new Mock<IPublisherService>();
            _mapperMock = new Mock<IMapper>();
            var moqContext = new Mock<HttpContextBase>();
            var moqController = new Mock<ControllerContext>();
            moqContext.Setup(x => x.Session).Returns(Mock.Of<HttpSessionStateBase>());
            moqController.Setup(c => c.HttpContext).Returns(moqContext.Object);
            _controller = new GameController(
                _gameServiceMock.Object,
                platformTypeServiceMock.Object,
                genreServiceMock.Object,
                publisherServiceMock.Object,
                _mapperMock.Object,
                new FileSystemAccess())
            {
                ControllerContext = moqController.Object
            };
            genreServiceMock.Setup(g => g.GetList()).Returns(Mock.Of<List<GenreDTO>>());
            platformTypeServiceMock.Setup(g => g.GetList()).Returns(Mock.Of<List<PlatformTypeDTO>>());
            publisherServiceMock.Setup(g => g.GetList()).Returns(Mock.Of<List<PublisherDTO>>());
            unitOfWorkMock.Setup(u => u.Games).Returns(gameRepositoryMock.Object);
        }

        [Fact]
        public void GetAllGamesGenreFilter_SomeData_ReturnsViewResult()
        {
            SetData();

            var result = _controller.GetAllGames(new CatalogViewModel { GenreFilters = new[] { 1, 2 } }) as ViewResult;
            var model = result.Model as CatalogViewModel;

            Assert.True(model.Games.Count == 2);
            Assert.Equal(2, model.Games[0].Id);
            Assert.Equal(3, model.Games[1].Id);
        }

        [Fact]
        public void GetAllGamesPlatformTypeFilter_SomeData_ReturnsViewResultWithModel()
        {
            SetData();

            var result = _controller.GetAllGames(
                new CatalogViewModel { PlatformTypeFilters = new[] { 1, 2 } }) as ViewResult;
            var model = result.Model as CatalogViewModel;

            Assert.True(model.Games.Count == 2);
        }

        [Fact]
        public void GetAllGamesPublisherFilter_SomeData_ReturnsViewResultWithModel()
        {
            SetData();

            var result = _controller.GetAllGames(
                new CatalogViewModel { PublisherFilters = new[] { 1, 2 } }) as ViewResult;
            var model = result.Model as CatalogViewModel;

            Assert.True(model.Games.Count == 2);
        }

        [Fact]
        public void GetAllGamesPriceFilter_SomeData_ReturnsViewResultWithModel()
        {
            SetData();

            var result = _controller.GetAllGames(new CatalogViewModel { MinPrice = 2, MaxPrice = 4 }) as ViewResult;
            var model = result.Model as CatalogViewModel;

            Assert.True(model.Games.Count == 2);
        }

        [Fact]
        public void GetAllGamesDateFilterLastWeek_SomeData_ReturnsViewResultWithModel()
        {
            SetData();

            var result = _controller.GetAllGames(
                new CatalogViewModel { DateFilter = DateFilterType.LastWeek }) as ViewResult;
            var model = result.Model as CatalogViewModel;

            Assert.True(model.Games.Count == 2);
        }

        [Fact]
        public void GetAllGamesDateFilterLastMonth_SomeData_ReturnsViewResultWithModel()
        {
            SetData();

            var result = _controller.GetAllGames(
                new CatalogViewModel { DateFilter = DateFilterType.LastMonth }) as ViewResult;
            var model = result.Model as CatalogViewModel;

            Assert.True(model.Games.Count == 2);
        }

        [Fact]
        public void GetAllGamesDateFilterLastYear_SomeData_ReturnsViewResultWithModel()
        {
            SetData();

            var result = _controller.GetAllGames(
                new CatalogViewModel { DateFilter = DateFilterType.LastYear }) as ViewResult;
            var model = result.Model as CatalogViewModel;

            Assert.True(model.Games.Count == 2);
        }

        [Fact]
        public void GetAllGamesDateFilterLastTwoYears_SomeData_ReturnsViewResultWithModel()
        {
            SetData();

            var result = _controller.GetAllGames(
                new CatalogViewModel { DateFilter = DateFilterType.LastTwoYears }) as ViewResult;
            var model = result.Model as CatalogViewModel;

            Assert.True(model.Games.Count == 2);
        }

        [Fact]
        public void GetAllGamesDateFilterLastThreeYears_SomeData_ReturnsViewResultWithModel()
        {
            SetData();

            var result = _controller.GetAllGames(
                new CatalogViewModel { DateFilter = DateFilterType.LastThreeYears }) as ViewResult;
            var model = result.Model as CatalogViewModel;

            Assert.True(model.Games.Count == 2);
        }

        [Fact]
        public void GetAllGamesNameFilter_SomeData_ReturnsViewResultWithModel()
        {
            SetData();

            var result = _controller.GetAllGames(new CatalogViewModel { NameFilter = "tes" }) as ViewResult;
            var model = result.Model as CatalogViewModel;

            Assert.True(model.Games.Count == 2);
        }

        [Fact]
        public void GetAllGamesCombineFilter_SomeData_ReturnsViewResultWithModel()
        {
            SetData();

            var result = _controller.GetAllGames(new CatalogViewModel
            {
                GenreFilters = new[] { 3, 2 },
                PublisherFilters = new[] { 3, 2 },
                MinPrice = 3
            }) as ViewResult;
            var model = result.Model as CatalogViewModel;

            Assert.True(model.Games.Count == 2);
        }

        [Fact]
        public void GetAllGamesPriceAscSort_SomeData_ReturnsViewResultWithModel()
        {
            _gameServiceMock.Setup(m => m.GetFilteredList(It.IsAny<GameFilterDTO>(), It.IsAny<PageInfo>())).Returns(new List<GameDTO>
            {
                new GameDTO { Id = 1, Key = "g1" },
                new GameDTO { Id = 2, Key = "g2" },
                new GameDTO { Id = 3, Key = "g3" },
                new GameDTO { Id = 4, Key = "g4" },
                new GameDTO { Id = 5, Key = "g5" }
            });

            var result = _controller.GetAllGames(
                new CatalogViewModel { SortType = SortTypeViewModel.PriceAsc }) as ViewResult;
            var model = result.Model as CatalogViewModel;

            Assert.True(model.Games.Count == 5);
        }

        [Fact]
        public void GetAllGamesPriceDescSort_SomeData_ReturnsViewResultWithModel()
        {
            _gameServiceMock.Setup(m => m.GetFilteredList(It.IsAny<GameFilterDTO>(), It.IsAny<PageInfo>())).Returns(new List<GameDTO>
            {
                new GameDTO { Id = 5, Key = "g1" },
                new GameDTO { Id = 4, Key = "g2" },
                new GameDTO { Id = 3, Key = "g3" },
                new GameDTO { Id = 2, Key = "g4" },
                new GameDTO { Id = 1, Key = "g5" }
            });

            var result = _controller.GetAllGames(
                new CatalogViewModel { SortType = SortTypeViewModel.PriceDesc }) as ViewResult;
            var model = result.Model as CatalogViewModel;

            Assert.True(model.Games.Count == 5);
        }

        [Fact]
        public void GetCountOfGames_SomeData_ReturnsValue()
        {
            _gameServiceMock.Setup(g => g.GetQuantity()).Returns(10);

            var result = _controller.GetCountOfGames();

            Assert.Equal(10, result);
        }

        [Fact]
        public void GameDetails_SomeData_ReturnsView()
        {
            _gameServiceMock.Setup(g => g.GetByKey("g1")).Returns(new GameDTO()
            {
                Id = 1,
                Key = "g1",
                Genres = new List<GenreDTO>() { new GenreDTO { NameEn = "g1", NameRu = "g2" } },
                PlatformTypes = new List<PlatformTypeDTO>() { new PlatformTypeDTO { TypeRu = "p1", TypeEn = "p2" } }
            });
            _mapperMock.Setup(m => m.Map<GameDTO, GameViewModel>(It.IsAny<GameDTO>())).Returns(
                new GameViewModel { Id = 1 });

            var result = _controller.GameDetails("g1") as ViewResult;
            var model = result.Model as GameViewModel;

            Assert.Equal(1, model.Id);
        }

        [Fact]
        public void GameDetails_NonExistentKey_ReturnsRedirectResult()
        {
            var ex = Assert.Throws<HttpException>(() => _controller.GameDetails(It.IsAny<string>()));

            Assert.NotNull(ex);
        }

        [Fact]
        public void DownloadGame_BagKey_ReturnsRedirectResult()
        {
            _gameServiceMock.Setup(g => g.GetByKey(It.IsAny<string>()))
                .Returns((GameDTO)null);

            var ex = Assert.Throws<HttpException>(() => _controller.DownloadGame(It.IsAny<string>()));

            Assert.NotNull(ex);
        }

        [Fact]
        public void DownloadGame_SomeKey_ReturnsFileResult()
        {
            _gameServiceMock.Setup(g => g.GetByKey(It.IsAny<string>()))
                .Returns(new GameDTO { NameEn = "g1" });

            var result = _controller.DownloadGame(It.IsAny<string>()) as FileResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void RenderImage_SomeKey_ReturnsFileResult()
        {
            _gameServiceMock.Setup(g => g.GetByKey(It.IsAny<string>()))
                .Returns(new GameDTO { NameEn = "g1", ImageReference = "test.png" });

            var result = _controller.RenderImage(It.IsAny<string>()) as FileResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void RenderImageAsync_SomeKey_ReturnsFileResult()
        {
            _gameServiceMock.Setup(g => g.GetByKey(It.IsAny<string>()))
                .Returns(new GameDTO { NameEn = "g1", ImageReference = "test.png" });

            var result = _controller.RenderImageAsync(It.IsAny<string>()).Result as FileResult;

            Assert.NotNull(result);
        }

        private void SetData()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<Game>, List<GameDTO>>(It.IsAny<IEnumerable<Game>>())).Returns(new List<GameDTO>
            {
                new GameDTO { Id = 2, Key = "g1" },
                new GameDTO { Id = 3, Key = "g2" }
            });
            _gameServiceMock.Setup(g => g.GetFilteredList(It.IsAny<GameFilterDTO>(), It.IsAny<PageInfo>())).Returns(new List<GameDTO>
            {
                new GameDTO { Id = 2, Key = "g1" },
                new GameDTO { Id = 3, Key = "g2" }
            });
        }
    }
}