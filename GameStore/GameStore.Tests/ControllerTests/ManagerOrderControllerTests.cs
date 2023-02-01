using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Infrastructure;
using GameStore.Web.Areas.Manager.Controllers;
using GameStore.Web.Areas.Manager.ViewModels.Order;
using Moq;
using Xunit;

namespace GameStore.Tests.ControllerTests
{
    public class ManagerOrderControllerTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IOrderService> _orderServiceMock;
        private readonly ManagerOrderController _controller;

        public ManagerOrderControllerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _orderServiceMock = new Mock<IOrderService>();
            _controller = new ManagerOrderController(_orderServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public void ManageOrders_SomeData_ReturnsOrdersList()
        {
            _orderServiceMock.Setup(o => o.GetByDate(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(
                new List<OrderDTO>
                {
                    new OrderDTO { Id = 1 },
                    new OrderDTO { Id = 2 }
                });

            var result = _controller.Index() as ViewResult;
            var model = result.Model as List<OrderDTO>;

            Assert.Equal(2, model.Count);
        }

        [Fact]
        public void ChangeShippedStatus_SomeData_InvokeSomeAction()
        {
            _controller.ChangeShippedStatus(It.IsAny<int>(), It.IsAny<DateTime>());

            _orderServiceMock.Verify(c => c.ChangeShippedStatus(It.IsAny<int>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public void EditOrderDetailGet_SomeData_ReturnsViewWithModel()
        {
            _orderServiceMock.Setup(o => o.GetOrderDetailById(It.IsAny<int>())).Returns(new OrderDetailsDTO());
            _mapperMock.Setup(m => m.Map<OrderDetailsDTO, UpdateOrderDetailViewModel>(It.IsAny<OrderDetailsDTO>()))
                .Returns(new UpdateOrderDetailViewModel { Id = 1 });

            var result = _controller.EditOrderDetail(1) as ViewResult;
            var model = result.Model as UpdateOrderDetailViewModel;

            Assert.Equal(1, model.Id);
        }

        [Fact]
        public void EditOrderDetail_SomeData_ReturnsRedirectResult()
        {
            _orderServiceMock.Setup(o => o.UpdateOrderDetails(It.IsAny<OrderDetailsDTO>())).Returns(new OperationDetails(true));
            _mapperMock.Setup(m => m.Map<UpdateOrderDetailViewModel, OrderDetailsDTO>(It.IsAny<UpdateOrderDetailViewModel>()))
                .Returns(new OrderDetailsDTO { Id = 1 });

            var result = _controller.EditOrderDetail(new UpdateOrderDetailViewModel()) as RedirectToRouteResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void EditOrderDetail_ServiceError_ReturnsView()
        {
            _orderServiceMock.Setup(o => o.UpdateOrderDetails(It.IsAny<OrderDetailsDTO>())).Returns(new OperationDetails(false));
            _mapperMock.Setup(m => m.Map<UpdateOrderDetailViewModel, OrderDetailsDTO>(It.IsAny<UpdateOrderDetailViewModel>()))
                .Returns(new OrderDetailsDTO { Id = 1 });

            var result = _controller.EditOrderDetail(new UpdateOrderDetailViewModel()) as ViewResult;

            Assert.NotNull(result);
        }
    }
}