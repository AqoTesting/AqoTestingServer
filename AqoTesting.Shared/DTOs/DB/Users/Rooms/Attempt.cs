using MongoDB.Bson;

namespace AqoTesting.Shared.DTOs.DB.Users.Rooms
{
    public class Attempt
    {
        public ObjectId TestId { get; set; }
        public int SectionId { get; set; }
        public int QuestionId { get; set; }
        public object Answer { get; set; }
    }
}
