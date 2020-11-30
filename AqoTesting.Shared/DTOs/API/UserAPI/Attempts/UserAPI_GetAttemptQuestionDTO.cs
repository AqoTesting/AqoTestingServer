using AqoTesting.Shared.DTOs.API.UserAPI.Attempts.Options;
using AqoTesting.Shared.Enums;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Attempts
{
    public class UserAPI_GetAttemptQuestionDTO
    {
        public QuestionTypes Type { get; set; }
        public string? Text { get; set; }
        public bool Shuffle { get; set; }
        public UserAPI_AttemptCommonOptionDTO[] Options { get; set; }
        public int Cost { get; set; }
        public int BlurTime { get; set; } = 0;
        public int TotalTime { get; set; } = 0;
    }
}
