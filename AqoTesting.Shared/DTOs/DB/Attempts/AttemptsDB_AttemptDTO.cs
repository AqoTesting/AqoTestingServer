﻿using AqoTesting.Core.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AqoTesting.Shared.DTOs.DB.Attempts
{
    public class AttemptsDB_AttemptDTO
    {
        [BsonId]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId MemberId { get; set; }
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId UserId { get; set; }
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId RoomId { get; set; }
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId TestId { get; set; }
        public Dictionary<string, AttemptsDB_SectionDTO> Sections { get; set; }
        public string CurrentSectionId { get; set; }
        public string CurrentQuestionId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool Ignore { get; set; }
        public int TotalBlurTime { get; set; }
        public int MaxPoints { get; set; }
        public int CorrectPoints { get; set; }
        public int PenalPoints { get; set; }
        public float CorrectRatio { get; set; }
        public float PenalRatio { get; set; }
    }
}
