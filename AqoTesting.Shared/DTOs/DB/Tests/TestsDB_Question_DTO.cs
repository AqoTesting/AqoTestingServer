using AqoTesting.Shared.Enums;
using MongoDB.Bson;

namespace AqoTesting.Shared.DTOs.DB.Tests
{
    public class TestsDB_Question_DTO
    {
        public QuestionTypes Type { get; set; }
        public string? Text { get; set; }
        public string? ImageUrl { get; set; }
        public bool? Shuffle { get; set; }
        public BsonDocument Options { get; set; }
        public int? Cost { get; set; } = 1;
        public int Weight { get; set; }
    }
}
