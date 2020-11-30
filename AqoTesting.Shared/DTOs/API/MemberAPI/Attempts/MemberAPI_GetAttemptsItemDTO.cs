using System;

namespace AqoTesting.Shared.DTOs.API.MemberAPI.Attempts
{
    public class MemberAPI_GetAttemptsItemDTO
    {
        public string? Id { get; set; }
        public string? MemberId { get; set; }
        public string? UserId { get; set; }
        public string? TestId { get; set; }
        public string CurrentSectionId { get; set; }
        public string CurrentQuestionId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool Ignore { get; set; }
        public int Score { get; set; }
    }
}
