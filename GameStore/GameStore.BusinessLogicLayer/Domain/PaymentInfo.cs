using System;
using System.Collections.Generic;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Domain.Enums;

namespace GameStore.BusinessLogicLayer.Domain
{
    public class PaymentInfo
    {
        public PaymentType PaymentType { get; set; }

        public DateTime Date { get; set; }

        public int InvoiceNumber { get; set; }

        public string AccountNumber { get; set; }

        public string CardHolder { get; set; }

        public string CardNumber { get; set; }

        public DateTime ExpiresDate { get; set; }

        public short SecureCode { get; set; }

        public List<OrderDetailsDTO> OrderDetails { get; set; }
    }
}