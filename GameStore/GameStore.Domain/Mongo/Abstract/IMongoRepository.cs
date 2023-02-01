using System.Linq;

namespace GameStore.Domain.Mongo.Abstract
{
    public interface IMongoRepository<T> where T : class
    {
        IQueryable<T> GetList();
    }
}