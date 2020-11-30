namespace AqoTesting.Shared.DTOs.API.UserAPI.Attempts.Options
{
    public class UserAPI_AttemptCommonOptionDTO
    {
        public bool IsCorrect { get; set; }
        public int CorrectIndex { get; set; }
        public bool Chosen { get; set; }
        public string? Text { get; set; }
        public string? ImageUrl { get; set; }
        public string? LeftText { get; set; }
        public string? LeftImageUrl { get; set; }
        public string? RightText { get; set; }
        public string? RightImageUrl { get; set; }
    }
}
