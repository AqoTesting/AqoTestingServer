using AqoTesting.Shared.DTOs.DB.Tests;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace AqoTesting.Shared.DTOs.DB.Users.Rooms
{
    public class Attempt
    {
        [BsonId]
        public ObjectId AttemptId { get; set; }
        public ObjectId TestId { get; set; }
        public ObjectId MemberId { get; set; }
        public int Seed { get; set; }
        public Section[] Sections { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCompleted { get; set; }
        public int Score { get; set; }
    }
}
