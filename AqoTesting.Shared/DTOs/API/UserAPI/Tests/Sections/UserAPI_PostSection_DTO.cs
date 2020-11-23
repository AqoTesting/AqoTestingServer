using AqoTesting.Shared.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections
{
    public class UserAPI_PostSection_DTO
    {
        public bool Deleted { get; set; } = false;

        [StringLength(128, MinimumLength = 1)]
        public string? Title { get; set; }

        [RequiredIf("Deleted", false)]
        [MinLength(1)]
        [MaxLength(32)]
        public Dictionary<string, UserAPI_PostQuestion_DTO>? Questions { get; set; }

        public bool Shuffle { get; set; } = false;

        public int Weight { get; set; } = 0;
    }
}
