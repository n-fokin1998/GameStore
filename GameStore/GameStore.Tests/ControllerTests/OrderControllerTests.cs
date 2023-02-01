using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.Web.Controllers;
using GameStore.Web.ViewModels;
using Moq;
using Xunit;

namespace GameStore.Tests.ControllerTests
{
    public class OrderControllerTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _mapperMock = new Mock<IMapper>();
            var orderServiceMock = new Mock<IOrderService>();
            _controller = new OrderController(orderServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public void OrdersHistory_SomeDateRange_ReturnsViewResult()
        {
            var orderViewModel = new OrderHistoryViewModel()
            {
                Id = 1,
                CustomerId = "1",
                Date = new DateTime(),
                OrderDetails = new List<OrderDetailsDTO>()
            };
            _mapperMock.Setup(m => m.Map<IEnumerable<OrderDTO>, List<OrderHistoryViewModel>>(
                It.IsAny<IEnumerable<OrderDTO>>())).Returns(new List<OrderHistoryViewModel>
            {
                orderViewModel
            });
            _mapperMock.Setup(m => m.Map<IEnumerable<OrderDTO>, List<OrderHistoryViewModel>>(
                It.IsAny<IEnumerable<OrderDTO>>())).Returns(new List<OrderHistoryViewModel>
            {
                orderViewModel
            });

            var result = _controller.OrdersHistory(It.IsAny<DateTime>(), It.IsAny<DateTime>()) as ViewResult;
            var model = result.Model as List<OrderHistoryViewModel>;

            Assert.NotEmpty(model);
            Assert.Equal(model.FirstOrDefault().Id, orderViewModel.Id);
            Assert.Equal(model.FirstOrDefault().CustomerId, orderViewModel.CustomerId);
            Assert.Equal(model.FirstOrDefault().Date, orderViewModel.Date);
            Assert.Equal(model.FirstOrDefault().OrderDetails, orderViewModel.OrderDetails);
        }

        [Fact]
        public void OrdersHistory_InvalidDateRange_ReturnsViewResultWithNullModel()
        {
            var result = _controller.OrdersHistory(DateTime.Now, DateTime.MinValue) as ViewResult;
            var model = result.Model as List<OrderHistoryViewModel>;

            Assert.Empty(model);
        }
    }
}