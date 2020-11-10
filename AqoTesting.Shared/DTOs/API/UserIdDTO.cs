using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API
{
    public class UserIdDTO
    {
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [RegularExpression(@"^[0-9abcdef]+$")]
        public string UserId { get; set; }
    }
}
