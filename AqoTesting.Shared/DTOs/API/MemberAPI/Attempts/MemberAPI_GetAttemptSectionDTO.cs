using System.Collections.Generic;

namespace AqoTesting.Shared.DTOs.API.MemberAPI.Attempts
{
    public class MemberAPI_GetAttemptSectionDTO
    {
        public string Title { get; set; }
        public Dictionary<string, MemberAPI_GetAttemptQuestionDTO> Questions { get; set; }
        public int Weight { get; set; }
    }
}
