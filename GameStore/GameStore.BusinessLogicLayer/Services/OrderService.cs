using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.App_LocalResources;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Infrastructure;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.Domain.Mongo.Abstract;
using MongoOrder = GameStore.Domain.Mongo.Entities.Order;
using Order = GameStore.Domain.Entities.Order;

namespace GameStore.BusinessLogicLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMongoUnitOfWork _mongoUnitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMongoUnitOfWork mongoUnitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mongoUnitOfWork = mongoUnitOfWork;
            _mapper = mapper;
        }

        public OrderDetailsDTO GetOrderDetailById(int id)
        {
            var orderDetail = _unitOfWork.OrderDetails.GetItem(id);
            var orderDetailDto = _mapper.Map<OrderDetails, OrderDetailsDTO>(orderDetail);
            orderDetailDto.GameName = _unitOfWork.Games.GetItem(orderDetail.GameId).NameEn;

            return orderDetailDto;
        }

        public OperationDetails Add(OrderDTO orderDto)
        {
            if (orderDto == null)
            {
                return new OperationDetails(false, BLLRes.OrderNotFound, null);
            }

            var order = _mapper.Map<OrderDTO, Order>(orderDto);
            order.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDTO>, List<OrderDetails>>(orderDto.OrderDetails);
            foreach (var orderDetail in order.OrderDetails)
            {
                var game = _unitOfWork.Games.GetItem(orderDetail.GameId);
                game.UnitsInStock -= orderDetail.Quantity;
                if (game.UnitsInStock < 0)
                {
                    return new OperationDetails(false, BLLRes.OutOfStock, null);
                }

                _unitOfWork.Games.Update(game, game.Id);
            }

            _unitOfWork.Orders.Add(order);
            _unitOfWork.Commit();

            return new OperationDetails(true);
        }

        public IEnumerable<OrderDTO> GetList()
        {
            var orders = _unitOfWork.Orders.GetList().ToList();
            foreach (var order in orders)
            {
                order.OrderDetails = order.OrderDetails.Where(o => !o.IsDeleted).ToList();
            }

            var mongoOrders = _mongoUnitOfWork.Orders.GetList().ToList();
            var result = _mapper.Map<IEnumerable<Order>, List<OrderDTO>>(orders);
            result.AddRange(_mapper.Map<IEnumerable<MongoOrder>, List<OrderDTO>>(mongoOrders));

            return result;
        }

        public IEnumerable<OrderDTO> GetByDate(DateTime? from, DateTime? to)
        {
            var orders = _unitOfWork.Orders.GetList();
            var mongoOrders = _mongoUnitOfWork.Orders.GetList().ToList();
            if (from == null && to == null)
            {
                var date = DateTime.Now.AddDays(-30);
                orders = orders.Where(o => o.Date <= date);
                mongoOrders = mongoOrders.Where(o => o.OrderDate <= date).ToList();
            }
            else
            {
                orders = from.HasValue && to.HasValue 
                    ? orders.Where(o => o.Date >= from && o.Date <= to)
                    : (!from.HasValue ? orders.Where(o => o.Date <= to) : orders.Where(o => o.Date >= from));
                mongoOrders = from.HasValue && to.HasValue 
                    ? mongoOrders.Where(o => o.OrderDate >= from && o.OrderDate <= to).ToList() 
                    : (!from.HasValue
                    ? mongoOrders.Where(o => o.OrderDate <= to).ToList() 
                    : mongoOrders.Where(o => o.OrderDate >= from).ToList());
            }

            var orderList = orders.ToList();
            foreach (var order in orderList)
            {
                order.OrderDetails = order.OrderDetails.Where(o => !o.IsDeleted).ToList();
            }

            var result = _mapper.Map<IEnumerable<Order>, List<OrderDTO>>(orderList);
            result.AddRange(_mapper.Map<IEnumerable<MongoOrder>, List<OrderDTO>>(mongoOrders));

            return result;
        }

        public OperationDetails Update(OrderDTO orderDto)
        {
            if (orderDto == null)
            {
                return new OperationDetails(false, BLLRes.OrderNotFound, null);
            }

            var order = _mapper.Map<OrderDTO, Order>(orderDto);
            AttachOrderDetailsCollection(orderDto.OrderDetails, order);
            _unitOfWork.Orders.Update(order, order.Id);
            _unitOfWork.Commit();

            return new OperationDetails(true);
        }

        public OperationDetails UpdateOrderDetails(OrderDetailsDTO orderDetailsDto)
        {
            if (orderDetailsDto == null)
            {
                return new OperationDetails(false, BLLRes.OrderDetailsNotFound, null);
            }

            var orderDetails = _mapper.Map<OrderDetailsDTO, OrderDetails>(orderDetailsDto);
            orderDetails.Price = orderDetails.Quantity * _unitOfWork.Games.GetItem(orderDetails.GameId).Price;
            _unitOfWork.OrderDetails.Update(orderDetails, orderDetails.Id);
            _unitOfWork.Commit();

            return new OperationDetails(true);
        }

        public OperationDetails DeleteOrderDetails(OrderDetailsDTO orderDetailsDto)
        {
            if (orderDetailsDto == null)
            {
                return new OperationDetails(false, BLLRes.OrderDetailsNotFound, null);
            }

            var orderDetail = _unitOfWork.OrderDetails.GetItem(orderDetailsDto.Id);
            if (orderDetail == null)
            {
                return new OperationDetails(false, BLLRes.OrderDetailsNotFound, null);
            }

            orderDetail.IsDeleted = true;
            _unitOfWork.OrderDetails.Update(orderDetail, orderDetail.Id);
            _unitOfWork.Commit();

            return new OperationDetails(true);
        }

        public OperationDetails ChangeShippedStatus(int orderId, DateTime shippedDate)
        {
            var order = _unitOfWork.Orders.GetItem(orderId);
            if (order == null)
            {
                return new OperationDetails(false, BLLRes.OrderNotFound, null);
            }

            order.ShippedDate = shippedDate;
            _unitOfWork.Orders.Update(order, order.Id);
            _unitOfWork.Commit();

            return new OperationDetails(true);
        }

        private void AttachOrderDetailsCollection(IEnumerable<OrderDetailsDTO> orderDetails, Order order)
        {
            order.OrderDetails = new List<OrderDetails>();
            var orderDetailsIds = orderDetails.Select(o => o.Id).ToList();
            var existingOrderDetails = _unitOfWork.OrderDetails.GetList()
                .Where(o => orderDetailsIds.Any(i => i == o.Id)).ToList();
            foreach (var orderDetail in existingOrderDetails)
            {
                order.OrderDetails.Add(orderDetail);
            }
        }
    }
}