using AqoTesting.Shared.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections
{
    public class UserAPI_PostSection_DTO
    {
        [Required]
        public bool? Deleted { get; set; }

        [Required]
        public int Id { get; set; }

        [StringLength(128, MinimumLength = 1)]
        public string? Title { get; set; }

        [RequiredIf("Deleted", false)]
        [MinLength(1)]
        [MaxLength(32)]
        public Dictionary<int, UserAPI_PostQuestion_DTO>? Questions { get; set; }

        [RequiredIf("Deleted", false)]
        public bool? Shuffle { get; set; }

        [RequiredIf("Deleted", false)]
        public int? Weight { get; set; }
    }
}
