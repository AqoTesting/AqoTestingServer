using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections.Options
{
    public class UserAPI_CommonOption_DTO
    {
        public bool? IsCorrect { get; set; }

        [StringLength(512, MinimumLength = 1)]
        public string? Text { get; set; }

        [StringLength(2048)]
        [Url]
        public string? ImageUrl { get; set; }

        [StringLength(512, MinimumLength = 1)]
        public string? RightText { get; set; }

        [StringLength(512, MinimumLength = 1)]
        public string? LeftText { get; set; }

        [StringLength(2048)]
        [Url]
        public string? RightImageUrl { get; set; }

        [StringLength(2048)]
        [Url]
        public string? LeftImageUrl { get; set; }
    }
}
