using System;
using System.Collections.Generic;

namespace GameStore.Domain.Entities
{
    public class Order : EntityBase
    {
        public Order()
        {
            OrderDetails = new List<OrderDetails>();
        }

        public string CustomerId { get; set; }

        public int ShipperId { get; set; }

        public DateTime Date { get; set; }

        public DateTime? ShippedDate { get; set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}