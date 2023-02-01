using System;
using System.Collections.Generic;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Infrastructure;

namespace GameStore.BusinessLogicLayer.Abstract.Services
{
    public interface IOrderService
    {
        OrderDetailsDTO GetOrderDetailById(int id);

        OperationDetails Add(OrderDTO orderDto);

        IEnumerable<OrderDTO> GetList();

        IEnumerable<OrderDTO> GetByDate(DateTime? from, DateTime? to);

        OperationDetails Update(OrderDTO orderDto);

        OperationDetails UpdateOrderDetails(OrderDetailsDTO orderDetailsDto);

        OperationDetails DeleteOrderDetails(OrderDetailsDTO orderDetailsDto);

        OperationDetails ChangeShippedStatus(int orderId, DateTime shippedDate);
    }
}