using System.Collections.Generic;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Infrastructure;

namespace GameStore.BusinessLogicLayer.Abstract.Services
{
    public interface IPlatformTypeService
    {
        IEnumerable<PlatformTypeDTO> GetList();

        PlatformTypeDTO GetById(int id);

        OperationDetails Add(PlatformTypeDTO platformTypeDto);

        OperationDetails Update(PlatformTypeDTO platformTypeDto);

        OperationDetails Delete(PlatformTypeDTO platformTypeDto);
    }
}