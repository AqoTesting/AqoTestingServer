using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.Shared.DTOs.BD.Rooms
{
    public class Room
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public Member[] Members { get; set; }
        public ObjectId[] TestIds { get; set; }
        public ObjectId OwnerId { get; set; }
        public bool IsDataRequired { get; set; }
        public RequestedField[] RequestedFields { get; set; }
        public bool IsActive { get; set; }
    }
}
