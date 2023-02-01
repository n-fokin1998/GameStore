using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Services;
using GameStore.Domain.Mongo.Abstract;
using GameStore.Domain.Mongo.Entities;
using Moq;
using Xunit;

namespace GameStore.Tests.ServiceTests
{
    public class ShipperServiceTests
    {
        private readonly ShipperService _shipperService;

        public ShipperServiceTests()
        {
            var shipperRepositoryMock = new Mock<IMongoRepository<Shipper>>();
            var mongoUnitOfWorkMock = new Mock<IMongoUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            _shipperService = new ShipperService(mongoUnitOfWorkMock.Object, mapperMock.Object);
            mapperMock.Setup(m => m.Map<IEnumerable<Shipper>, List<ShipperDTO>>(It.IsAny<IEnumerable<Shipper>>()))
                .Returns(new List<ShipperDTO>
            {
                new ShipperDTO { ShipperId = 1 }
            });
            mongoUnitOfWorkMock.Setup(m => m.Shippers).Returns(shipperRepositoryMock.Object);
        }

        [Fact]
        public void GetShippersList_SomeData_ReturnsList()
        {
            var result = _shipperService.GetList();

            Assert.Equal(1, result.FirstOrDefault().ShipperId);
        }
    }
}