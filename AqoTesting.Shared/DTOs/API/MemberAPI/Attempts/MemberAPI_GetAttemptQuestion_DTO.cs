using AqoTesting.Shared.Enums;

namespace AqoTesting.Shared.DTOs.API.MemberAPI.Attempts
{
    public class MemberAPI_GetAttemptQuestion_DTO
    {
        public QuestionTypes Type { get; set; }
        public string? Text { get; set; }
        public string? ImageUrl { get; set; }
        public MemberAPI_AttemptCommonOption_DTO[] Options { get; set; }
        public int Cost { get; set; }
        public int Weight { get; set; }
    }
}
