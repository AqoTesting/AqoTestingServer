using AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections;
using AqoTesting.Shared.Enums;
using System;
using System.Collections.Generic;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Tests
{
    public class UserAPI_GetTest_DTO
    {
        public string? Id { get; set; }
        public string? OwnerId { get; set; }
        public string? RoomId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public UserAPI_Document_DTO[]? Documents { get; set; }
        public Dictionary<string, UserAPI_GetSection_DTO>? Sections { get; set; }
        public int AttemptSectionsNumber { get; set; } = 0;
        public int AttemptsNumber { get; set; }
        public FinalResultCalculationMethod FinalResultCalculationMethod { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ActivationDate { get; set; }
        public DateTime? DeactivationDate { get; set; }
        public bool Shuffle { get; set; }
        public Dictionary<string, int>? RatingScale { get; set; }
    }
}