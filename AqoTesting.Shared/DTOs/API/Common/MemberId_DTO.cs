using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.Common
{
    public class MemberId_DTO
    {
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [RegularExpression(@"^[0-9abcdef]+$")]
        public string? MemberId { get; set; }
    }
}
