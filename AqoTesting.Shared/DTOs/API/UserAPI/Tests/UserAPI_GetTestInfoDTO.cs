using AqoTesting.Shared.Enums;
using System;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Tests
{
    public class UserAPI_GetTestInfoDTO
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? RoomId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public UserAPI_TestDocumentDTO[]? Documents { get; set; }
        public int AttemptsNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ActivationDate { get; set; }
        public DateTime? DeactivationDate { get; set; }
        public bool Shuffle { get; set; }
        public UserAPI_TestRankDTO[]? Ranks { get; set; }
        public UserAPI_TestDocumentDTO[]? Documents { get; set; }
        public int AttemptsNumber { get; set; }
        public FinalResultCalculationMethod FinalResultCalculationMethod { get; set; }
    }
}
