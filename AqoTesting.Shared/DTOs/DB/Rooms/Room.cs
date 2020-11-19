using AqoTesting.Core.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace AqoTesting.Shared.DTOs.DB.Users.Rooms
{
    public class Room
    {
        [BsonId]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Description { get; set; }

        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId OwnerId { get; set; }
        public bool IsDataRequired { get; set; } = false;
        public RoomField[] Fields { get; set; } = new RoomField[0]; //RoomWorker GetRoomFields
        public bool IsActive { get; set; }
        public bool IsApproveManually { get; set; }
        public bool IsRegistrationEnabled { get; set; }
    }
}
