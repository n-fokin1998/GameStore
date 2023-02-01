using System.Collections.Generic;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Infrastructure;

namespace GameStore.BusinessLogicLayer.Abstract.Services
{
    public interface IUserService
    {
        IEnumerable<UserDTO> GetList();

        UserDTO GetById(int id);

        OperationDetails Update(UserDTO roleDto);

        OperationDetails Delete(UserDTO roleDto);

        OperationDetails Register(UserDTO userDto);
    }
}