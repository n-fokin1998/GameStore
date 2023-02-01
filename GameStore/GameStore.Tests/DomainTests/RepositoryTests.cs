using System.Data.Entity;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.Domain.Mongo.Abstract;
using GameStore.Domain.Repositories;
using Moq;
using Xunit;

namespace GameStore.Tests.DomainTests
{
    public class RepositoryTests
    {
        private readonly Mock<IDbContext> _contextMock;
        private readonly AbstractRepository<Game> _repository;

        public RepositoryTests()
        {
            _contextMock = new Mock<IDbContext>();
            var loggerMock = new Mock<ILogRepository>();
            _repository = new GameRepository(_contextMock.Object, loggerMock.Object);
        }

        [Fact]
        public void GetItem_SomeData_ReturnsItem()
        {
            _contextMock.Setup(c => c.Set<Game>().Find(It.IsAny<int>())).Returns(Mock.Of<Game>());

            var result = _repository.GetItem(It.IsAny<int>());

            Assert.NotNull(result);
            _contextMock.Verify(c => c.Set<Game>().Find(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GetList_SomeData_ReturnsIQueryable()
        {
            _contextMock.Setup(c => c.Set<Game>()).Returns(Mock.Of<DbSet<Game>>());

            var result = _repository.GetList();

            Assert.NotNull(result);
        }
    }
}