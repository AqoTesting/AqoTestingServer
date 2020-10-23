using AqoTesting.Shared.Enums;

namespace AqoTesting.Shared.DTOs.DB.Tests
{
    public class Question
    {
        public int Id { get; set; }
        //public int SectionId { get; set; }
        public QuestionTypeEnum Type { get; set; }
        public string? Text { get; set; }
        public bool Shuffle { get; set; }
        public string OptionsJson { get; set; }
    }
}
