using AqoTesting.Shared.Enums;

namespace AqoTesting.Shared.DTOs.API.MemberAPI.Tests
{
    public class MemberAPI_TestRankDTO
    {
        public float MinimumSuccessRatio { get; set; }
        public string? Title { get; set; }
        public string? BackgroundColor { get; set; }
        public string? TextColor { get; set; }
        public FontStyles FontStyle { get; set; } = FontStyles.Normal;
    }
}
