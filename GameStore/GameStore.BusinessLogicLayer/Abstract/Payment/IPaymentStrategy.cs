using GameStore.BusinessLogicLayer.Domain;

namespace GameStore.BusinessLogicLayer.Abstract.Payment
{
    public interface IPaymentStrategy
    {
        PaymentResult Pay(PaymentInfo paymentInfo);
    }
}