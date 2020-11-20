using AqoTesting.Shared.Enums;
using MongoDB.Bson;

namespace AqoTesting.Shared.DTOs.DB.Attempts
{
    public class AttemptsDB_Question_DTO
    {
        public int Id { get; set; }
        public QuestionTypes Type { get; set; }
        public string? Text { get; set; }
        public bool Shuffle { get; set; }
        public BsonDocument Options { get; set; }
        public int Cost { get; set; }
        public int BlurTime { get; set; } = 0;
        public int TotalTime { get; set; } = 0;
    }
}
