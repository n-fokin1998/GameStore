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
    public class PublisherServiceTests
    {
        private readonly Mock<IRepository<Publisher>> _publisherRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PublisherService _publisherService;

        public PublisherServiceTests()
        {
            _publisherRepositoryMock = new Mock<IRepository<Publisher>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mongoUnitOfWorkMock = new Mock<IMongoUnitOfWork>();
            var mongoLoggerMock = new Mock<ILogRepository>();
            _mapperMock = new Mock<IMapper>();
            _publisherService = new PublisherService(unitOfWorkMock.Object, _mapperMock.Object);
            unitOfWorkMock.Setup(uow => uow.Publishers).Returns(_publisherRepositoryMock.Object);
            mongoUnitOfWorkMock.Setup(uow => uow.Logs).Returns(mongoLoggerMock.Object);
        }

        [Fact]
        public void GetPublishersList_SomeData_ReturnsPublishersList()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<Publisher>, List<PublisherDTO>>(
                It.IsAny<IEnumerable<Publisher>>())).Returns(new List<PublisherDTO>
            {
                new PublisherDTO() { Id = 1 },
                new PublisherDTO() { Id = 2 }
            });

            var result = _publisherService.GetList();

            Assert.Equal(1, result.FirstOrDefault().Id);
        }

        [Fact]
        public void GetPublishersList_EmptyData_ReturnsEmptyList()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<Publisher>, List<PublisherDTO>>(
                It.IsAny<IEnumerable<Publisher>>())).Returns(new List<PublisherDTO>());
 
            var result = _publisherService.GetList();

            Assert.Empty(result);
        }

        [Fact]
        public void GetPublisherById_SomeId_ReturnsPublisher()
        {
            _mapperMock.Setup(m => m.Map<Publisher, PublisherDTO>(It.IsAny<Publisher>()))
                .Returns(new PublisherDTO() { Id = 1 });

            var result = _publisherService.GetById(It.IsAny<int>());

            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void GetPublisherById_BadId_ReturnsNull()
        {
            var result = _publisherService.GetById(2);

            Assert.Null(result);
        }

        [Fact]
        public void AddPublisher_NullPublisher_ReturnFalseSucceeded()
        {
            var result = _publisherService.Add(null);

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void AddPublisher_ExistentPublisherWithSameName_ReturnFalseSucceeded()
        {
            _publisherRepositoryMock.Setup(g => g.GetList()).Returns(new List<Publisher>
            {
                new Publisher { Id = 1, CompanyName = "p1" }
            }.AsQueryable());

            var result = _publisherService.Add(new PublisherDTO { Id = 1, CompanyName = "p1" });

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void AddPublisher_RightPublisher_ReturnTrueSucceeded()
        {
            _publisherService.Add(new PublisherDTO { Id = 1, CompanyName = "p1" });

            _publisherRepositoryMock.Verify(c => c.Add(It.IsAny<Publisher>()), Times.Once);
        }

        [Fact]
        public void DeletePublisher_NullPublisher_ReturnFalseSucceeded()
        {
            var result = _publisherService.Delete(null);

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void DeletePublisher_NonExistenPublisher_ReturnFalseSucceeded()
        {
            _publisherRepositoryMock.Setup(g => g.GetItem(1)).Returns((Publisher)null);

            var result = _publisherService.Delete(new PublisherDTO { Id = 1, CompanyName = "p1" });

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void DeletePublisher_ExistingPublisher_ReturnTrueSucceeded()
        {
            _publisherRepositoryMock.Setup(g => g.GetItem(1)).Returns(Mock.Of<Publisher>());

            _publisherService.Delete(new PublisherDTO { Id = 1, CompanyName = "p1" });

            _publisherRepositoryMock.Verify(c => c.Update(It.IsAny<Publisher>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void UpdatePublisher_NullPublisher_ReturnFalseSucceeded()
        {
            var result = _publisherService.Update(null);

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void UpdatePublisher_NonExistenPublisher_ReturnFalseSucceeded()
        {
            _publisherRepositoryMock.Setup(g => g.GetItem(1)).Returns((Publisher)null);

            var result = _publisherService.Update(new PublisherDTO { Id = 1, CompanyName = "p1" });

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void UpdatePublisher_ExistentPublisherWithSameName_ReturnFalseSucceeded()
        {
            _publisherRepositoryMock.Setup(g => g.GetList()).Returns(new List<Publisher>
            {
                new Publisher { Id = 2, CompanyName = "p1" }
            }.AsQueryable());

            var result = _publisherService.Update(new PublisherDTO { Id = 1, CompanyName = "p1" });

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void UpdatePublisher_RightPublisher_ReturnTrueSucceeded()
        {
            _mapperMock.Setup(m => m.Map<PublisherDTO, Publisher>(It.IsAny<PublisherDTO>()))
                .Returns(Mock.Of<Publisher>());
            _publisherRepositoryMock.Setup(g => g.GetItem(It.IsAny<int>())).Returns(new Publisher { Id = 1 });

            _publisherService.Update(new PublisherDTO { Id = 1, CompanyName = "p1" });

            _publisherRepositoryMock.Verify(c => c.Update(It.IsAny<Publisher>(), It.IsAny<int>()), Times.Once);
        }
    }
}