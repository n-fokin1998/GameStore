using MongoDB.Driver;

namespace GameStore.Domain.Mongo.Abstract
{
    public interface IMongoContext
    {
        IMongoDatabase Database { get; set; }
    }
}