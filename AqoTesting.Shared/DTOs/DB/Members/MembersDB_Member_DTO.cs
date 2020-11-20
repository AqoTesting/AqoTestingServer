using AqoTesting.Core.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AqoTesting.Shared.DTOs.DB.Members
{
    public class MembersDB_Member_DTO
    {
        [BsonId]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId OwnerId { get; set; }
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId RoomId { get; set; }
        public string? Login { get; set; }
        public byte[]? PasswordHash { get; set; }
        public string? Email { get; set; }
        public bool IsRegistered { get; set; }
        public bool IsApproved { get; set; }
        public Dictionary<string, string>? Fields { get; set; }
        public byte[]? FieldsHash { get; set; }
    }
}
