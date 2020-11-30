using AqoTesting.Shared.Enums;

namespace AqoTesting.Shared.DTOs.DB.Tests
{
    public class TestsDB_RankDTO
    {
        public float MinimumSuccessRatio { get; set; }
        public string Title { get; set; }
        public string? BackgroundColor { get; set; }
        public string? TextColor { get; set; }
        public FontStyles FontStyle { get; set; } = FontStyles.Normal;
    }
}
