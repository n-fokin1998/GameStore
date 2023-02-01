using System;
using GameStore.Domain.Mongo.Util;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.Domain.Mongo.Entities
{
    public class Order
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("OrderID")]
        public int OrderId { get; set; }

        [BsonElement("CustomerID")]
        public string CustomerId { get; set; }

        [BsonElement("EmployeeID")]
        public int EmployeeId { get; set; }

        [BsonElement("OrderDate")]
        [BsonSerializer(typeof(BsonDateDeserializer))]
        public DateTime? OrderDate { get; set; }

        [BsonElement("RequiredDate")]
        [BsonSerializer(typeof(BsonDateDeserializer))]
        public DateTime? RequiredDate { get; set; }

        [BsonElement("ShippedDate")]
        [BsonSerializer(typeof(BsonDateDeserializer))]
        public DateTime? ShippedDate { get; set; }

        [BsonElement("ShipVia")]
        [BsonSerializer(typeof(BsonStringDeserializer))]
        public string ShipVia { get; set; }

        [BsonElement("Freight")]
        public double Freight { get; set; }

        [BsonElement("ShipName")]
        public string ShipName { get; set; }

        [BsonElement("ShipAddress")]
        [BsonSerializer(typeof(BsonStringDeserializer))]
        public string ShipAddress { get; set; }

        [BsonElement("ShipCity")]
        [BsonSerializer(typeof(BsonStringDeserializer))]
        public string ShipCity { get; set; }

        [BsonElement("ShipRegion")]
        public string ShipRegion { get; set; }

        [BsonElement("ShipPostalCode")]
        [BsonSerializer(typeof(BsonStringDeserializer))]
        public string ShipPostalCode { get; set; }

        [BsonElement("ShipCountry")]
        public string ShipCountry { get; set; }
    }
}