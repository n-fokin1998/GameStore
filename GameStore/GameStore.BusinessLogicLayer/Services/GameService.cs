using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.App_LocalResources;
using GameStore.BusinessLogicLayer.Domain;
using GameStore.BusinessLogicLayer.Domain.Enums;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Infrastructure;
using GameStore.BusinessLogicLayer.Services.Filter;
using GameStore.BusinessLogicLayer.Services.Filter.Filters;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;

namespace GameStore.BusinessLogicLayer.Services
{
    public class GameService : IGameService
    {
        private const string OtherGenre = "Other";
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GameSelectionPipeline _gameSelectionPipeline;

        public GameService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            GameSelectionPipeline gameSelectionPipeline)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _gameSelectionPipeline = gameSelectionPipeline;
        }

        public OperationDetails Add(GameDTO gameDto)
        {
            if (gameDto == null)
            {
                return new OperationDetails(false, BLLRes.GameNotFound, null);
            }

            if (_unitOfWork.Games.GetList().FirstOrDefault(g => g.Key == gameDto.Key) != null)
            {
                return new OperationDetails(false, BLLRes.GameExists, null);
            }

            var newGame = _mapper.Map<GameDTO, Game>(gameDto);
            newGame.Genres = new List<Genre>();
            newGame.PlatformTypes = new List<PlatformType>();
            AttachGenresCollection(gameDto.Genres, newGame);
            AttachPlatformTypesCollection(gameDto.PlatformTypes, newGame);
            var publisher = gameDto.PublisherId.HasValue 
                ? _unitOfWork.Publishers.GetItem(gameDto.PublisherId.Value)
                : null;
            newGame.Publisher = publisher;
            newGame.UploadDate = DateTime.UtcNow;
            _unitOfWork.Games.Add(newGame);
            _unitOfWork.Commit();

            return new OperationDetails(true);
        }

        public OperationDetails Delete(GameDTO gameDto)
        {
            if (gameDto == null)
            {
                return new OperationDetails(false, BLLRes.GameNotFound, null);
            }

            var game = _unitOfWork.Games.GetItem(gameDto.Id);
            if (game == null)
            {
                return new OperationDetails(false, BLLRes.GameNotFound, null);
            }

            game.IsDeleted = true;
            _unitOfWork.Games.Update(game, game.Id);
            _unitOfWork.Commit();

            return new OperationDetails(true);
        }

        public IEnumerable<GameDTO> GetList()
        {
            var games = _unitOfWork.Games.GetList().Where(g => !g.IsDeleted && g.UnitsInStock > 0).ToList();

            return _mapper.Map<IEnumerable<Game>, List<GameDTO>>(games);
        }

        public GameDTO GetByKey(string key)
        {
            var game = _unitOfWork.Games.GetList().FirstOrDefault(g => g.Key == key);
            if (game == null)
            {
                return null;
            }

            RemoveDeletedGenres(game);
            RemoveDeletedPlatformTypes(game);

            return _mapper.Map<Game, GameDTO>(game);
        }

        public OperationDetails Update(GameDTO gameDto)
        {
            if (gameDto == null)
            {
                return new OperationDetails(false, BLLRes.GameNotFound, null);
            }

            var game = _unitOfWork.Games.GetItem(gameDto.Id);
            if (game == null)
            {
                return new OperationDetails(false, BLLRes.GameNotFound, null);
            }

            game = _mapper.Map<GameDTO, Game>(gameDto);
            game.Publisher = gameDto.PublisherId.HasValue 
                ? _unitOfWork.Publishers.GetItem(gameDto.PublisherId.Value) 
                : null;
            AttachGenresCollection(gameDto.Genres, game);
            AttachPlatformTypesCollection(gameDto.PlatformTypes, game);
            _unitOfWork.Games.Update(game, game.Id);
            _unitOfWork.Commit();

            return new OperationDetails(true);
        }

        public IEnumerable<GameDTO> GetByGenre(int genreId)
        {
            var genre = _unitOfWork.Genres.GetItem(genreId);
            if (genre == null)
            {
                throw new ServiceException(BLLRes.GenreNotFound, null);
            }

            var games = genre.Games.Where(g => !g.IsDeleted).ToList();

            return _mapper.Map<IEnumerable<Game>, List<GameDTO>>(games);
        }

        public IEnumerable<GameDTO> GetByPlatformType(int platformTypeId)
        {
            var platformType = _unitOfWork.PlatformTypes.GetItem(platformTypeId);
            if (platformType == null)
            {
                throw new ServiceException(BLLRes.PlatformTypeNotFound, null);
            }

            var games = platformType.Games.Where(g => !g.IsDeleted).ToList();

            return _mapper.Map<IEnumerable<Game>, List<GameDTO>>(games);
        }

        public int GetQuantity()
        {
            return _unitOfWork.Games.GetList().Count();
        }

        public IEnumerable<GameDTO> GetFilteredList(GameFilterDTO gameFilterDto, PageInfo pageInfo)
        {
            _gameSelectionPipeline.Register(new GenreFilter(gameFilterDto.GenreFilters))
                .Register(new PlatformTypeFilter(gameFilterDto.PlatformTypeFilters))
                .Register(new PublisherFilter(gameFilterDto.PublisherFilters))
                .Register(new DateFilter(gameFilterDto.DateFilter))
                .Register(new NameFilter(gameFilterDto.NameFilter));
            if (gameFilterDto.MinPrice != null || gameFilterDto.MaxPrice != null)
            {
                _gameSelectionPipeline.Register(
                    new PriceFilter(gameFilterDto.MinPrice ?? 0, gameFilterDto.MaxPrice ?? decimal.MaxValue));
            }

            _gameSelectionPipeline.Register(new GameSorting((SortType)gameFilterDto.SortType));
            var filteredGames = _gameSelectionPipeline.Process();
            var pageSize = gameFilterDto.ItemsPerPage != null ? (int)gameFilterDto.ItemsPerPage : 10;
            pageSize = pageSize == -1 ? filteredGames.Count() + 1 : pageSize;
            var page = gameFilterDto.Page ?? 1;
            var result = filteredGames.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            pageInfo.PageNumber = page;
            pageInfo.PageSize = pageSize;
            pageInfo.TotalItems = filteredGames.Count();

            return _mapper.Map<IEnumerable<Game>, List<GameDTO>>(result);
        }

        private void RemoveDeletedGenres(Game game)
        {
            var genres = game.Genres.Where(genre => genre.IsDeleted).ToList();
            foreach (var genre in genres)
            {
                game.Genres.Remove(genre);
            }
        }

        private void RemoveDeletedPlatformTypes(Game game)
        {
            var platformTypes = game.PlatformTypes.Where(platformType => platformType.IsDeleted).ToList();
            foreach (var platformType in platformTypes)
            {
                game.PlatformTypes.Remove(platformType);
            }
        }

        private void AttachGenresCollection(IEnumerable<GenreDTO> genres, Game game)
        {
            game.Genres = new List<Genre>();
            if (genres == null)
            {
                return;
            }

            var genresIds = genres.Select(g => g.Id).ToList();
            var existingGenres = _unitOfWork.Genres.GetList().Where(g => genresIds.Any(i => i == g.Id)).ToList();
            foreach (var genre in existingGenres)
            {
                game.Genres.Add(genre);
            }

            if (game.Genres.Count == 0)
            {
                game.Genres.Add(_unitOfWork.Genres.GetList().FirstOrDefault(g => g.NameEn == OtherGenre));
            }
        }

        private void AttachPlatformTypesCollection(IEnumerable<PlatformTypeDTO> platformTypes, Game game)
        {
            game.PlatformTypes = new List<PlatformType>();
            if (platformTypes == null)
            {
                return;
            }

            var platformTypeIds = platformTypes.Select(p => p.Id).ToList();
            var existingPlatformTypes = _unitOfWork.PlatformTypes.GetList()
                .Where(p => platformTypeIds.Any(i => i == p.Id)).ToList();
            foreach (var platformType in existingPlatformTypes)
            {
                game.PlatformTypes.Add(platformType);
            }
        }
    }
}