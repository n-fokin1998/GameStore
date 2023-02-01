using Autofac.Features.Indexed;
using GameStore.BusinessLogicLayer.Domain;
using GameStore.BusinessLogicLayer.Domain.Enums;
using GameStore.BusinessLogicLayer.Abstract.Payment;

namespace GameStore.BusinessLogicLayer.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IIndex<PaymentType, IPaymentStrategy> _strategies;

        public PaymentService(IIndex<PaymentType, IPaymentStrategy> strategies)
        {
            _strategies = strategies;
        }

        public PaymentResult Pay(PaymentInfo paymentInfo)
        {
            return _strategies[paymentInfo.PaymentType].Pay(paymentInfo);
        }
    }
}