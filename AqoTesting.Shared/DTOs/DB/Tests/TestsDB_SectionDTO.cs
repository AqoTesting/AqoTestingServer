using System.Collections.Generic;

namespace AqoTesting.Shared.DTOs.DB.Tests
{
    public class TestsDB_SectionDTO
    {
        public string Title { get; set; }
        public Dictionary<string, TestsDB_QuestionDTO> Questions { get; set; }
        public int AttemptQuestionsNumber { get; set; }
        public bool Shuffle { get; set; }
        public int Weight { get; set; }
    }
}
