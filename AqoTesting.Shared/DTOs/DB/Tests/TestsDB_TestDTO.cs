using AqoTesting.Core.Utils;
using AqoTesting.Shared.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AqoTesting.Shared.DTOs.DB.Tests
{
    public class TestsDB_TestDTO
    {
        [BsonId]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId UserId { get; set; }
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId RoomId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TestsDB_DocumentDTO[] Documents {get; set; }
        public Dictionary<string, TestsDB_SectionDTO> Sections { get; set; }
        public int AttemptSectionsNumber { get; set; }
        public int AttemptsNumber { get; set; }
        public int LastConsiderableAttemptsNumber { get; set; }
        public int TimeLimit { get; set; }
        public FinalResultCalculationMethod FinalResultCalculationMethod { get; set; }
        public bool IsActive { get; set; } // приоритет над датами (если true)
        public DateTime CreationDate { get; set; }
        public DateTime? ActivationDate { get; set; }
        public DateTime? DeactivationDate { get; set; }
        public bool Shuffle { get; set; }
        public TestsDB_RankDTO[] Ranks { get; set; }
    }
}
