using System.Web.Mvc;
using GameStore.BusinessLogicLayer.Abstract.Services;

namespace GameStore.Web.Controllers
{
    public class ShipperController : BaseController
    {
        private readonly IShipperService _shipperService;

        public ShipperController(IShipperService shipperService)
        {
            _shipperService = shipperService;
        }

        public ActionResult Index()
        {
            var shippers = _shipperService.GetList();

            return View(shippers);
        }
    }
}