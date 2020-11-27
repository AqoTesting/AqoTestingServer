using System.Collections.Generic;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections
{
    public class UserAPI_GetTestSection_DTO
    {
        public string? Title { get; set; }
        public Dictionary<string, UserAPI_GetTestQuestion_DTO>? Questions { get; set; }
        public int? AttemptQuestionsNumber { get; set; }
        public bool? Shuffle { get; set; }
        public int? Weight { get; set; }
    }
}
