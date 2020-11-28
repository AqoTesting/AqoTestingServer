using AqoTesting.Core.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AqoTesting.Shared.DTOs.DB.Attempts
{
    public class AttemptsDB_Attempt_DTO
    {
        [BsonId]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId MemberId { get; set; }
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId UserId { get; set; }
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId TestId { get; set; }
        public Dictionary<string, AttemptsDB_Section_DTO> Sections { get; set; }
        public string CurrentSectionId { get; set; }
        public string CurrentQuestionId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool Ignore { get; set; }
        public int Score { get; set; }
    }
}
