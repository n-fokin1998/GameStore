using System.Configuration;
using GameStore.Domain.Mongo.Abstract;
using GameStore.Domain.Mongo.Util;
using MongoDB.Driver;

namespace GameStore.Domain.Mongo
{
    public class GameStoreMongoContext : IMongoContext
    {
        public GameStoreMongoContext(string connectionString)
        {
            connectionString = ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
            var connection = new MongoUrlBuilder(connectionString);
            var client = new MongoClient(connectionString);
            Database = client.GetDatabase(connection.DatabaseName);
            EntityMongoMapper.Initialize();
        }

        public IMongoDatabase Database { get; set; }
    }
}