using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.Domain;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.Web.Infrastructure;
using GameStore.Web.ViewModels;
using GameStore.Web.ViewModels.Enums;

namespace GameStore.Web.Controllers
{
    public class GameController : BaseController
    {
        private const string ErrorMessage = "Not found!";
        private const string EnglishCulture = "en";
        private const string GameFileType = "application/octet-stream";
        private const string GameFileName = "Game.dat";
        private readonly IGameService _gameService;
        private readonly IGenreService _genreService;
        private readonly IPlatformTypeService _platformTypeService;
        private readonly IPublisherService _publisherService;
        private readonly IMapper _mapper;
        private readonly FileSystemAccess _fileSystemAccess;

        public GameController(
            IGameService gameService,
            IPlatformTypeService platformTypeService,
            IGenreService genreService,
            IPublisherService publisherService,
            IMapper mapper,
            FileSystemAccess fileSystemAccess)
        {
            _gameService = gameService;
            _genreService = genreService;
            _platformTypeService = platformTypeService;
            _publisherService = publisherService;
            _mapper = mapper;
            _fileSystemAccess = fileSystemAccess;
        }

        //public ActionResult GetAllGames(
        //    string[] GenreFilters,
        //    int[] PlatformTypeFilters,
        //    int[] PublisherFilters,
        //    decimal? MinPrice,
        //    decimal? MaxPrice,
        //    DateFilterType? DateFilter,
        //    string NameFilter,
        //    SortTypeViewModel? SortType,
        //    int? Page,
        //    ItemsPerPageViewModel? ItemsPerPage,
        //    int? CurrentPage)
        //{
        //    var genreFilters = GenreFilters?.Select(int.Parse).ToList();
        //    var model = new CatalogViewModel
        //    {
        //        GenreFilters = genreFilters?.ToArray(),
        //        PlatformTypeFilters = PlatformTypeFilters,
        //        PublisherFilters = PublisherFilters,
        //        MinPrice = MinPrice,
        //        MaxPrice = MaxPrice,
        //        DateFilter = DateFilter ?? DateFilterType.All,
        //        NameFilter = NameFilter,
        //        SortType = SortType,
        //        Page = Page,
        //        ItemsPerPage = ItemsPerPage,
        //        CurrentPage = CurrentPage
        //    };
        //    if (model.SortType == null)
        //    {
        //        model.SortType = SortTypeViewModel.MostPopular;
        //    }

        //    var pageInfo = new PageInfo();
        //    var filteredGames = _gameService.GetFilteredList(
        //        _mapper.Map<CatalogViewModel, GameFilterDTO>(model), pageInfo);
        //    model.Games = filteredGames.ToList();
        //    model.CurrentPage = pageInfo.PageNumber;
        //    ViewBag.PageInfo = pageInfo;
        //    model.Genres = _genreService.GetList().ToList();
        //    model.PlatformTypes = _platformTypeService.GetList().ToList();
        //    model.Publishers = _publisherService.GetList().ToList();

        //    return View(model);
        //}

        public ActionResult GetAllGames(CatalogViewModel model)
        {
            if (model.SortType == null)
            {
                model.SortType = SortTypeViewModel.MostPopular;
            }

            var pageInfo = new PageInfo();
            var filteredGames = _gameService.GetFilteredList(
                _mapper.Map<CatalogViewModel, GameFilterDTO>(model), pageInfo);
            model.Games = filteredGames.ToList();
            model.CurrentPage = pageInfo.PageNumber;
            ViewBag.PageInfo = pageInfo;
            model.Genres = _genreService.GetList().ToList();
            model.PlatformTypes = _platformTypeService.GetList().ToList();
            model.Publishers = _publisherService.GetList().ToList();

            return View(model);
        }

        public ActionResult GetGenresByGameKey(string key)
        {
            var game = _gameService.GetByKey(key);
            if (game == null)
            {
                throw new HttpException(404, ErrorMessage);
            }

            ViewBag.GameName = Thread.CurrentThread.CurrentCulture.Name.StartsWith(EnglishCulture)
                ? game.NameEn
                : game.NameRu;
            return View(game.Genres);
        }

        public ActionResult GameDetails(string key)
        {
            var game = _gameService.GetByKey(key);
            if (game == null)
            {
                throw new HttpException(404, ErrorMessage);
            }

            game.Popularity++;
            _gameService.Update(game);
            var model = _mapper.Map<GameDTO, GameViewModel>(game);
            model.Publisher = game.PublisherId != null ? _publisherService.GetById((int)game.PublisherId) : null;
            if (Thread.CurrentThread.CurrentCulture.Name.StartsWith(EnglishCulture))
            {
                model.Description = game.DescriptionEn;
                model.GenreNames = game.Genres.Select(g => g.NameEn).ToList();
                model.PlatformNames = game.PlatformTypes.Select(g => g.TypeEn).ToList();
            }
            else
            {
                model.Description = game.DescriptionRu;
                model.GenreNames = game.Genres.Select(g => g.NameRu).ToList();
                model.PlatformNames = game.PlatformTypes.Select(g => g.TypeRu).ToList();
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult DownloadGame(string key)
        {
            var game = _gameService.GetByKey(key);
            if (game == null || game.IsDeleted)
            {
                throw new HttpException(404, ErrorMessage);
            }

            GenerateGameFile(out var filePath, out var fileName, out var fileType, game);

            return File(filePath, fileType, fileName);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 60)]
        public int GetCountOfGames()
        {
            return _gameService.GetQuantity();
        }

        public ActionResult RenderImage(string key)
        {
            var game = _gameService.GetByKey(key);
            if (game == null)
            {
                return RedirectToAction("GetAllGames", "Game");
            }

            var extension = Path.GetExtension(game.ImageReference);
            var mime = "image/" + extension?.Substring(1);
            return File(game.ImageReference, mime);
        }

        public async Task<ActionResult> RenderImageAsync(string key)
        {
            var game = await Task.Run(() => _gameService.GetByKey(key));
            if (game == null)
            {
                return RedirectToAction("GetAllGames", "Game");
            }

            var extension = Path.GetExtension(game.ImageReference);
            var mime = "image/" + extension?.Substring(1);
            return File(game.ImageReference, mime);
        }

        private void GenerateGameFile(out string filePath, out string fileName, out string fileType, GameDTO game)
        {
            filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            fileType = GameFileType;
            fileName = GameFileName;
            filePath += fileName;
            _fileSystemAccess.CreateGameFile(filePath, "Name: " + game.NameEn);
        }
    }
}