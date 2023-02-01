using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.BusinessLogicLayer.Abstract.Services;

namespace GameStore.Web.Controllers
{
    public class PlatformTypeController : BaseController
    {
        private const string ErrorMessage = "Not found!";
        private readonly IGameService _gameService;
        private readonly IPlatformTypeService _platformTypeService;

        public PlatformTypeController(IGameService gameService, IPlatformTypeService platformTypeService)
        {
            _gameService = gameService;
            _platformTypeService = platformTypeService;
        }

        public ViewResult Index()
        {
            var platformTypes = _platformTypeService.GetList();

            return View(platformTypes);
        }

        public ActionResult PlatformTypeDetails(int id)
        {
            var platformType = _platformTypeService.GetById(id);
            if (platformType == null)
            {
                throw new HttpException(404, ErrorMessage);
            }

            platformType.Games = _gameService.GetByPlatformType(platformType.Id).ToList();

            return View(platformType);
        }
    }
}