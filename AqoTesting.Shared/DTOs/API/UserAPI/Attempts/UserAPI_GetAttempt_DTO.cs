using System;
using System.Collections.Generic;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Attempts
{
    public class UserAPI_GetAttempt_DTO
    {
        public string? Id { get; set; }
        public string? MemberId { get; set; }
        public string? UserId { get; set; }
        public string? TestId { get; set; }
        public Dictionary<string, UserAPI_GetAttemptSection_DTO> Sections { get; set; }
        public string? CurrentSectionId { get; set; }
        public string? CurrentQuestionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool Ignore { get; set; }
        public int Score { get; set; }
    }
}
