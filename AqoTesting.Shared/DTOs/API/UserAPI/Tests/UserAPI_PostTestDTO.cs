using AqoTesting.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Tests
{
    public class UserAPI_PostTestDTO
    {
        [Required]
        [StringLength(128, MinimumLength = 1)]
        public string? Title { get; set; }

        [StringLength(4096, MinimumLength = 1)]
        public string? Description { get; set; }

        [MaxLength(16)]
        public UserAPI_TestDocumentDTO[]? Documents { get; set; } = new UserAPI_TestDocumentDTO[0];

        [Range(0, int.MaxValue)]
        public int AttemptSectionsNumber { get; set; } = 0;

        [Range(1, int.MaxValue)]
        public int AttemptsNumber { get; set; } = 1;

        [Range(0, int.MaxValue)]
        public int TimeLimit { get; set; }

        public FinalResultCalculationMethod FinalResultCalculationMethod { get; set; } = FinalResultCalculationMethod.Best;

        [Required]
        public bool? IsActive { get; set; }

        public DateTime? ActivationDate { get; set; }
        public DateTime? DeactivationDate { get; set; }

        [Required]
        public bool? Shuffle { get; set; }

        public UserAPI_TestRankDTO[]? Ranks { get; set; } = new UserAPI_TestRankDTO[0];
    }
}