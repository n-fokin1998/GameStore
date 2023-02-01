using System.Collections.Generic;
using GameStore.BusinessLogicLayer.DTO;

namespace GameStore.BusinessLogicLayer.Abstract.Services
{
    public interface IShipperService
    {
        IEnumerable<ShipperDTO> GetList();
    }
}