using AqoTesting.Shared.Enums;

namespace AqoTesting.Shared.DTOs.API.MemberAPI.Attempts
{
    public class MemberAPI_GetAttemptQuestionDTO
    {
        public QuestionTypes Type { get; set; }
        public string? Text { get; set; }
        public string? ImageUrl { get; set; }
        public MemberAPI_AttemptCommonOptionDTO[] Options { get; set; }
        public bool Touched { get; set; }
        public int Cost { get; set; }
        public int Weight { get; set; }
        public int BlurTime { get; set; }
        public int TotalTime { get; set; }
    }
}
