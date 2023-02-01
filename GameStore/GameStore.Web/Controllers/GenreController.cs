using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.Web.ViewModels;

namespace GameStore.Web.Controllers
{
    public class GenreController : BaseController
    {
        private const string EnglishCulture = "en";
        private const string ErrorMessage = "Not found!";
        private readonly IGameService _gameService;
        private readonly IGenreService _genreService;

        public GenreController(IGameService gameService, IGenreService genreService)
        {
            _gameService = gameService;
            _genreService = genreService;
        }

        public ViewResult Index()
        {
            var genres = _genreService.GetList();
            var model = new GenreListViewModel { Genres = genres };

            return View(model);
        }

        public ActionResult GenreDetails(int id)
        {
            var genre = _genreService.GetById(id);
            if (genre == null)
            {
                throw new HttpException(404, ErrorMessage);
            }

            if (genre.ParentGenreId != null)
            {
                genre.ParentGenre = _genreService.GetById((int)genre.ParentGenreId);
            }

            genre.Games = _gameService.GetByGenre(genre.Id).ToList();

            return View(genre);
        }

        public ActionResult GetGamesByGenreId(int id)
        {
            var genre = _genreService.GetById(id);
            if (genre == null)
            {
                throw new HttpException(404, ErrorMessage);
            }

            ViewBag.GenreName = Thread.CurrentThread.CurrentCulture.Name.StartsWith(EnglishCulture)
                ? genre.NameEn
                : genre.NameRu;
            return View(_gameService.GetByGenre(id).ToList());
        }
    }
}