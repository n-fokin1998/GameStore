using System.Linq;
using GameStore.Domain.Mongo.Entities;

namespace GameStore.Domain.Mongo.Abstract
{
    public interface IShipperRepository
    {
        IQueryable<Shipper> GetList();
    }
}