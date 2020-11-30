using AqoTesting.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Tests
{
    public class UserAPI_TestRankDTO
    {
        [Required]
        [Range((float) 0.0, (float) 1.0)]
        public float MinimumSuccessRatio { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 1)]
        public string? Title { get; set; }

        [StringLength(6, MinimumLength = 6)]
        [RegularExpression(@"^[0-9abcdef]+$")]
        public string? BackgroundColor { get; set; }

        [StringLength(6, MinimumLength = 6)]
        [RegularExpression(@"^[0-9abcdef]+$")]
        public string? TextColor { get; set; }

        public FontStyles FontStyle { get; set; } = FontStyles.Normal;
    }
}
