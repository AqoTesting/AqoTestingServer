using MongoDB.Bson;
using System;
using System.Linq;

namespace AqoTesting.Domain.Utils
{
    public static class DBUtils
    {
        public static Tuple<ObjectId, BsonDocument> PrepateItemToWrite(object item)
        {
            var bsonDocument = item.ToBsonDocument();
            return Tuple.Create((ObjectId)bsonDocument.ElementAt(0).Value, bsonDocument);
        }
    }
}