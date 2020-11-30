using AqoTesting.Shared.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections
{
    public class UserAPI_PostTestSectionDTO
    {
        public bool Deleted { get; set; } = false;

        [StringLength(128, MinimumLength = 1)]
        public string? Title { get; set; }

        [RequiredIf("Deleted", false)]
        [MaxLength(32)]
        public Dictionary<string, UserAPI_PostTestQuestionDTO>? Questions { get; set; } = new Dictionary<string, UserAPI_PostTestQuestionDTO>();

        [Range(0, int.MaxValue)]
        public int AttemptQuestionsNumber { get; set; } = 0;

        public bool Shuffle { get; set; } = false;

        public int Weight { get; set; } = 0;
    }
}
