using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace AqoTesting.Shared.DTOs.DB.Attempts
{
    public class AttemptsDB_Attempt_DTO
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public ObjectId MemberId { get; set; }
        public ObjectId UserId { get; set; }
        public ObjectId TestId { get; set; }
        public Dictionary<string, AttemptsDB_Question_DTO> Questions { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCompleted { get; set; }
        public bool Ignore { get; set; }
        public int Score { get; set; }
    }
}
