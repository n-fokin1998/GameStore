using System;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.Domain.Mongo.Abstract;
using GameStore.Domain.Mongo.Entities;
using MongoDB.Bson;

namespace GameStore.Domain.Repositories
{
    public class GameRepository : AbstractLoggingRepository<Game>
    {
        public GameRepository(IDbContext context, ILogRepository logRepository) : base(context, logRepository)
        {
        }

        public override void Update(Game item, int id)
        {
            var old = Context.Set<Game>().Find(id);
            var oldForLog = old.ToBsonDocument();
            old?.Genres.Clear();
            old?.PlatformTypes.Clear();
            Context.Entry(old).CurrentValues.SetValues(item);

            foreach (var genre in item.Genres)
            {
                old?.Genres.Add(genre);
            }

            foreach (var platformType in item.PlatformTypes)
            {
                old?.PlatformTypes.Add(platformType);
            }

            LogRepository.Add(new Log
            {
                Date = DateTime.UtcNow,
                Action = "Update",
                EntityType = typeof(Game).ToString(),
                OldEntityVersion = oldForLog,
                NewEntityVersion = item.ToBsonDocument()
            });
        }
    }
}