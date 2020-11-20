using AqoTesting.Core.Utils;
using AqoTesting.Shared.Enums;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace AqoTesting.Shared.DTOs.DB.Users.Rooms
{
    public class RoomsDB_Field_DTO
    {
        public string Name { get; set; }
        public FieldType Type { get; set; }
        public bool IsRequired { get; set; }

        [JsonConverter(typeof(BsonDocumentConverter))]
        public BsonDocument Data { get; set; }
        //[BsonExtraElements]
        //public Dictionary<string, string> Data { get; set; }
    }
}
