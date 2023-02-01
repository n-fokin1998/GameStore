using System;
using System.Security.Authentication;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.Domain;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Infrastructure;
using GameStore.BusinessLogicLayer.Abstract.Payment;
using GameStore.Web.Controllers;
using GameStore.Web.Infrastructure;
using GameStore.Web.ViewModels;
using Moq;
using Xunit;

namespace GameStore.Tests.ControllerTests
{
    public class BasketControllerTests
    {
        private readonly Mock<IGameService> _gameServiceMock;
        private readonly Mock<IOrderService> _orderServiceMock;
        private readonly Mock<IPaymentService> _paymentServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly BasketController _controller;
        private readonly PaymentResult _successPaymentResult;
        private readonly PaymentResult _failPaymentResult;

        public BasketControllerTests()
        {
            var moqContext = new Mock<HttpContextBase>();
            var moqController = new Mock<ControllerContext>();
            moqContext.Setup(x => x.Session).Returns(Mock.Of<HttpSessionStateBase>());
            moqController.Setup(c => c.HttpContext).Returns(moqContext.Object);
            _gameServiceMock = new Mock<IGameService>();
            _orderServiceMock = new Mock<IOrderService>();
            var shipperServiceMock = new Mock<IShipperService>();
            _paymentServiceMock = new Mock<IPaymentService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new BasketController(
                _gameServiceMock.Object,
                _orderServiceMock.Object,
                _paymentServiceMock.Object,
                _mapperMock.Object,
                shipperServiceMock.Object,
                new FileSystemAccess());
            _controller.ControllerContext = moqController.Object;
            _successPaymentResult = new PaymentResult { Successed = true };
            _failPaymentResult = new PaymentResult { Successed = false };
        }

        [Fact]
        public void BasketIndex_SomeData_ReturnsViewResult()
        {
            SetData();

            var result = _controller.Index();

            Assert.NotNull(result);
        }

        [Fact]
        public void BuyProduct_RightKey_ReturnsRedirectResult()
        {
            SetData();

            var result = _controller.BuyProduct("g1") as RedirectToRouteResult;

            Assert.Equal("Index", result.RouteValues["action"].ToString());
            Assert.Equal("Basket", result.RouteValues["controller"].ToString());
        }

        [Fact]
        public void BuyProduct_BadGameKey_ReturnsRedirectResult()
        {
            var ex = Assert.Throws<HttpException>(() => _controller.BuyProduct(It.IsAny<string>()));

            Assert.NotNull(ex);
        }

        [Fact]
        public void MakeOrderGet_SomeData_ReturnsViewResult()
        {
            var result = _controller.MakeOrder();

            Assert.NotNull(result.Model);
        }

