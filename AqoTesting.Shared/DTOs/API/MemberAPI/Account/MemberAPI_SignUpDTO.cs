using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.MemberAPI.Account
{
    public class MemberAPI_SignUpDTO
    {
        private string? _login;
        private string? _email;

        [Required]
        [StringLength(32, MinimumLength = 1)]
        [RegularExpression(@"^[A-z0-9_]+$")]
        public string? Login {
            get => _login;
            set => _login = value?.ToLower();
        }

        [Required]
        [StringLength(128, MinimumLength = 6)]
        public string? Password { get; set; }

        [Required]
        [StringLength(320, MinimumLength = 6)]
        [EmailAddress]
        public string? Email {
            get => _email;
            set => _email = value?.ToLower();
        }

        [Required]
        [StringLength(24, MinimumLength = 24)]
        [RegularExpression(@"^[0-9abcdef]+$")]
        public string? RoomId { get; set; }

        [Required]
        public Dictionary<string, string>? Fields { get; set; }
    }
}
