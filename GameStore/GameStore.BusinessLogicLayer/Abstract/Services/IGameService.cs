using System.Collections.Generic;
using GameStore.BusinessLogicLayer.Domain;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Infrastructure;

namespace GameStore.BusinessLogicLayer.Abstract.Services
{
    public interface IGameService
    {
        IEnumerable<GameDTO> GetList();

        IEnumerable<GameDTO> GetByGenre(int genreId);

        IEnumerable<GameDTO> GetByPlatformType(int platformTypeId);

        IEnumerable<GameDTO> GetFilteredList(GameFilterDTO gameFilterDto, PageInfo pageInfo);

        GameDTO GetByKey(string key);

        OperationDetails Add(GameDTO gameDto);

        OperationDetails Update(GameDTO gameDto);

        OperationDetails Delete(GameDTO gameDto);

        int GetQuantity();
    }
}