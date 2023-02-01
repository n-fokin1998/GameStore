using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Services;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.Domain.Mongo.Abstract;
using Moq;
using Xunit;

namespace GameStore.Tests.ServiceTests
{
    public class GenreServiceTests
    {
        private readonly Mock<IRepository<Genre>> _genreRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GenreService _genreService;

        public GenreServiceTests()
        {
            _genreRepositoryMock = new Mock<IRepository<Genre>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mongoUnitOfWorkMock = new Mock<IMongoUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            var mongoLoggerMock = new Mock<ILogRepository>();
            _genreService = new GenreService(unitOfWorkMock.Object, _mapperMock.Object);
            unitOfWorkMock.Setup(uow => uow.Genres).Returns(_genreRepositoryMock.Object);
            mongoUnitOfWorkMock.Setup(uow => uow.Logs).Returns(mongoLoggerMock.Object);
        }

        [Fact]
        public void AddGenre_NullGenre_ReturnFalseSucceeded()
        {
            var result = _genreService.Add(null);

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void AddGenre_ExistentGenreWithSameName_ReturnFalseSucceeded()
        {
            _genreRepositoryMock.Setup(g => g.GetList()).Returns(new List<Genre>
            {
                new Genre { Id = 1, NameEn = "g1" }
            }.AsQueryable());

            var result = _genreService.Add(new GenreDTO { Id = 1, NameEn = "g1" });

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void AddGenre_RightGenre_ReturnTrueSucceeded()
        {
            _genreService.Add(new GenreDTO { Id = 1, NameEn = "g1" });

            _genreRepositoryMock.Verify(c => c.Add(It.IsAny<Genre>()), Times.Once);
        }

        [Fact]
        public void GetChildList_SomeData_ReturnsGenreList()
        {
            _genreRepositoryMock.Setup(g => g.GetList()).Returns(new List<Genre>
            {
                new Genre { Id = 1, ParentGenreId = 2 },
                new Genre { Id = 2, ParentGenreId = 3 },
                new Genre { Id = 3 }
            }.AsQueryable());
            _mapperMock.Setup(m => m.Map<IEnumerable<Genre>, List<GenreDTO>>(It.IsAny<IEnumerable<Genre>>()))
                .Returns(new List<GenreDTO>
                {
                    new GenreDTO { Id = 1 },
                    new GenreDTO { Id = 2 }
                });

            var result = _genreService.GetChildList(1);

            Assert.Equal(1, result.FirstOrDefault().Id);
        }

        [Fact]
        public void GetGenresList_SomeData_ReturnsGenreList()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<Genre>, List<GenreDTO>>(It.IsAny<IEnumerable<Genre>>()))
                .Returns(new List<GenreDTO>
            {
                new GenreDTO() { Id = 1 },
                new GenreDTO() { Id = 2 }
            });

            var result = _genreService.GetList();

            Assert.Equal(1, result.FirstOrDefault().Id);
        }

        [Fact]
        public void GetGenresList_EmptyData_ReturnsEmptyList()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<Genre>, List<GenreDTO>>(It.IsAny<IEnumerable<Genre>>()))
                .Returns(new List<GenreDTO>());

            var result = _genreService.GetList();

            Assert.Empty(result);
        }

        [Fact]
        public void GetGenreById_SomeId_ReturnsGenre()
        {
            _mapperMock.Setup(m => m.Map<Genre, GenreDTO>(It.IsAny<Genre>()))
                .Returns(new GenreDTO() { Id = 1 });

            var result = _genreService.GetById(It.IsAny<int>());

            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void DeleteGenre_NullGenre_ReturnFalseSucceeded()
        {
            var result = _genreService.Delete(null);

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void DeleteGenre_NonExistenGenre_ReturnFalseSucceeded()
        {
            _genreRepositoryMock.Setup(g => g.GetItem(1)).Returns((Genre)null);

            var result = _genreService.Delete(new GenreDTO { Id = 1, NameEn = "g1" });

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void DeleteGenre_ExistingGenre_ReturnTrueSucceeded()
        {
            _genreRepositoryMock.Setup(g => g.GetItem(1)).Returns(
                new Genre { Id = 1, NameEn = "g1", ParentGenreId = 2 });
            _genreRepositoryMock.Setup(g => g.GetList()).Returns(new List<Genre>()
            {
                new Genre() { Id = 1, NameEn = "g1", ParentGenreId = 2 },
                new Genre() { Id = 2, NameEn = "g2" },
                new Genre() { Id = 3, NameEn = "g3", ParentGenreId = 1 }
            }.AsQueryable());

            _genreService.Delete(new GenreDTO { Id = 1, NameEn = "g1", ParentGenreId = 2 });

            _genreRepositoryMock.Verify(c => c.Update(It.IsAny<Genre>(), It.IsAny<int>()), Times.Exactly(2));
        }

        [Fact]
        public void UpdateGenre_NullGenre_ReturnFalseSucceeded()
        {
            var result = _genreService.Update(null);

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void UpdateGenre_ExistentGenreWithSameName_ReturnFalseSucceeded()
        {
            _genreRepositoryMock.Setup(g => g.GetList()).Returns(new List<Genre>
            {
                new Genre { Id = 2, NameEn = "g1" }
            }.AsQueryable());

            var result = _genreService.Update(new GenreDTO { Id = 1, NameEn = "g1" });

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void UpdateGenre_RightGenre_ReturnTrueSucceeded()
        {
            _mapperMock.Setup(m => m.Map<GenreDTO, Genre>(It.IsAny<GenreDTO>())).Returns(Mock.Of<Genre>());
            _genreRepositoryMock.Setup(g => g.GetItem(It.IsAny<int>())).Returns(Mock.Of<Genre>());

            _genreService.Update(new GenreDTO { Id = 1, NameEn = "g1" });

            _genreRepositoryMock.Verify(c => c.Update(It.IsAny<Genre>(), It.IsAny<int>()), Times.Once);
        }
    }
}