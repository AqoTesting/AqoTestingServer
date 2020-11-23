using AqoTesting.Core.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AqoTesting.Shared.DTOs.DB.Tests
{
    public class TestsDB_Test_DTO
    {
        [BsonId]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId OwnerId { get; set; }
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId RoomId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TestsDB_Document_DTO[] Documents {get; set; }
        public bool IsActive { get; set; } //приоритет над датами (если true)
        public Dictionary<string, TestsDB_Section_DTO> Sections { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ActivationDate { get; set; }
        public DateTime? DeactivationDate { get; set; }
        public bool Shuffle { get; set; }
        public Dictionary<string, int>? RatingScale { get; set; }
    }
}
