using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.Domain.Mongo.Entities
{
    public class Shipper
    {
        [BsonId]
        public ObjectId Id { get; set; }
        
        [BsonElement("ShipperID")]
        public int ShipperId { get; set; }

        [BsonElement("CompanyName")]
        public string CompanyName { get; set; }

        [BsonElement("Phone")]
        public string Phone { get; set; }
    }
}