using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BusinessLogicLayer.Domain;
using GameStore.BusinessLogicLayer.Domain.Enums;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Infrastructure;
using GameStore.BusinessLogicLayer.Services;
using GameStore.BusinessLogicLayer.Services.Filter;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.Domain.Mongo.Abstract;
using Moq;
using Xunit;

namespace GameStore.Tests.ServiceTests
{
    public class GameServiceTests
    {
        private readonly Mock<IRepository<Game>> _gameRepositoryMock;
        private readonly Mock<IRepository<Genre>> _genreRepositoryMock;
        private readonly Mock<IRepository<PlatformType>> _platformTypeRepositoryMock;
        private readonly Mock<IRepository<Publisher>> _publisherRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GameService _gameService;
        private readonly GameDTO _game1;
        private readonly GameDTO _game2;

        public GameServiceTests()
        {
            _gameRepositoryMock = new Mock<IRepository<Game>>();
            _genreRepositoryMock = new Mock<IRepository<Genre>>();
            _platformTypeRepositoryMock = new Mock<IRepository<PlatformType>>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            _publisherRepositoryMock = new Mock<IRepository<Publisher>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            var mongoUnitOfWorkMock = new Mock<IMongoUnitOfWork>();
            var mongoLoggerMock = new Mock<ILogRepository>();
            _mapperMock = new Mock<IMapper>();
            _gameService = new GameService(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                new GameSelectionPipeline(_unitOfWorkMock.Object));
            _unitOfWorkMock.Setup(uow => uow.Games).Returns(_gameRepositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.Genres).Returns(_genreRepositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.PlatformTypes).Returns(_platformTypeRepositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.Publishers).Returns(_publisherRepositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.Comments).Returns(commentRepositoryMock.Object);
            mongoUnitOfWorkMock.Setup(uow => uow.Logs).Returns(mongoLoggerMock.Object);
            _game1 = new GameDTO { Id = 1, Key = "g1" };
            _game2 = new GameDTO { Id = 2, Key = "g2" };
        }

        [Fact]
        public void GetGamesList_SomeData_ReturnsGameList()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<Game>, List<GameDTO>>(
                It.IsAny<IEnumerable<Game>>())).Returns(new List<GameDTO>
            {
                _game1,
                _game2
            });
            _unitOfWorkMock.Setup(uow => uow.Games).Returns(_gameRepositoryMock.Object);

            var result = _gameService.GetList();

