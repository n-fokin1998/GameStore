using System;
using System.Collections.Generic;
using GameStore.BusinessLogicLayer.DTO;

namespace GameStore.Web.ViewModels
{
    public class OrderHistoryViewModel
    {
        public int Id { get; set; }

        public string CustomerId { get; set; }

        public int ShipperId { get; set; }

        public ShipperDTO Shipper { get; set; }

        public DateTime Date { get; set; }

        public List<OrderDetailsDTO> OrderDetails { get; set; }
    }
}