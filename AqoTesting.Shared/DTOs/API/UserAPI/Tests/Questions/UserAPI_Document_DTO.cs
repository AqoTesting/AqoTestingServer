using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Tests.Questions
{
    public class UserAPI_Document_DTO
    {
        [Required]
        [StringLength(128, MinimumLength = 1)]
        public string? Title { get; set; }

        [StringLength(2048)]
        [Url]
        public string? Link { get; set; }
    }
}
