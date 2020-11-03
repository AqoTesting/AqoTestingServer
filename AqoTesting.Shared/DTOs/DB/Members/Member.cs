using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AqoTesting.Shared.DTOs.DB.Members
{
    public class Member
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public ObjectId RoomId { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public Attempt[] Attempts { get; set; }
        public object UserData { get; set; }
    }
}
