using System.Collections.Generic;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Attempts
{
    public class UserAPI_GetAttemptSectionDTO
    {
        public string? Title { get; set; }
        public Dictionary<string, UserAPI_GetAttemptQuestionDTO>? Questions { get; set; }
        public int Weight { get; set; }
    }
}
