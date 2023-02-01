using System;
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
    public class OrderServiceTests
    {
        private readonly Mock<IRepository<Order>> _orderRepositoryMock;
        private readonly Mock<IRepository<Game>> _gameRepositoryMock;
        private readonly Mock<IRepository<OrderDetails>> _orderDetailsRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _orderRepositoryMock = new Mock<IRepository<Order>>();
            _gameRepositoryMock = new Mock<IRepository<Game>>();
            _orderDetailsRepositoryMock = new Mock<IRepository<OrderDetails>>();
            var mongoOrderMock = new Mock<IMongoRepository<Domain.Mongo.Entities.Order>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mongoUnitOfWorkMock = new Mock<IMongoUnitOfWork>();
            var mongoLoggerMock = new Mock<ILogRepository>();
            _mapperMock = new Mock<IMapper>();
            _orderService = new OrderService(unitOfWorkMock.Object, mongoUnitOfWorkMock.Object, _mapperMock.Object);
            unitOfWorkMock.Setup(uow => uow.Orders).Returns(_orderRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.Games).Returns(_gameRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.OrderDetails).Returns(_orderDetailsRepositoryMock.Object);
            mongoUnitOfWorkMock.Setup(m => m.Orders).Returns(mongoOrderMock.Object);
            mongoUnitOfWorkMock.Setup(m => m.Logs).Returns(mongoLoggerMock.Object);
        }

        [Fact]
        public void AddOrder_NullOrder_ReturnFalseSucceeded()
        {
            var result = _orderService.Add(null);

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void AddOrder_GameOutOfStock_ReturnFalseSucceeded()
        {
            _mapperMock.Setup(m => m.Map<OrderDTO, Order>(It.IsAny<OrderDTO>())).Returns(new Order());
            _mapperMock.Setup(m => m.Map<IEnumerable<OrderDetailsDTO>, List<OrderDetails>>(
                It.IsAny<IEnumerable<OrderDetailsDTO>>())).Returns(new List<OrderDetails>
            {
                new OrderDetails { GameId = 1 }
            });
            _gameRepositoryMock.Setup(g => g.GetItem(1)).Returns(new Game() { Id = 1, UnitsInStock = -1 });

            var result = _orderService.Add(new OrderDTO
            {
                Id = 1,
                Date = DateTime.UtcNow,
                CustomerId = "c1",
                OrderDetails = new List<OrderDetailsDTO> { new OrderDetailsDTO { GameId = 1, Quantity = 5 } }
            });

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void AddOrder_SomeOrder_ReturnTrueSucceeded()
        {
            _mapperMock.Setup(m => m.Map<OrderDTO, Order>(It.IsAny<OrderDTO>())).Returns(new Order());
            _mapperMock.Setup(m => m.Map<IEnumerable<OrderDetailsDTO>, List<OrderDetails>>(
                It.IsAny<IEnumerable<OrderDetailsDTO>>())).Returns(new List<OrderDetails>
            {
                new OrderDetails { GameId = 1 }
            });
            _gameRepositoryMock.Setup(g => g.GetItem(1)).Returns(Mock.Of<Game>());

            var result = _orderService.Add(new OrderDTO()
            {
                Id = 1,
                Date = DateTime.UtcNow,
                CustomerId = "c1",
                OrderDetails = new List<OrderDetailsDTO>()
            });

            Assert.True(result.Succeeded);
            _orderRepositoryMock.Verify(c => c.Add(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public void GetOrdersList_SomeData_ReturnsOrdersList()
        {
            SetData();

            var result = _orderService.GetList();

            Assert.Equal(4, result.Count());
        }

        [Fact]
        public void GetOrdersByDateList_DateRange_ReturnsOrdersList()
        {
            SetData();

            var result = _orderService.GetByDate(new DateTime(2018, 1, 1), new DateTime());

            Assert.Equal(4, result.Count());
        }

        [Fact]
        public void GetOrdersByDateList_StartDateIsNull_ReturnsOrdersList()
        {
            SetData();

            var result = _orderService.GetByDate(null, new DateTime());

            Assert.Equal(4, result.Count());
        }

        [Fact]
        public void GetOrdersByDateList_FinalDateIsNull_ReturnsOrdersList()
        {
            SetData();

            var result = _orderService.GetByDate(new DateTime(2018, 1, 1), null);

            Assert.Equal(4, result.Count());
        }

        [Fact]
        public void GetOrderDetailById_SomeId_ReturnsOrderDetails()
        {
            _orderDetailsRepositoryMock.Setup(o => o.GetItem(1)).Returns(new OrderDetails { Id = 1, GameId = 1 });
            _mapperMock.Setup(m => m.Map<OrderDetails, OrderDetailsDTO>(It.IsAny<OrderDetails>()))
                .Returns(new OrderDetailsDTO { Id = 1, GameId = 1 });
            _gameRepositoryMock.Setup(g => g.GetItem(1)).Returns(new Game { NameEn = "g1" });

            var result = _orderService.GetOrderDetailById(1);

            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void UpdateOrder_SomeData_InvokeUpdateAction()
        {
            _mapperMock.Setup(m => m.Map<OrderDTO, Order>(It.IsAny<OrderDTO>())).Returns(Mock.Of<Order>());
            _orderRepositoryMock.Setup(g => g.GetItem(It.IsAny<int>())).Returns(Mock.Of<Order>());

            _orderService.Update(new OrderDTO
            {
                Id = 1,
                OrderDetails = new List<OrderDetailsDTO>
                {
                    new OrderDetailsDTO
                    {
                        Id = 1,
                        GameId = 1,
                        Quantity = 2
                    },
                    new OrderDetailsDTO
                    {
                        Id = 2,
                        GameId = 2,
                        Quantity = 1
                    },
                }
            });

            _orderRepositoryMock.Verify(c => c.Update(It.IsAny<Order>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void UpdateOrderDetails_SomeData_InvokeUpdateAction()
        {
            _mapperMock.Setup(m => m.Map<OrderDetailsDTO, OrderDetails>(It.IsAny<OrderDetailsDTO>()))
                .Returns(Mock.Of<OrderDetails>());
            _gameRepositoryMock.Setup(g => g.GetItem(It.IsAny<int>())).Returns(new Game { Id = 1, Price = 5 });

            _orderService.UpdateOrderDetails(new OrderDetailsDTO
            {
                Id = 1,
                GameId = 1,
                Quantity = 2
            });

            _orderDetailsRepositoryMock.Verify(c => c.Update(It.IsAny<OrderDetails>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void DeleteOrderDetails_SomeData_InvokeUpdateAction()
        {
            _orderDetailsRepositoryMock.Setup(g => g.GetItem(It.IsAny<int>())).Returns(new OrderDetails { Id = 1 });

            _orderService.DeleteOrderDetails(new OrderDetailsDTO
            {
                Id = 1,
                GameId = 1,
                Quantity = 2
            });

            _orderDetailsRepositoryMock.Verify(c => c.Update(It.IsAny<OrderDetails>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void ChangeShippedStatus_SomeData_InvokeUpdateAction()
        {
            _mapperMock.Setup(m => m.Map<OrderDTO, Order>(It.IsAny<OrderDTO>())).Returns(Mock.Of<Order>());
            _orderRepositoryMock.Setup(g => g.GetItem(It.IsAny<int>())).Returns(new Order { Id = 1 });

            _orderService.ChangeShippedStatus(1, DateTime.Now);

            _orderRepositoryMock.Verify(c => c.Update(It.IsAny<Order>(), It.IsAny<int>()), Times.Once);
        }

        private void SetData()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<Order>, List<OrderDTO>>(It.IsAny<IEnumerable<Order>>()))
                .Returns(new List<OrderDTO>
            {
                new OrderDTO { Id = 1, Date = new DateTime(2018, 1, 21) },
                new OrderDTO { Id = 2, Date = new DateTime(2018, 5, 1) }
            });
            _mapperMock.Setup(m => m.Map<IEnumerable<Domain.Mongo.Entities.Order>, List<OrderDTO>>(
                It.IsAny<IEnumerable<Domain.Mongo.Entities.Order>>())).Returns(new List<OrderDTO>
            {
                new OrderDTO { Id = 1, Date = new DateTime(2018, 1, 21) },
                new OrderDTO { Id = 2, Date = new DateTime(2018, 5, 1) }
            });
        }
    }
}