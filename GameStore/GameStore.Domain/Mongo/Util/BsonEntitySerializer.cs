using System;
using System.Collections.Generic;
using GameStore.Domain.Entities;
using MongoDB.Bson.Serialization;

namespace GameStore.Domain.Mongo.Util
{
    public class BsonEntitySerializer<TEntity> : IBsonSerializer where TEntity : EntityBase
    {
        public Type ValueType => typeof(ICollection<TEntity>);

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            if (value == null)
            {
                return;
            }

            var entityList = (ICollection<TEntity>)value;

            context.Writer.WriteStartArray();
            foreach (var entity in entityList)
            {
                context.Writer.WriteString(entity.Id.ToString());
            }

            context.Writer.WriteEndArray();
        }

        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            throw new NotImplementedException();
        }
    }
}