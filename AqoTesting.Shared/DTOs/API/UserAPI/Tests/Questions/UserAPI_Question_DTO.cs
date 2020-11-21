using AqoTesting.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Tests.Questions
{
    public class UserAPI_Question_DTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public QuestionTypes Type { get; set; }

        [Required]
        [StringLength(1024, MinimumLength = 1)]
        public string? Text { get; set; }

        public bool? Shuffle { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(16)]
        public UserAPI_CommonOption_DTO[]? Options { get; set; }

        public int? Cost { get; set; }
    }
}
