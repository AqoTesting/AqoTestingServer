using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Tests
{
    public class UserAPI_TestRank_DTO
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int MinimumScore { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 1)]
        public string? Title { get; set; }

        [StringLength(6, MinimumLength = 6)]
        [RegularExpression(@"^[0-9abcdef]+$")]
        public string? Color { get; set; }
    }
}
