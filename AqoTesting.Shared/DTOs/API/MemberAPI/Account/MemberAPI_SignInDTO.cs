using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.MemberAPI.Account
{
    public class MemberAPI_SignInDTO
    {
        private string? _login;

        [Required]
        [StringLength(320, MinimumLength = 1)]
        [RegularExpression(@"^[A-z0-9_@\.\-]+$")]
        public string? Login {
            get => _login;
            set => _login = value?.ToLower();
        }

        [Required]
        [StringLength(128, MinimumLength = 6)]
        public string? Password { get; set; }

        [Required]
        [StringLength(24, MinimumLength = 24)]
        [RegularExpression(@"^[0-9abcdef]+$")]
        public string? RoomId { get; set; }
    }
}
