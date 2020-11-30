using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Account
{
    public class UserAPI_SignUpDTO
    {
        [Required]
        [StringLength(32, MinimumLength = 1)]
        public string? Login { get; set; }

        [Required]
        [StringLength(128, MinimumLength = 6)]
        public string? Password { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(320, MinimumLength = 6)]
        public string? Email { get; set; }

        [StringLength(64, MinimumLength = 1)]
        public string? Name { get; set; }
    }
}
