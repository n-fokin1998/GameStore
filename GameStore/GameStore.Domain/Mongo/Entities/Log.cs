using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.Domain.Mongo.Entities
{
    public class Log
    {
        [BsonId]
        public ObjectId Id { get; set; }
        
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Date { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string Action { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string EntityType { get; set; }

        public BsonDocument OldEntityVersion { get; set; }

        public BsonDocument NewEntityVersion { get; set; }
    }
}