using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace GameStore.Domain.Mongo.Util
{
    public class BsonStringDeserializer : SerializerBase<string>
    {
        public override string Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            switch (context.Reader.GetCurrentBsonType())
            {
                case BsonType.String when context.Reader.State != BsonReaderState.Type:
                    return context.Reader.ReadString();
                case BsonType.Int32 when context.Reader.State != BsonReaderState.Type:
                    return context.Reader.ReadInt32().ToString();
            }

            return null;
        }
    }
}