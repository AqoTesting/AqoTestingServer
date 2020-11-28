using System.Collections.Generic;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Attempts
{
    public class UserAPI_GetAttemptSection_DTO
    {
        public string? Title { get; set; }
        public Dictionary<string, UserAPI_GetAttemptQuestion_DTO>? Questions { get; set; }
        public int Weight { get; set; }
    }
}
