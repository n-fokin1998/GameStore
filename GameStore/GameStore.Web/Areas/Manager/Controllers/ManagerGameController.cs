using System.Collections.Generic;
using System.IO;
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
    [Authorize(Roles = "Manager, Publisher")]
    public class ManagerGameController : BaseController
    {
        private const string ErrorMessage = "Not found!";
        private const string ImagePath = "~/Content/Images/Games/";
        private const string EnglishCulture = "en";
        private readonly IGameService _gameService;
        private readonly IGenreService _genreService;
        private readonly IPlatformTypeService _platformTypeService;
        private readonly IPublisherService _publisherService;
        private readonly IMapper _mapper;

        public ManagerGameController(
            IGameService gameService,
            IPlatformTypeService platformTypeService,
            IGenreService genreService,
            IPublisherService publisherService,
            IMapper mapper)
        {
            _gameService = gameService;
            _genreService = genreService;
            _platformTypeService = platformTypeService;
            _publisherService = publisherService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ViewResult CreateGame()
        {
            var model = new AddGameViewModel();
            AttachSelectLists(model);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public ActionResult CreateGame(AddGameViewModel model, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                var game = _mapper.Map<AddGameViewModel, GameDTO>(model);
                AttachGenres(model.Genres, game);
                AttachPlatformTypes(model.PlatformTypes, game);
                if (uploadImage != null)
                {
                    var fileName = Path.GetFileName(uploadImage.FileName);
                    uploadImage.SaveAs(Server.MapPath(ImagePath + fileName));
                    game.ImageReference = ImagePath + fileName;
                }

                var result = _gameService.Add(game);
                if (result.Succeeded)
                {
                    return RedirectToAction("GetAllGames", "Game", new { key = model.Key, area = string.Empty });
                }

                ModelState.AddModelError(nameof(game.Key), result.Message);
            }

            AttachSelectLists(model);

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Publisher")]
        public ActionResult EditGame(string key)
        {
            var game = _gameService.GetByKey(key);
            if (game == null || game.IsDeleted)
            {
                throw new HttpException(404, ErrorMessage);
            }

            var publisher = game.PublisherId != null ? _publisherService.GetById((int)game.PublisherId) : null;
            if (CurrentUser != null && CurrentUser.InRoles("Publisher") && CurrentUser.Name != publisher?.CompanyName)
            {
                return RedirectToAction("GameDetails", "Game", new { key = game.Key, area = string.Empty });
            }

            var model = _mapper.Map<GameDTO, AddGameViewModel>(game);
            AttachSelectLists(model);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Publisher")]
        public ActionResult EditGame(AddGameViewModel model, HttpPostedFileBase uploadImage)
        {
            if (!ModelState.IsValid)
            {
                AttachSelectLists(model);

                return View(model);
            }

            var game = _mapper.Map<AddGameViewModel, GameDTO>(model);
            AttachGenres(model.Genres, game);
            AttachPlatformTypes(model.PlatformTypes, game);
            if (uploadImage != null)
            {
                var fileName = Path.GetFileName(uploadImage.FileName);
                uploadImage.SaveAs(Server.MapPath(ImagePath + fileName));
                game.ImageReference = ImagePath + fileName;
            }

            var result = _gameService.Update(game);
            if (result.Succeeded)
            {
                return RedirectToAction("GameDetails", "Game", new { key = model.Key, area = string.Empty });
            }

            ModelState.AddModelError(nameof(game.Key), result.Message);

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ActionResult DeleteGame(string key)
        {
            var game = _gameService.GetByKey(key);
            if (game == null || game.IsDeleted)
            {
                throw new HttpException(404, ErrorMessage);
            }

            var model = new DeleteGameViewModel { GameKey = key };

            return PartialView("DeleteModalWindow", model);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public ActionResult DeleteGame(DeleteGameViewModel model)
        {
            _gameService.Delete(_gameService.GetByKey(model.GameKey));

            return RedirectToAction("GetAllGames", "Game", new { area = string.Empty });
        }

        private void AttachGenres(int[] genres, GameDTO game)
        {
            game.Genres = new List<GenreDTO>();
            if (genres == null)
            {
                return;
            }

            foreach (var g in genres)
            {
                var genre = _genreService.GetById(g);
                if (genre != null)
                {
                    game.Genres.Add(genre);
                }
            }
        }

        private void AttachPlatformTypes(int[] platformTypes, GameDTO game)
        {
            game.PlatformTypes = new List<PlatformTypeDTO>();
            if (platformTypes == null)
            {
                return;
            }

            foreach (var p in platformTypes)
            {
                var platformType = _platformTypeService.GetById(p);
                if (platformType != null)
                {
                    game.PlatformTypes.Add(platformType);
                }
            }
        }

        private void AttachSelectLists(AddGameViewModel model)
        {
            var selectedGenres = new List<int>();
            var selectedPlatformTypes = new List<int>();
            var game = model.Key != null ? _gameService.GetByKey(model.Key) : null;
            if (game != null)
            {
                selectedGenres = game.Genres.Select(g => g.Id).ToList();
                selectedPlatformTypes = game.PlatformTypes.Select(p => p.Id).ToList();
            }

            if (Thread.CurrentThread.CurrentCulture.Name.StartsWith(EnglishCulture))
            {
                model.GenresList = new MultiSelectList(
                    _genreService.GetList(),
                    nameof(GenreDTO.Id),
                    nameof(GenreDTO.NameEn),
                    selectedGenres);
                model.PlatformTypesList = new MultiSelectList(
                    _platformTypeService.GetList(),
                    nameof(PlatformTypeDTO.Id),
                    nameof(PlatformTypeDTO.TypeEn),
                    selectedPlatformTypes);
            }
            else
            {
                model.GenresList = new MultiSelectList(
                    _genreService.GetList(),
                    nameof(GenreDTO.Id),
                    nameof(GenreDTO.NameRu),
                    selectedGenres);
                model.PlatformTypesList = new MultiSelectList(
                    _platformTypeService.GetList(),
                    nameof(PlatformTypeDTO.Id),
                    nameof(PlatformTypeDTO.TypeRu),
                    selectedPlatformTypes);
            }

            model.PublishersList = new SelectList(
                _publisherService.GetList(),
                nameof(PublisherDTO.Id),
                nameof(PublisherDTO.CompanyName));
        }
    }
}