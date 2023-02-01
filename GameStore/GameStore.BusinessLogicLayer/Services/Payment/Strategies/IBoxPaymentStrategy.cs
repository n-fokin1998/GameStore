using System;
using System.Linq;
using GameStore.BusinessLogicLayer.Domain;
using GameStore.BusinessLogicLayer.Domain.Enums;
using GameStore.BusinessLogicLayer.Abstract.Payment;

namespace GameStore.BusinessLogicLayer.Services.Payment.Strategies
{
    public class IBoxPaymentStrategy : IPaymentStrategy
    {
        public PaymentResult Pay(PaymentInfo paymentInfo)
        {
            return new PaymentResult
            {
                Successed = true,
                Amount = paymentInfo.OrderDetails.Sum(o => o.Price),
                Date = paymentInfo.Date,
                TransactionId = Guid.NewGuid(),
                PaymentType = PaymentType.IBox
            };
        }
    }
}