using System;
using GameStore.Domain.Abstract;
using GameStore.Domain.Mongo.Abstract;
using GameStore.Domain.Mongo.Entities;
using MongoDB.Bson;

namespace GameStore.Domain.Repositories
{
    public class AbstractLoggingRepository<T> : AbstractRepository<T> where T : class
    {
        protected readonly ILogRepository LogRepository;

        public AbstractLoggingRepository(IDbContext context, ILogRepository logRepository) : base(context)
        {
            LogRepository = logRepository;
        }

        public override void Add(T item)
        {
            base.Add(item);
            LogRepository.Add(new Log
            {
                Date = DateTime.UtcNow,
                Action = "Add",
                EntityType = typeof(T).ToString(),
                NewEntityVersion = item.ToBsonDocument()
            });
        }

        public override void Update(T item, int id)
        {
            var old = Context.Set<T>().Find(id).ToBsonDocument();
            base.Update(item, id);
            LogRepository.Add(new Log
            {
                Date = DateTime.UtcNow,
                Action = "Update",
                EntityType = typeof(T).ToString(),
                OldEntityVersion = old,
                NewEntityVersion = item.ToBsonDocument()
            });
        }

        public override void Delete(T item)
        {
            base.Delete(item);
            LogRepository.Add(new Log
            {
                Date = DateTime.UtcNow,
                Action = "Delete",
                EntityType = typeof(T).ToString(),
                OldEntityVersion = item.ToBsonDocument()
            });
        }
    }
}