using AqoTesting.Shared.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AqoTesting.Shared.DTOs.DB.Tests
{
    public class Question
    {
        public int Id { get; set; }
        //public int SectionId { get; set; }
        public QuestionTypes Type { get; set; }
        public string? Text { get; set; }
        public bool Shuffle { get; set; }
        public BsonDocument Options { get; set; }
        public int Score { get; set; }
        [BsonIgnoreIfNull]
        public int? BlurTime { get; set; }
        [BsonIgnoreIfNull]
        public int? TotalTime { get; set; }
    }
}
