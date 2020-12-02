using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Account
{
    public class UserAPI_SignUpDTO
    {
        private string? _email;
        [Required]
        [StringLength(32, MinimumLength = 1)]
        [RegularExpression(@"^[A-z0-9_]+$")]
        public string? Login { get; set; }

        [Required]
        [StringLength(128, MinimumLength = 6)]
        public string? Password { get; set; }

        [Required]
        [StringLength(320, MinimumLength = 6)]
        [EmailAddress]
        public string? Email
        {
            get => _email;
            set => _email = value?.ToLower();
        }

        [StringLength(128, MinimumLength = 1)]
        public string? Name { get; set; }
    }
}
