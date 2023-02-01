using GameStore.Domain.Mongo.Entities;

namespace GameStore.Domain.Mongo.Abstract
{
    public interface ILogRepository
    {
        void Add(Log item);
    }
}