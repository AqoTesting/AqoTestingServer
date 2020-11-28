using System.Collections.Generic;

namespace AqoTesting.Shared.DTOs.DB.Attempts
{
    public class AttemptsDB_Section_DTO
    {
        public string Title { get; set; }
        public Dictionary<string, AttemptsDB_Question_DTO> Questions { get; set; }
        public int Weight { get; set; }
    }
}
