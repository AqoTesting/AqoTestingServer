using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace AqoTesting.Shared.DTOs.DB.Members
{
    public class Member
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public ObjectId RoomId { get; set; }
        public string? Login { get; set; }
        public byte[]? PasswordHash { get; set; }
        public Attempt[]? Attempts { get; set; }
        public bool IsRegistered { get; set; }
        public bool IsChecked { get; set; }
        public Dictionary<string, string>? Fields { get; set; }
        public byte[]? FieldsHash { get; set; }
    }
}
