using System.Collections.Generic;

namespace AqoTesting.Shared.DTOs.DB.Attempts
{
    public class AttemptsDB_SectionDTO
    {
        public string Title { get; set; }
        public Dictionary<string, AttemptsDB_QuestionDTO> Questions { get; set; }
        public int Weight { get; set; }
    }
}
