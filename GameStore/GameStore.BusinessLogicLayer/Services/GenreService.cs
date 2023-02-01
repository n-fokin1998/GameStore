using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.App_LocalResources;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Infrastructure;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;

namespace GameStore.BusinessLogicLayer.Services
{
    public class GenreService : IGenreService
    {
        private const string NameEn = "NameEn";
        private const string NameRu = "NameRu";
        private const string OtherGenre = "Other";
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly List<Genre> _childGenres;

        public GenreService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _childGenres = new List<Genre>();
        }

        public IEnumerable<GenreDTO> GetList()
        {
            var genres = _unitOfWork.Genres.GetList().Where(g => !g.IsDeleted && g.NameEn != OtherGenre).ToList();

            return _mapper.Map<IEnumerable<Genre>, List<GenreDTO>>(genres);
        }

        public IEnumerable<GenreDTO> GetChildList(int id)
        {
            GetChildGenres(_unitOfWork.Genres.GetList().ToList(), id);

            return _mapper.Map<IEnumerable<Genre>, List<GenreDTO>>(_childGenres);
        }

        public GenreDTO GetById(int id)
        {
            var genre = _unitOfWork.Genres.GetItem(id);
            genre = genre != null && !genre.IsDeleted && genre.NameEn != OtherGenre ? genre : null;

            return _mapper.Map<Genre, GenreDTO>(genre);
        }

        public OperationDetails Add(GenreDTO genreDto)
        {
            if (genreDto == null)
            {
                return new OperationDetails(false, BLLRes.GenreNotFound, null);
            }

            var property = string.Empty;
            if (!IsUniqueName(genreDto, ref property))
            {
                return new OperationDetails(false, BLLRes.GenreExists, property);
            }

            var genre = _mapper.Map<GenreDTO, Genre>(genreDto);
            _unitOfWork.Genres.Add(genre);
            _unitOfWork.Commit();

            return new OperationDetails(true);
        }

        public OperationDetails Update(GenreDTO genreDto)
        {
            if (genreDto == null)
            {
                return new OperationDetails(false, BLLRes.GenreNotFound, null);
            }

            var genreRu = _unitOfWork.Genres.GetList().FirstOrDefault(p => p.NameRu == genreDto.NameRu);
            if (genreRu != null && genreRu.Id != genreDto.Id)
            {
                return new OperationDetails(false, BLLRes.GenreExists, NameRu);
            }

            var genreEn = _unitOfWork.Genres.GetList().FirstOrDefault(p => p.NameEn == genreDto.NameEn);
            if (genreEn != null && genreEn.Id != genreDto.Id)
            {
                return new OperationDetails(false, BLLRes.GenreExists, NameEn);
            }

            var genre = _mapper.Map<GenreDTO, Genre>(genreDto);
            _unitOfWork.Genres.Update(genre, genre.Id);
            _unitOfWork.Commit();

            return new OperationDetails(true);
        }

        public OperationDetails Delete(GenreDTO genreDto)
        {
            if (genreDto == null)
            {
                return new OperationDetails(false, BLLRes.GenreNotFound, null);
            }

            var genre = _unitOfWork.Genres.GetItem(genreDto.Id);
            if (genre == null)
            {
                return new OperationDetails(false, BLLRes.GenreNotFound, null);
            }

            DeleteGenreNodes(genre.Id);
            _unitOfWork.Commit();

            return new OperationDetails(true);
        }

        private bool IsUniqueName(GenreDTO genre, ref string property)
        {
            if (_unitOfWork.Genres.GetList().FirstOrDefault(p => p.NameRu == genre.NameRu) != null)
            {
                property = NameRu;
                return false;
            }

            if (_unitOfWork.Genres.GetList().FirstOrDefault(p => p.NameEn == genre.NameEn) == null)
            {
                return true;
            }

            property = NameEn;

            return false;
        }

        private void DeleteGenreNodes(int id)
        {
            var inner = _unitOfWork.Genres.GetList().Where(x => x.ParentGenreId == id).ToList();
            foreach (var node in inner)
            {
                node.ParentGenre = null;
                _unitOfWork.Genres.Update(node, node.Id);
            }

            var deleted = _unitOfWork.Genres.GetList().Single(x => x.Id == id);
            deleted.IsDeleted = true;
            _unitOfWork.Genres.Update(deleted, deleted.Id);
        }

        private void GetChildGenres(List<Genre> genres, int id)
        {
            var children = genres.Where(g => g.ParentGenreId == id).ToList();
            _childGenres.AddRange(children);
            foreach (var c in children)
            {
                GetChildGenres(genres, c.Id);
            }
        }
    }
}