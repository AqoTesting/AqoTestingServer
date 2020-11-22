using AqoTesting.Shared.Attributes;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections.Options;
using AqoTesting.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections
{
    public class UserAPI_PostQuestion_DTO
    {
        [Required]
        public bool? Deleted { get; set; }

        [Required]
        public int Id { get; set; }

        [RequiredIf("Deleted", false)]
        public QuestionTypes Type { get; set; }

        [RequiredIf("Deleted", false)]
        [StringLength(1024, MinimumLength = 1)]
        public string? Text { get; set; }

        public bool? Shuffle { get; set; }

        [RequiredIf("Deleted", false)]
        [MinLength(2)]
        [MaxLength(16)]
        public UserAPI_CommonOption_DTO[]? Options { get; set; }

        public int? Cost { get; set; }

        [RequiredIf("Deleted", false)]
        public int? Weight { get; set; }
    }
}
