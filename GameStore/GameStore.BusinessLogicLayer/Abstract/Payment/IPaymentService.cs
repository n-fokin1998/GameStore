using GameStore.BusinessLogicLayer.Domain;

namespace GameStore.BusinessLogicLayer.Abstract.Payment
{
    public interface IPaymentService
    {
        PaymentResult Pay(PaymentInfo paymentInfo);
    }
}