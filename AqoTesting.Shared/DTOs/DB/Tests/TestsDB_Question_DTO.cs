using AqoTesting.Shared.Enums;
using MongoDB.Bson;

namespace AqoTesting.Shared.DTOs.DB.Tests
{
    public class TestsDB_Question_DTO
    {
        public int Id { get; set; }
        public QuestionTypes Type { get; set; }
        public string? Text { get; set; }
        public bool? Shuffle { get; set; }
        public BsonDocument Options { get; set; }
        public int? Cost { get; set; }
    }
}
