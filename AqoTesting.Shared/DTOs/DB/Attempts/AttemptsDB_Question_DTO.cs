using AqoTesting.Core.Utils;
using AqoTesting.Shared.Enums;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace AqoTesting.Shared.DTOs.DB.Attempts
{
    public class AttemptsDB_Question_DTO
    {
        public QuestionTypes Type { get; set; }
        public string? Text { get; set; }
        public string? ImageUrl { get; set; }
        [JsonConverter(typeof(BsonDocumentConverter))]
        public BsonDocument Options { get; set; }
        public bool Touched { get; set; }
        public int Cost { get; set; }
        public int Weight { get; set; }
        public int BlurTime { get; set; }
        public int TotalTime { get; set; }
    }
}
