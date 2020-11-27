using AqoTesting.Shared.DTOs.DB.Tests;
using System;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Attempts
{
    public class UserAPI_GetAttemptsItem_DTO
    {
        public string? Id { get; set; }
        public string? MemberId { get; set; }
        public string? UserId { get; set; }
        public string? TestId { get; set; }
        //public Dictionary<string, TestsDB_Question_DTO> Questions { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCompleted { get; set; }
        public bool Ignore { get; set; }
        public int Score { get; set; }
    }
}
