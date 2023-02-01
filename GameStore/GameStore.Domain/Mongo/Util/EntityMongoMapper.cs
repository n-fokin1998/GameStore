using GameStore.Domain.Entities;
using MongoDB.Bson.Serialization;

namespace GameStore.Domain.Mongo.Util
{
    public class EntityMongoMapper
    {
        public static void Initialize()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Game)))
            {
                BsonClassMap.RegisterClassMap<Game>(cm =>
                {
                    cm.AutoMap();
                    cm.UnmapProperty(g => g.Publisher);
                    cm.UnmapProperty(g => g.Comments);
                    cm.MapMember(p => p.Genres).SetSerializer(new BsonEntitySerializer<Genre>());
                    cm.MapMember(p => p.PlatformTypes).SetSerializer(new BsonEntitySerializer<PlatformType>());
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Comment)))
            {
                BsonClassMap.RegisterClassMap<Comment>(cm =>
                {
                    cm.AutoMap();
                    cm.UnmapProperty(c => c.Game);
                    cm.UnmapProperty(c => c.ParentComment);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(OrderDetails)))
            {
                BsonClassMap.RegisterClassMap<OrderDetails>(cm =>
                {
                    cm.AutoMap();
                    cm.UnmapProperty(c => c.Order);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Genre)))
            {
                BsonClassMap.RegisterClassMap<Genre>(cm =>
                {
                    cm.AutoMap();
                    cm.UnmapProperty(c => c.ParentGenre);
                    cm.MapMember(p => p.Games).SetSerializer(new BsonEntitySerializer<Game>());
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(PlatformType)))
            {
                BsonClassMap.RegisterClassMap<PlatformType>(cm =>
                {
                    cm.AutoMap();
                    cm.MapMember(p => p.Games).SetSerializer(new BsonEntitySerializer<Game>());
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Publisher)))
            {
                BsonClassMap.RegisterClassMap<Publisher>(cm =>
                {
                    cm.AutoMap();
                    cm.MapMember(p => p.Games).SetSerializer(new BsonEntitySerializer<Game>());
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(User)))
            {
                BsonClassMap.RegisterClassMap<User>(cm =>
                {
                    cm.AutoMap();
                    cm.MapMember(p => p.Roles).SetSerializer(new BsonEntitySerializer<Role>());
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Role)))
            {
                BsonClassMap.RegisterClassMap<Role>(cm =>
                {
                    cm.AutoMap();
                    cm.MapMember(p => p.Users).SetSerializer(new BsonEntitySerializer<User>());
                });
            }
        }
    }
}