        [Fact]
        public void ShowIBoxViewGet_SomeModel_ReturnsViewResult()
        {
            var result = _controller.ShowIBoxView(new PaymentViewModel()) as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void ShowVisaViewGet_SomeModel_ReturnsViewResult()
        {
            var result = _controller.ShowVisaView(new PaymentViewModel()) as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void BankTerminal_PaymentServiceError_ReturnsViewResult()
        {
            _paymentServiceMock.Setup(p => p.Pay(It.IsAny<PaymentInfo>())).Returns(_failPaymentResult);

            _controller.BankTerminal(new PaymentViewModel { AccountNumber = "a1", Date = DateTime.UtcNow });

            _paymentServiceMock.Verify(srv => srv.Pay(It.IsAny<PaymentInfo>()), Times.Once);
        }

        [Fact]
        public void BankTerminal_OrderServiceError_ReturnsViewResult()
        {
            _mapperMock.Setup(m => m.Map<PaymentViewModel, PaymentInfo>(It.IsAny<PaymentViewModel>()))
                .Returns(new PaymentInfo());
            _paymentServiceMock.Setup(p => p.Pay(It.IsAny<PaymentInfo>())).Returns(_successPaymentResult);
            _orderServiceMock.Setup(p => p.Add(It.IsAny<OrderDTO>())).Returns(new OperationDetails(false));

            _controller.BankTerminal(new PaymentViewModel { AccountNumber = "a1", Date = DateTime.UtcNow });

            _paymentServiceMock.Verify(srv => srv.Pay(It.IsAny<PaymentInfo>()), Times.Once);
        }

        [Fact]
        public void IBoxTerminal_PaymentServiceError_ReturnsViewResult()
        {
            _paymentServiceMock.Setup(p => p.Pay(It.IsAny<PaymentInfo>())).Returns(_failPaymentResult);

            _controller.IBoxTerminal(new PaymentViewModel { AccountNumber = "a1", Date = DateTime.UtcNow });

            _paymentServiceMock.Verify(srv => srv.Pay(It.IsAny<PaymentInfo>()), Times.Once);
        }

        [Fact]
        public void IBoxTerminal_SomePaymentResult_MethodInvoked()
        {
            _mapperMock.Setup(m => m.Map<PaymentViewModel, PaymentInfo>(It.IsAny<PaymentViewModel>()))
                .Returns(new PaymentInfo());
            _paymentServiceMock.Setup(p => p.Pay(It.IsAny<PaymentInfo>())).Returns(_successPaymentResult);
            _orderServiceMock.Setup(p => p.Add(It.IsAny<OrderDTO>())).Returns(new OperationDetails(true));

            _controller.IBoxTerminal(new PaymentViewModel { AccountNumber = "a1", Date = DateTime.UtcNow });

            _paymentServiceMock.Verify(srv => srv.Pay(It.IsAny<PaymentInfo>()), Times.Once);
        }

        [Fact]
        public void IBoxTerminal_SomePaymentResult_ReturnsViewModel()
        {
            _mapperMock.Setup(m => m.Map<PaymentViewModel, PaymentInfo>(It.IsAny<PaymentViewModel>()))
                .Returns(new PaymentInfo());
            _paymentServiceMock.Setup(p => p.Pay(It.IsAny<PaymentInfo>())).Returns(_successPaymentResult);
            _orderServiceMock.Setup(p => p.Add(It.IsAny<OrderDTO>())).Returns(new OperationDetails(true));

            var result = _controller.IBoxTerminal(
                new PaymentViewModel { AccountNumber = "a1", Date = DateTime.UtcNow });
            var model = (PaymentViewModel)result.Model;

            Assert.Equal(1, model.Successed);
        }

        [Fact]
        public void VisaTerminal_PaymentServiceError_ReturnsViewResult()
        {
            _paymentServiceMock.Setup(p => p.Pay(It.IsAny<PaymentInfo>())).Returns(_failPaymentResult);

            _controller.VisaTerminal(new PaymentViewModel { AccountNumber = "a1", Date = DateTime.UtcNow });

            _paymentServiceMock.Verify(srv => srv.Pay(It.IsAny<PaymentInfo>()), Times.Once);
        }

        [Fact]
        public void VisaTerminal_SomePaymentResult_MethodInvoked()
        {
            _mapperMock.Setup(m => m.Map<PaymentViewModel, PaymentInfo>(It.IsAny<PaymentViewModel>()))
                .Returns(new PaymentInfo());
            _paymentServiceMock.Setup(p => p.Pay(It.IsAny<PaymentInfo>())).Returns(_successPaymentResult);
            _orderServiceMock.Setup(p => p.Add(It.IsAny<OrderDTO>())).Returns(new OperationDetails(true));

            _controller.VisaTerminal(new PaymentViewModel { AccountNumber = "a1", Date = DateTime.UtcNow });

            _paymentServiceMock.Verify(srv => srv.Pay(It.IsAny<PaymentInfo>()), Times.Once);
        }

        [Fact]
        public void VisaTerminal_SomePaymentResult_ReturnsViewModel()
        {
            _mapperMock.Setup(m => m.Map<PaymentViewModel, PaymentInfo>(It.IsAny<PaymentViewModel>()))
                .Returns(new PaymentInfo());
            _paymentServiceMock.Setup(p => p.Pay(It.IsAny<PaymentInfo>())).Returns(_successPaymentResult);
            _orderServiceMock.Setup(p => p.Add(It.IsAny<OrderDTO>())).Returns(new OperationDetails(true));

            var result = _controller.VisaTerminal(
                new PaymentViewModel { AccountNumber = "a1", Date = DateTime.UtcNow });
            var model = (PaymentViewModel)result.Model;

            Assert.Equal(1, model.Successed);
        }

        private void SetData()
        {
            _gameServiceMock.Setup(g => g.GetByKey("g1"))
                .Returns(new GameDTO { Id = 1, Key = "g1" });
        }
    }
}