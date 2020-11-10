using AqoTesting.Shared.Enums;
using MongoDB.Bson;

namespace AqoTesting.Shared.DTOs.DB.Users.Rooms
{
    public class RoomField
    {
        public string Name { get; set; }
        public FieldType Type { get; set; }
        public bool IsRequired { get; set; }
        public BsonDocument Data { get; set; }
        //[BsonExtraElements]
        //public Dictionary<string, string> Data { get; set; }
    }
}
