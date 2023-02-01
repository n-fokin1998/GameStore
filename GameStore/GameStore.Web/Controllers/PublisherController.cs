using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.BusinessLogicLayer.Abstract.Services;

namespace GameStore.Web.Controllers
{
    public class PublisherController : BaseController
    {
        private const string ErrorMessage = "Not found!";
        private readonly IGameService _gameService;
        private readonly IPublisherService _publisherService;

        public PublisherController(IGameService gameService, IPublisherService publisherService)
        {
            _gameService = gameService;
            _publisherService = publisherService;
        }

        public ViewResult Index()
        {
            var publishers = _publisherService.GetList();

            return View(publishers);
        }

        public ActionResult PublisherDetails(string companyName)
        {
            var publisher = _publisherService
                .GetList().FirstOrDefault(p => p.CompanyName == companyName);
            if (publisher == null)
            {
                throw new HttpException(404, ErrorMessage);
            }

            var gamesList = _gameService.GetList();
            publisher.Games = gamesList.Where(g => g.PublisherId == publisher.Id).ToList();

            return View(publisher);
        }

        public ActionResult GetGamesByPublisherName(string companyName)
        {
            var publisher = _publisherService
                .GetList().FirstOrDefault(p => p.CompanyName == companyName);
            if (publisher == null)
            {
                throw new HttpException(404, ErrorMessage);
            }

            var gamesList = _gameService.GetList().Where(g => g.PublisherId == publisher.Id).ToList();
            ViewBag.PublisherName = publisher.CompanyName;
            return View(gamesList);
        }
    }
}