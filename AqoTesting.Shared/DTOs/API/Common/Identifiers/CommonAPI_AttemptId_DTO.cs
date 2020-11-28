using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.Common.Identifiers
{
    public class CommonAPI_AttemptId_DTO
    {
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [RegularExpression(@"^[0-9abcdef]+$")]
        public string? AttemptId { get; set; }
    }
}
