using System;
using System.Collections.Generic;

namespace GameStore.BusinessLogicLayer.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }

        public string CustomerId { get; set; }

        public int ShipperId { get; set; }

        public DateTime Date { get; set; }

        public DateTime? ShippedDate { get; set; }

        public List<OrderDetailsDTO> OrderDetails { get; set; }
    }
}