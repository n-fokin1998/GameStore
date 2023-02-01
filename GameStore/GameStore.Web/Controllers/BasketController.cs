using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.Domain;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Abstract.Payment;
using GameStore.Web.Infrastructure;
using GameStore.Web.ViewModels;
using OrderDTO = GameStore.BusinessLogicLayer.DTO.OrderDTO;

namespace GameStore.Web.Controllers
{
    public class BasketController : BaseController
    {
        private const string ErrorMessage = "Not found!";
        private const string InvoiceFileType = "text/plain";
        private const string InvoiceFileName = "Bill.txt";
        private readonly IGameService _gameService;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly IShipperService _shipperService;
        private readonly IMapper _mapper;
        private readonly FileSystemAccess _fileSystemAccess;

        public BasketController(
            IGameService gameService,
            IOrderService orderService,
            IPaymentService paymentService,
            IMapper mapper,
            IShipperService shipperService,
            FileSystemAccess fileSystemAccess)
        {
            _gameService = gameService;
            _orderService = orderService;
            _paymentService = paymentService;
            _shipperService = shipperService;
            _mapper = mapper;
            _fileSystemAccess = fileSystemAccess;
        }

        public ViewResult Index()
        {
            var gamesList = _gameService.GetList();
            var items = (from od in GetBasket().GetItems()
                         where gamesList.FirstOrDefault(g => g.Id == od.GameId) == null
                         select od.GameId).ToList();
            GetBasket().RemoveItems(items);

            return View(GetBasket());
        }

        [HttpGet]
        public ActionResult BuyProduct(string gamekey)
        {
            var game = _gameService.GetByKey(gamekey);
            if (game == null || game.IsDeleted)
            {
                throw new HttpException(404, ErrorMessage);
            }

            GetBasket().AddItem(game);

            return RedirectToAction("Index", "Basket");
        }

        [HttpGet]
        public ViewResult MakeOrder()
        {
            var order = new PaymentViewModel
            {
                OrderDetails = GetBasket().GetItems().ToList(),
                AccountNumber = Guid.NewGuid().ToString(),
                Sum = GetBasket().GetItems().Sum(o => o.Price).ToString("0.00", CultureInfo.InvariantCulture),
                Date = DateTime.UtcNow,
                ShippersList = new SelectList(
                    _shipperService.GetList(), 
                    nameof(ShipperDTO.ShipperId), 
                    nameof(ShipperDTO.CompanyName))
            };

            return View(order);
        }

        [HttpPost]
        public ActionResult BankTerminal(PaymentViewModel model)
        {
            model.Date = DateTime.UtcNow;
            model.OrderDetails = GetBasket().GetItems().ToList();
            var paymentResult = _paymentService.Pay(_mapper.Map<PaymentViewModel, PaymentInfo>(model));
            if (!paymentResult.Successed)
            {
                model.IsFailed = true;
                return View("MakeOrder", model);
            }

            if (!Checkout(model))
            {
                return View("MakeOrder", model);
            } 

            GenerateInvoiceFile(out var filePath, out var fileName, out var fileType, model);

            return File(filePath, fileType, fileName);
        }

        public ActionResult ShowIBoxView(PaymentViewModel model)
        {
            return View("IBoxTerminal", model);
        }

        [HttpPost]
        public ViewResult IBoxTerminal(PaymentViewModel model)
        {
            model.Date = DateTime.UtcNow;
            var paymentResult = _paymentService.Pay(_mapper.Map<PaymentViewModel, PaymentInfo>(model));
            if (!paymentResult.Successed)
            {
                model.OrderDetails = GetBasket().GetItems().ToList();
                model.IsFailed = true;
                return View("MakeOrder", model);
            }

            if (!Checkout(model))
            {
                return View("MakeOrder", model);
            }

            model.Successed = 1;

            return View(model);
        }

        public ActionResult ShowVisaView(PaymentViewModel model)
        {
            ModelState.Clear();

            return View("VisaTerminal", model);
        }

        [HttpPost]
        public ViewResult VisaTerminal(PaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Date = DateTime.UtcNow;
            var paymentResult = _paymentService.Pay(_mapper.Map<PaymentViewModel, PaymentInfo>(model));
            if (!paymentResult.Successed)
            {
                model.OrderDetails = GetBasket().GetItems().ToList();
                model.IsFailed = true;

                return View("MakeOrder", model);
            }

            if (!Checkout(model))
            {
                return View("MakeOrder", model);
            }

            model.Successed = 1;

            return View(model);
        }

        private Basket GetBasket()
        {
            var basket = (Basket)Session["Basket"];
            if (basket != null)
            {
                return basket;
            }

            basket = new Basket();
            Session["Basket"] = basket;

            return basket;
        }

        private bool Checkout(PaymentViewModel model)
        {
            var result = _orderService.Add(new OrderDTO
            {
                CustomerId = model.AccountNumber,
                Date = model.Date,
                ShipperId = model.ShipperId,
                OrderDetails = GetBasket().GetItems().ToList()
            });
            if (!result.Succeeded)
            {
                return false;
            }

            GetBasket().Clear();

            return true;
        }

        private void GenerateInvoiceFile(
            out string filePath,
            out string fileName,
            out string fileType,
            PaymentViewModel model)
        {
            filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            fileName = InvoiceFileName;
            filePath += fileName;
            _fileSystemAccess.CreateInvoiceFile(filePath, model);
            fileType = InvoiceFileType;
        }
    }
}