            Assert.Equal(1, result.FirstOrDefault().Id);
        }

        [Fact]
        public void GetGamesList_EmptyData_ReturnsEmptyList()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<Game>, List<GameDTO>>(It.IsAny<IEnumerable<Game>>()))
                .Returns(new List<GameDTO>());

            var result = _gameService.GetList();

            Assert.Empty(result);
        }

        [Fact]
        public void GetGameByKey_ExistingKey_ReturnsRightGame()
        {
            _mapperMock.Setup(m => m.Map<Game, GameDTO>(It.IsAny<Game>())).Returns(_game1);
            _gameRepositoryMock.Setup(g => g.GetList()).Returns(
                new List<Game>() { new Game { Key = "g1" } }.AsQueryable());

            var result = _gameService.GetByKey("g1");

            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void GetGameByKey_NonExisitentKey_ReturnsNull()
        {
            var result = _gameService.GetByKey(It.IsAny<string>());

            Assert.Null(result);
        }

        [Fact]
        public void GetGamesByGenre_NonExistentGenre_ThrowsException()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<Game>, List<GameDTO>>(It.IsAny<IEnumerable<Game>>()))
                .Returns(new List<GameDTO>());
            _genreRepositoryMock.Setup(g => g.GetItem(It.IsAny<int>())).Returns((Genre)null);

            var ex = Assert.Throws<ServiceException>(() => _gameService.GetByGenre(It.IsAny<int>()));

            Assert.Equal(typeof(ServiceException), ex.GetType());
        }

        [Fact]
        public void GetGamesByGenre_SomeGenre_ReturnEmptyGameList()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<Game>, List<GameDTO>>(It.IsAny<IEnumerable<Game>>()))
                .Returns(new List<GameDTO>());
            _genreRepositoryMock.Setup(g => g.GetItem(It.IsAny<int>())).Returns(Mock.Of<Genre>());

            var result = _gameService.GetByGenre(It.IsAny<int>());

            Assert.Empty(result);
        }

        [Fact]
        public void GetGamesByGenre_RightGenre_ReturnGameList()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<Game>, List<GameDTO>>(It.IsAny<IEnumerable<Game>>()))
                .Returns(new List<GameDTO>
            {
                _game1,
                _game2
            });
            _genreRepositoryMock.Setup(g => g.GetItem(1)).Returns(new Genre()
            {
                NameEn = "TestGenre",
                Games = new List<Game>()
                {
                    new Game() { Id = 1, Key = "g1" },
                    new Game() { Id = 2, Key = "g2" }
                }
            });

            var result = _gameService.GetByGenre(1);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetGamesByPlatformType_SomePlatformType_ReturnEmptyGameList()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<Game>, List<GameDTO>>(It.IsAny<IEnumerable<Game>>()))
                .Returns(new List<GameDTO>());
            _platformTypeRepositoryMock.Setup(g => g.GetItem(It.IsAny<int>())).Returns(Mock.Of<PlatformType>());

            var result = _gameService.GetByPlatformType(It.IsAny<int>());

            Assert.Empty(result);
        }

        [Fact]
        public void GetGamesByPlatformType_NonExistentPlatformType_ThrowsException()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<Game>, List<GameDTO>>(It.IsAny<IEnumerable<Game>>()))
                .Returns(new List<GameDTO>());
            _platformTypeRepositoryMock.Setup(g => g.GetItem(It.IsAny<int>())).Returns((PlatformType)null);

            var ex = Assert.Throws<ServiceException>(() => _gameService.GetByPlatformType(It.IsAny<int>()));

            Assert.Equal(typeof(ServiceException), ex.GetType());
        }

        [Fact]
        public void GetGamesByPlatformType_RightPlatformType_ReturnGameList()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<Game>, List<GameDTO>>(It.IsAny<IEnumerable<Game>>()))
                .Returns(new List<GameDTO>
            {
                _game1,
                _game2
            });
            _platformTypeRepositoryMock.Setup(g => g.GetItem(1)).Returns(new PlatformType()
            {
                TypeEn = "TestPlatformType",
                Games = new List<Game>
                {
                    new Game { Id = 1, Key = "g1" },
                    new Game { Id = 2, Key = "g2" }
                }
            });

            var result = _gameService.GetByPlatformType(1);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetFilteredGames_CombineFilters_ReturnGameList()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<Game>, List<GameDTO>>(It.IsAny<IEnumerable<Game>>()))
                .Returns(new List<GameDTO>
                {
                    _game1,
                    _game2
                });

            var result = _gameService.GetFilteredList(
                new GameFilterDTO
            {
                GenreFilters = new[] { 1, 2 },
                PlatformTypeFilters = new[] { 1, 2 },
                PublisherFilters = new[] { 1, 2 },
                DateFilter = DateFilterType.All,
                NameFilter = "test",
                MinPrice = 1,
                MaxPrice = 100,
                SortType = SortType.Date
            },
                new PageInfo());

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetFilteredGames_SomeDateFilter_ReturnGameList()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<Game>, List<GameDTO>>(It.IsAny<IEnumerable<Game>>()))
                .Returns(new List<GameDTO>
                {
                    _game1,
                    _game2
                });

            var result = _gameService.GetFilteredList(
                new GameFilterDTO
            {
                GenreFilters = new[] { 1, 2 },
                PlatformTypeFilters = new[] { 1, 2 },
                PublisherFilters = new[] { 1, 2 },
                DateFilter = DateFilterType.LastThreeYears,
                NameFilter = "test",
                MinPrice = 1,
                MaxPrice = 100,
                SortType = SortType.Date
            },
                new PageInfo());

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetFilteredGames_MostPopularSortType_ReturnGameList()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<Game>, List<GameDTO>>(It.IsAny<IEnumerable<Game>>()))
                .Returns(new List<GameDTO>
                {
                    _game1,
                    _game2
                });

            var result = _gameService.GetFilteredList(
                new GameFilterDTO
            {
                GenreFilters = new[] { 1, 2 },
                PlatformTypeFilters = new[] { 1, 2 },
                PublisherFilters = new[] { 1, 2 },
                DateFilter = DateFilterType.LastWeek,
                NameFilter = "test",
                MinPrice = 1,
                MaxPrice = 100,
                SortType = SortType.MostPopular
            },
                new PageInfo());

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetFilteredGames_MostCommentedSortType_ReturnGameList()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<Game>, List<GameDTO>>(It.IsAny<IEnumerable<Game>>()))
                .Returns(new List<GameDTO>
                {
                    _game1,
                    _game2
                });

            var result = _gameService.GetFilteredList(
                new GameFilterDTO
            {
                GenreFilters = new[] { 1, 2 },
                PlatformTypeFilters = new[] { 1, 2 },
                PublisherFilters = new[] { 1, 2 },
                DateFilter = DateFilterType.LastWeek,
                NameFilter = "test",
                MinPrice = 1,
                MaxPrice = 100,
                SortType = SortType.MostCommented
            },
                new PageInfo());

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetFilteredGames_PriceAscSortType_ReturnGameList()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<Game>, List<GameDTO>>(It.IsAny<IEnumerable<Game>>()))
                .Returns(new List<GameDTO>
                {
                    _game1,
                    _game2
                });

            var result = _gameService.GetFilteredList(
                new GameFilterDTO
            {
                GenreFilters = new[] { 1, 2 },
                PlatformTypeFilters = new[] { 1, 2 },
                PublisherFilters = new[] { 1, 2 },
                DateFilter = DateFilterType.LastWeek,
                NameFilter = "test",
                MinPrice = 1,
                MaxPrice = 100,
                SortType = SortType.PriceAsc
            },
                new PageInfo());

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetFilteredGames_PriceDescSortType_ReturnGameList()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<Game>, List<GameDTO>>(It.IsAny<IEnumerable<Game>>()))
                .Returns(new List<GameDTO>
                {
                    _game1,
                    _game2
                });

            var result = _gameService.GetFilteredList(
                new GameFilterDTO
            {
                GenreFilters = new[] { 1, 2 },
                PlatformTypeFilters = new[] { 1, 2 },
                PublisherFilters = new[] { 1, 2 },
                DateFilter = DateFilterType.LastWeek,
                NameFilter = "test",
                MinPrice = 1,
                MaxPrice = 100,
                SortType = SortType.PriceDesc
            }, 
                new PageInfo());

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetFilteredGames_EmptyNameFilter_ReturnGameList()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<Game>, List<GameDTO>>(It.IsAny<IEnumerable<Game>>()))
                .Returns(new List<GameDTO>
                {
                    _game1,
                    _game2
                });

            var result = _gameService.GetFilteredList(
                new GameFilterDTO
            {
                GenreFilters = new[] { 1, 2 },
                PlatformTypeFilters = new[] { 1, 2 },
                PublisherFilters = new[] { 1, 2 },
                DateFilter = DateFilterType.LastTwoYears,
                MinPrice = 1,
                MaxPrice = 100,
                SortType = SortType.Date
            }, 
                new PageInfo());

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetQuantity_SomeData_ReturnsValue()
        {
            _gameRepositoryMock.Setup(p => p.GetList()).Returns(
                new List<Game> { new Game { Id = 1 }, new Game { Id = 2 } }.AsQueryable());

            var result = _gameService.GetQuantity();

            Assert.Equal(2, result);
        }

        [Fact]
        public void AddGame_NullGame_ReturnFalseSucceeded()
        {
            var result = _gameService.Add(null);

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void AddGame_ExistentGameWithSameKey_ReturnFalseSucceeded()
        {
            _gameRepositoryMock.Setup(p => p.GetList()).Returns(
                new List<Game> { new Game { Id = 1, Key = "g1" } }.AsQueryable());

            var result = _gameService.Add(new GameDTO { Key = "g1", PublisherId = 1 });

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void AddGame_NonExistentPublisher_ReturnTrueSucceeded()
        {
            _mapperMock.Setup(m => m.Map<GameDTO, Game>(It.IsAny<GameDTO>())).Returns(new Game());
            _publisherRepositoryMock.Setup(p => p.GetItem(It.IsAny<int>())).Returns((Publisher)null);

            var result = _gameService.Add(new GameDTO { Id = 1, Key = "g1", PublisherId = 1 });

            Assert.True(result.Succeeded);
        }

        [Fact]
        public void AddGame_RightGame_ReturnTrueSucceeded()
        {
            _mapperMock.Setup(m => m.Map<GameDTO, Game>(It.IsAny<GameDTO>())).Returns(new Game());
            _publisherRepositoryMock.Setup(p => p.GetItem(1)).Returns(Mock.Of<Publisher>());

            _gameService.Add(new GameDTO { Id = 1, Key = "g1", PublisherId = 1 });

            _gameRepositoryMock.Verify(c => c.Add(It.IsAny<Game>()), Times.Once);
        }

        [Fact]
        public void DeleteGame_NullGame_ReturnFalseSucceeded()
        {
            var result = _gameService.Delete(null);

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void DeleteGame_NonExistenGame_ReturnFalseSucceeded()
        {
            _gameRepositoryMock.Setup(g => g.GetItem(1)).Returns((Game)null);

            var result = _gameService.Delete(_game1);

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void DeleteGame_ExistingGame_ReturnTrueSucceeded()
        {
            _gameRepositoryMock.Setup(g => g.GetItem(1)).Returns(Mock.Of<Game>());

            _gameService.Delete(_game1);

            _gameRepositoryMock.Verify(c => c.Update(It.IsAny<Game>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void UpdateGame_NullGame_ReturnFalseSucceeded()
        {
            var result = _gameService.Update(null);

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void UpdateGame_NonExistenGame_ReturnFalseSucceeded()
        {
            _gameRepositoryMock.Setup(g => g.GetItem(1)).Returns((Game)null);

            var result = _gameService.Update(_game1);

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void UpdateGame_RightGame_ReturnTrueSucceeded()
        {
            _mapperMock.Setup(m => m.Map<GameDTO, Game>(It.IsAny<GameDTO>())).Returns(Mock.Of<Game>());
            _gameRepositoryMock.Setup(g => g.GetItem(It.IsAny<int>())).Returns(Mock.Of<Game>());
            _publisherRepositoryMock.Setup(g => g.GetItem(It.IsAny<int>())).Returns(Mock.Of<Publisher>());

            _gameService.Update(_game1);

            _gameRepositoryMock.Verify(c => c.Update(It.IsAny<Game>(), It.IsAny<int>()), Times.Once);
        }
    }
}