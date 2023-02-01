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
    public class PlatformTypeServiceTests
    {
        private readonly Mock<IRepository<PlatformType>> _platformTypeRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PlatformTypeService _platformTypeService;

        public PlatformTypeServiceTests()
        {
            _platformTypeRepositoryMock = new Mock<IRepository<PlatformType>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mongoUnitOfWorkMock = new Mock<IMongoUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            var mongoLoggerMock = new Mock<ILogRepository>();
            _platformTypeService = new PlatformTypeService(unitOfWorkMock.Object, _mapperMock.Object);
            unitOfWorkMock.Setup(uow => uow.PlatformTypes).Returns(_platformTypeRepositoryMock.Object);
            mongoUnitOfWorkMock.Setup(uow => uow.Logs).Returns(mongoLoggerMock.Object);
        }

        [Fact]
        public void AddPlatformType_NullPlatformType_ReturnFalseSucceeded()
        {
            var result = _platformTypeService.Add(null);

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void AddPlatformType_ExistentPlatformTypeWithSameName_ReturnFalseSucceeded()
        {
            _platformTypeRepositoryMock.Setup(g => g.GetList()).Returns(new List<PlatformType>
            {
                new PlatformType { Id = 1, TypeEn = "p1" }
            }.AsQueryable());

            var result = _platformTypeService.Add(new PlatformTypeDTO { Id = 1, TypeEn = "p1" });

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void AddPlatformType_RightPlatformType_ReturnTrueSucceeded()
        {
            _platformTypeService.Add(new PlatformTypeDTO { Id = 1, TypeEn = "p1" });

            _platformTypeRepositoryMock.Verify(c => c.Add(It.IsAny<PlatformType>()), Times.Once);
        }

        [Fact]
        public void GetPlatformTypesList_SomeData_ReturnsPlatformTypeList()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<PlatformType>, List<PlatformTypeDTO>>(It.IsAny<IEnumerable<PlatformType>>())).Returns(new List<PlatformTypeDTO>
            {
                new PlatformTypeDTO() { Id = 1 },
                new PlatformTypeDTO() { Id = 2 }
            });

            var result = _platformTypeService.GetList();

            Assert.Equal(1, result.FirstOrDefault().Id);
        }

        [Fact]
        public void GetPlatformTypesList_EmptyData_ReturnsEmptyList()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<PlatformType>, List<PlatformTypeDTO>>(
                It.IsAny<IEnumerable<PlatformType>>())).Returns(new List<PlatformTypeDTO>());

            var result = _platformTypeService.GetList();

            Assert.Empty(result);
        }

        [Fact]
        public void GetPlatformTypeById_SomeId_ReturnsPlatformType()
        {
            _mapperMock.Setup(m => m.Map<PlatformType, PlatformTypeDTO>(It.IsAny<PlatformType>()))
                .Returns(new PlatformTypeDTO() { Id = 1 });

            var result = _platformTypeService.GetById(It.IsAny<int>());

            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void DeletePlatformType_NullPlatformType_ReturnFalseSucceeded()
        {
            var result = _platformTypeService.Delete(null);

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void DeletePlatformType_NonExistenPlatformType_ReturnFalseSucceeded()
        {
            _platformTypeRepositoryMock.Setup(g => g.GetItem(1)).Returns((PlatformType)null);

            var result = _platformTypeService.Delete(new PlatformTypeDTO { Id = 1, TypeEn = "p1" });

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void DeletePlatformType_ExistingPlatformType_ReturnTrueSucceeded()
        {
            _platformTypeRepositoryMock.Setup(g => g.GetItem(1)).Returns(Mock.Of<PlatformType>());

            _platformTypeService.Delete(new PlatformTypeDTO { Id = 1, TypeEn = "p1" });

            _platformTypeRepositoryMock.Verify(c => c.Update(It.IsAny<PlatformType>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void UpdatePlatformType_NullPlatformType_ReturnFalseSucceeded()
        {
            var result = _platformTypeService.Update(null);

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void UpdatePlatformType_ExistentPlatformTypeWithSameName_ReturnFalseSucceeded()
        {
            _platformTypeRepositoryMock.Setup(g => g.GetList()).Returns(new List<PlatformType>
            {
                new PlatformType { Id = 2, TypeEn = "p1" }
            }.AsQueryable());

            var result = _platformTypeService.Update(new PlatformTypeDTO { Id = 1, TypeEn = "p1" });

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void UpdatePlatformType_RightPlatformType_ReturnTrueSucceeded()
        {
            _mapperMock.Setup(m => m.Map<PlatformTypeDTO, PlatformType>(It.IsAny<PlatformTypeDTO>())).Returns(Mock.Of<PlatformType>());
            _platformTypeRepositoryMock.Setup(g => g.GetItem(It.IsAny<int>())).Returns(Mock.Of<PlatformType>());

            _platformTypeService.Update(new PlatformTypeDTO { Id = 1, TypeEn = "p1" });

            _platformTypeRepositoryMock.Verify(c => c.Update(It.IsAny<PlatformType>(), It.IsAny<int>()), Times.Once);
        }
    }
}