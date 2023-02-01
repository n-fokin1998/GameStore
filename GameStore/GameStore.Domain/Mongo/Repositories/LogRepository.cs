using System.Linq;
using GameStore.Domain.Mongo.Abstract;
using GameStore.Domain.Mongo.Entities;
using MongoDB.Driver;

namespace GameStore.Domain.Mongo.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly IMongoContext _context;

        public LogRepository(IMongoContext context)
        {
            _context = context;
        }

        public void Add(Log item)
        {
            var oldEntity = item.OldEntityVersion;
            var newEntity = item.NewEntityVersion;
            if (oldEntity != null)
            {
                oldEntity.RemoveElement(oldEntity.Elements.FirstOrDefault(e => e.Name == "_entityWrapper"));
                oldEntity.RemoveElement(oldEntity.Elements.FirstOrDefault(e => e.Name == "_t"));
            }

            if (newEntity != null)
            {
                newEntity.RemoveElement(newEntity.Elements.FirstOrDefault(e => e.Name == "_entityWrapper"));
                newEntity.RemoveElement(newEntity.Elements.FirstOrDefault(e => e.Name == "_t"));
            }

            _context.Database.GetCollection<Log>("logs").InsertOne(item);
        }
    }
}