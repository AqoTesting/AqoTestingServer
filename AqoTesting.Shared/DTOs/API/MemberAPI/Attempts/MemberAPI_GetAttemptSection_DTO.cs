using System.Collections.Generic;

namespace AqoTesting.Shared.DTOs.API.MemberAPI.Attempts
{
    public class MemberAPI_GetAttemptSection_DTO
    {
        public string Title { get; set; }
        public Dictionary<string, MemberAPI_GetAttemptQuestion_DTO> Questions { get; set; }
        public int Weight { get; set; }
    }
}
