using AqoTesting.Shared.DTOs.DB.Tests;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace AqoTesting.Shared.DTOs.DB.Attempts
{
    public class AttemptsDB_Attempt_DTO
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public ObjectId TestId { get; set; }
        public ObjectId MemberId { get; set; }
        public TestsDB_Question_DTO[] Questions { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCompleted { get; set; }
        public int Score { get; set; }
    }
}
