using System.Collections.Generic;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Infrastructure;

namespace GameStore.BusinessLogicLayer.Abstract.Services
{
    public interface IGenreService
    {
        IEnumerable<GenreDTO> GetList();

        IEnumerable<GenreDTO> GetChildList(int id);

        GenreDTO GetById(int id);

        OperationDetails Update(GenreDTO genreDto);

        OperationDetails Add(GenreDTO genreDto);

        OperationDetails Delete(GenreDTO genreDto);
    }
}