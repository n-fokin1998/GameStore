using GameStore.Domain.Mongo.Entities;

namespace GameStore.Domain.Mongo.Abstract
{
    public interface IMongoUnitOfWork
    {
        ILogRepository Logs { get; }

        IMongoRepository<Order> Orders { get; }

        IMongoRepository<Shipper> Shippers { get; }
    }
}