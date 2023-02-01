using System.Linq;
using System.Web.Http;
using GameStore.BusinessLogicLayer.Abstract.Services;

namespace GameStore.Web.Controllers.ApiControllers
{
    public class GameController : ApiController
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        public IHttpActionResult GetAllGames()
        {
            return Json(_gameService.GetList().ToList());
        }
    }
}