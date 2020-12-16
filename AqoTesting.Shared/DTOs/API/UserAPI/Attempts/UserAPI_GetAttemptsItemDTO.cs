using System;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Attempts
{
    public class UserAPI_GetAttemptsItemDTO
    {
        public string? Id { get; set; }
        public string? MemberId { get; set; }
        public string? UserId { get; set; }
        public string? RoomId { get; set; }
        public string? TestId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool Ignore { get; set; }
        public int TotalBlurTime { get; set; }
        public int MaxPoints { get; set; }
        public int CorrectPoints { get; set; }
        public int PenalPoints { get; set; }
        public float CorrectRatio { get; set; }
        public float PenalRatio { get; set; }
    }
}
