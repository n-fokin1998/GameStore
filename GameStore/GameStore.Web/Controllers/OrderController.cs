using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.Web.App_LocalResources;
using GameStore.Web.ViewModels;

namespace GameStore.Web.Controllers
{
    [Authorize(Roles = "Administrator, Manager, Moderator")]
    public class OrderController : BaseController
    {
        private static readonly string DateErrorMessage = GlobalRes.DateErrorMessage;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        public ActionResult OrdersHistory(DateTime? from, DateTime? to)
        {
            if (from > to)
            {
                ModelState.AddModelError("from", DateErrorMessage);

                return View(new List<OrderHistoryViewModel>());
            }

            var orders = _orderService.GetByDate(from, to);
            var model = _mapper.Map<IEnumerable<OrderDTO>, List<OrderHistoryViewModel>>(orders);

            return View(model);
        }
    }
}