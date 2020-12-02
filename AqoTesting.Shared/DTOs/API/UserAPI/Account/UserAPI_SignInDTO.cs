using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Account
{
    public class UserAPI_SignInDTO
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
    }
}
