using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Account
{
    public class UserAPI_SignInDTO
    {
        [Required]
        [StringLength(320, MinimumLength = 1)]
        public string? Login { get; set; }

        [Required]
        [StringLength(128, MinimumLength = 6)]
        public string? Password { get; set; }
    }
}
