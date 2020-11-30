using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.MemberAPI.Account
{
    public class MemberAPI_SignInDTO
    {
        [Required]
        [StringLength(320, MinimumLength = 1)]
        public string? Login { get; set; }

        [Required]
        [StringLength(128, MinimumLength = 6)]
        public string? Password { get; set; }

        [Required]
        [StringLength(24, MinimumLength = 24)]
        [RegularExpression(@"^[0-9abcdef]+$")]
        public string? RoomId { get; set; }
    }
}
