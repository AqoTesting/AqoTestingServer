using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AqoTesting.Shared.DTOs.DB.Users.Rooms
{
    public class Room
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public Member[] Members { get; set; } = new Member[0];
        public ObjectId[] TestIds { get; set; } = new ObjectId[0];
        public ObjectId OwnerId { get; set; }
        public bool IsDataRequired { get; set; } = false;
        public RequestedField[] RequestedFields { get; set; } = new RequestedField[0];
        public bool IsActive { get; set; } = false;
    }
}
