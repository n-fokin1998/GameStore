using System;
using GameStore.BusinessLogicLayer.Domain.Enums;

namespace GameStore.BusinessLogicLayer.Domain
{
    public class PaymentResult
    {
        public bool Successed { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public Guid TransactionId { get; set; }

        public PaymentType PaymentType { get; set; }
    }
}