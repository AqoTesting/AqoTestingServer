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
        public string Description { get; set; }
        public ObjectId[] Members { get; set; } = new ObjectId[0];
        public ObjectId[] TestIds { get; set; } = new ObjectId[0];
        public ObjectId OwnerId { get; set; }
        public bool IsDataRequired { get; set; } = false;
        public RoomField[] Fields { get; set; } = new RoomField[0]; //RoomWorker GetRoomFields
        public bool IsActive { get; set; }
        public bool IsApproveManually { get; set; }
        public bool IsRegistrationEnabled { get; set; }
    }
}
