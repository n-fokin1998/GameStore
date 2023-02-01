using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.Web.Areas.Manager.ViewModels;
using GameStore.Web.Controllers;

namespace GameStore.Web.Areas.Manager.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerGenreController : BaseController
    {
        private const string ErrorMessage = "Not found!";
        private const string EnglishCulture = "en";
        private readonly IGenreService _genreService;
        private readonly IMapper _mapper;

        public ManagerGenreController(IGenreService genreService, IMapper mapper)
        {
            _genreService = genreService;
            _mapper = mapper;
        }

        [HttpGet]
        public ViewResult CreateGenre()
        {
            ViewBag.Genres = Thread.CurrentThread.CurrentCulture.Name.StartsWith(EnglishCulture)
                ? new SelectList(_genreService.GetList(), nameof(GenreDTO.Id), nameof(GenreDTO.NameEn))
                : new SelectList(_genreService.GetList(), nameof(GenreDTO.Id), nameof(GenreDTO.NameRu));

            return View();
        }

        [HttpPost]
        public ActionResult CreateGenre(GenreViewModel model)
        {
            if (ModelState.IsValid)
            {
                var genre = _mapper.Map<GenreViewModel, GenreDTO>(model);
                var result = _genreService.Add(genre);
                if (result.Succeeded)
                {
                    return RedirectToAction("GenreDetails", "Genre", new { id = model.Id, area = string.Empty });
                }

                ModelState.AddModelError(result.Property ?? nameof(genre.NameEn), result.Message);
            }

            var genres = _genreService.GetList().ToList();
            genres.RemoveAll(g => g.Id == model.Id);
            ViewBag.Genres = Thread.CurrentThread.CurrentCulture.Name.StartsWith(EnglishCulture)
                ? new SelectList(genres, nameof(GenreDTO.Id), nameof(GenreDTO.NameEn))
                : new SelectList(genres, nameof(GenreDTO.Id), nameof(GenreDTO.NameRu));

            return View(model);
        }

        [HttpGet]
        public ActionResult EditGenre(int id)
        {
            var genre = _genreService.GetById(id);
            if (genre == null)
            {
                throw new HttpException(404, ErrorMessage);
            }

            var model = _mapper.Map<GenreDTO, GenreViewModel>(genre);
            var genres = _genreService.GetList().ToList();
            genres.RemoveAll(g => g.Id == id || _genreService.GetChildList(id).Any(ge => ge.Id == g.Id));
            ViewBag.Genres = Thread.CurrentThread.CurrentCulture.Name.StartsWith(EnglishCulture)
                ? new SelectList(genres, nameof(GenreDTO.Id), nameof(GenreDTO.NameEn))
                : new SelectList(genres, nameof(GenreDTO.Id), nameof(GenreDTO.NameRu));

            return View(model);
        }

        [HttpPost]
        public ActionResult EditGenre(GenreViewModel model)
        {
            if (ModelState.IsValid)
            {
                var genre = _mapper.Map<GenreViewModel, GenreDTO>(model);
                var result = _genreService.Update(genre);
                if (result.Succeeded)
                {
                    return RedirectToAction("GenreDetails", "Genre", new { id = model.Id, area = string.Empty });
                }

                ModelState.AddModelError(result.Property ?? nameof(genre.NameEn), result.Message);
            }

            var genres = _genreService.GetList().ToList();
            genres.RemoveAll(g => g.Id == model.Id || _genreService.GetChildList(model.Id).Any(ge => ge.Id == g.Id));
            ViewBag.Genres = Thread.CurrentThread.CurrentCulture.Name.StartsWith(EnglishCulture)
                ? new SelectList(genres, nameof(GenreDTO.Id), nameof(GenreDTO.NameEn))
                : new SelectList(genres, nameof(GenreDTO.Id), nameof(GenreDTO.NameRu));

            return View(model);
        }

        [HttpGet]
        public ActionResult DeleteGenre(int id)
        {
            var genre = _genreService.GetById(id);
            if (genre == null)
            {
                throw new HttpException(404, ErrorMessage);
            }

            var model = new DeleteGenreViewModel { Id = id };

            return PartialView("DeleteGenreModalWindow", model);
        }

        [HttpPost]
        public ActionResult DeleteGenre(DeleteGenreViewModel model)
        {
            var genre = _genreService.GetById(model.Id);
            _genreService.Delete(genre);

            return RedirectToAction("Index", "Genre", new { area = string.Empty });
        }
    }
}