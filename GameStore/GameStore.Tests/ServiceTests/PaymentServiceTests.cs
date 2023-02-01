using System;
using System.Collections.Generic;
using Autofac.Features.Indexed;
using GameStore.BusinessLogicLayer.Abstract.Payment;
using GameStore.BusinessLogicLayer.Domain;
using GameStore.BusinessLogicLayer.Domain.Enums;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Services.Payment;
using GameStore.BusinessLogicLayer.Services.Payment.Strategies;
using Moq;
using Xunit;

namespace GameStore.Tests.ServiceTests
{
    public class PaymentServiceTests
    {
        private readonly Mock<IIndex<PaymentType, IPaymentStrategy>> _strategiesMock;
        private readonly PaymentService _paymentService;

        public PaymentServiceTests()
        {
            _strategiesMock = new Mock<IIndex<PaymentType, IPaymentStrategy>>();
            _paymentService = new PaymentService(_strategiesMock.Object);
        }

        [Fact]
        public void BankPay_SomeData_ReturnsPaymentResult()
        {
            _strategiesMock.Setup(s => s[PaymentType.Bank]).Returns(new BankPaymentStrategy());

            var result = _paymentService.Pay(new PaymentInfo
            {
                PaymentType = PaymentType.Bank,
                Date = DateTime.Now,
                OrderDetails = new List<OrderDetailsDTO>()
                {
                    new OrderDetailsDTO() { Price = 10 }
                }
            });

            Assert.Equal(PaymentType.Bank, result.PaymentType);
        }

        [Fact]
        public void IBoxPay_SomeData_ReturnsPaymentResult()
        {
            _strategiesMock.Setup(s => s[PaymentType.IBox]).Returns(new IBoxPaymentStrategy());

            var result = _paymentService.Pay(new PaymentInfo
            {
                PaymentType = PaymentType.IBox,
                Date = DateTime.Now,
                OrderDetails = new List<OrderDetailsDTO>()
                {
                    new OrderDetailsDTO() { Price = 10 }
                }
            });

            Assert.Equal(PaymentType.IBox, result.PaymentType);
        }

        [Fact]
        public void VisaPay_SomeData_ReturnsPaymentResult()
        {
            _strategiesMock.Setup(s => s[PaymentType.Visa]).Returns(new VisaPaymentStrategy());

            var result = _paymentService.Pay(new PaymentInfo
            {
                PaymentType = PaymentType.Visa,
                Date = DateTime.Now,
                OrderDetails = new List<OrderDetailsDTO>()
                {
                    new OrderDetailsDTO() { Price = 10 }
                }
            });

            Assert.Equal(PaymentType.Visa, result.PaymentType);
        }
    }
}