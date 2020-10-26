using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.Users.Test
{
    public class TestIdDTO
    {
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [RegularExpression(@"^[0-9abcdef]+$")]
        public string Id { get; set; }
    }
}
