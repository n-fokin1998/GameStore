using System.Collections.Generic;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Infrastructure;

namespace GameStore.BusinessLogicLayer.Abstract.Services
{
    public interface IRoleService
    {
        IEnumerable<RoleDTO> GetList();

        RoleDTO GetById(int id);

        OperationDetails Add(RoleDTO roleDto);

        OperationDetails Update(RoleDTO roleDto);

        OperationDetails Delete(RoleDTO roleDto);
    }
}