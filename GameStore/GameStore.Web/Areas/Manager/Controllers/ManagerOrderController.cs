using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.Web.Areas.Manager.ViewModels.Order;
using GameStore.Web.Controllers;

namespace GameStore.Web.Areas.Manager.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerOrderController : BaseController
    {
        private const string ErrorMessage = "Not found!";
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public ManagerOrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            var orders = _orderService.GetByDate(DateTime.Now.AddDays(-30), DateTime.Now);

            return View(orders);
        }

        public ActionResult OrderDetails(int id)
        {
            var order = _orderService.GetList().FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                throw new HttpException(404, ErrorMessage);
            }

            return View(order);
        }

        public ActionResult ChangeShippedStatus(int orderId, DateTime? shippedDate)
        {
            if (shippedDate != null)
            {
                _orderService.ChangeShippedStatus(orderId, shippedDate.Value);
            }

            return RedirectToAction("Index", "ManagerOrder", new { area = "Manager" });
        }

        [HttpGet]
        public ActionResult EditOrderDetail(int id)
        {
            var orderDetail = _orderService.GetOrderDetailById(id);
            if (orderDetail == null)
            {
                throw new HttpException(404, ErrorMessage);
            }

            var model = _mapper.Map<OrderDetailsDTO, UpdateOrderDetailViewModel>(orderDetail);

            return View(model);
        }

        [HttpPost]
        public ActionResult EditOrderDetail(UpdateOrderDetailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var orderDetails = _mapper.Map<UpdateOrderDetailViewModel, OrderDetailsDTO>(model);
            var result = _orderService.UpdateOrderDetails(orderDetails);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "ManagerOrder");
            }

            ModelState.AddModelError(result.Property ?? nameof(model.Quantity), result.Message);

            return View(model);
        }

        [HttpGet]
        public ActionResult DeleteOrderDetail(int id)
        {
            var orderDetail = _orderService.GetOrderDetailById(id);
            if (orderDetail == null)
            {
                throw new HttpException(404, ErrorMessage);
            }

            var model = new DeleteOrderDetailViewModel { Id = id, OrderId = orderDetail.OrderId };

            return PartialView("DeleteOrderDetailModalWindow", model);
        }

        [HttpPost]
        public ActionResult DeleteOrderDetail(DeleteOrderDetailViewModel model)
        {
            var orderDetail = _orderService.GetOrderDetailById(model.Id);
            _orderService.DeleteOrderDetails(orderDetail);

            return RedirectToAction("Index", "ManagerOrder");
        }
    }
}