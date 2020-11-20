using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Tests.Questions
{
    public struct UserAPI_Section_DTO
    {
        [Required]
        public int Id { get; set; }

        [StringLength(128, MinimumLength = 1)]
        public string? Title { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(32)]
        public UserAPI_Question_DTO[] Questions { get; set; }

        [Required]
        public bool Shuffle { get; set; }
    }
}
