using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace AqoTesting.Shared.DTOs.DB.Tests
{
    public class Test
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public ObjectId OwnerId { get; set; }
        public ObjectId RoomId { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; } //приоритет над датами (если true)
        public Section[] Sections { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ActivationDate { get; set; }
        public DateTime? DeactivationDate { get; set; }
        public bool Shuffle { get; set; }
    }
}
