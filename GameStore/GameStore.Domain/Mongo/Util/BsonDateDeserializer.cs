using System;
using System.Globalization;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace GameStore.Domain.Mongo.Util
{
    public class BsonDateDeserializer : SerializerBase<DateTime?>
    {
        public override DateTime? Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            if (context.Reader.GetCurrentBsonType() != BsonType.String || context.Reader.State == BsonReaderState.Type)
            {
                return null;
            }

            var input = context.Reader.ReadString();
            if (DateTime.TryParseExact(input, "yyyy-M-d H:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
            {
                return result;
            }

            return null;
        }
    }
}