using AqoTesting.Shared.Enums;
using System;

namespace AqoTesting.Shared.DTOs.API.MemberAPI.Tests
{
    public class MemberAPI_GetTestDTO
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? RoomId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public MemberAPI_TestDocumentDTO[]? Documents { get; set; }
        public int AttemptsNumber { get; set; }
        public int LastConsiderableAttemptsNumber { get; set; }
        public int TimeLimit { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ActivationDate { get; set; }
        public DateTime? DeactivationDate { get; set; }
        public MemberAPI_TestRankDTO[]? Ranks { get; set; }
        public FinalResultCalculationMethod FinalResultCalculationMethod { get; set; }
    }
}