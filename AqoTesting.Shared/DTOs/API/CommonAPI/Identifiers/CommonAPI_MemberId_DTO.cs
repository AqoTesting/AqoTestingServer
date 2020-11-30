using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers
{
    public class CommonAPI_MemberIdDTO
    {
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [RegularExpression(@"^[0-9abcdef]+$")]
        public string? MemberId { get; set; }
    }
}
