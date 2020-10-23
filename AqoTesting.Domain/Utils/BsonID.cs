using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AqoTesting.Domain.Utils
{
    public class BsonID
    {
        [BsonId]
        public ObjectId Id { get; set; }
    }
}