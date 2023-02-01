using System.Linq;
using GameStore.Domain.Mongo.Abstract;
using MongoDB.Driver;

namespace GameStore.Domain.Mongo.Repositories
{
    public class MongoAbstractRepository<T> : IMongoRepository<T> where T : class 
    {
        private readonly IMongoContext _context;
        private readonly string _collectionName;

        public MongoAbstractRepository(IMongoContext context, string collectionName)
        {
            _context = context;
            _collectionName = collectionName;
        }

        public IQueryable<T> GetList()
        {
            return _context.Database.GetCollection<T>(_collectionName).AsQueryable();
        }
    }
}