using AqoTesting.Shared.DTOs.API.UserAPI.Tests.Questions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Tests
{
    public class UserAPI_PostTest_DTO
    {
        [Required]
        [StringLength(128, MinimumLength = 1)]
        public string? Title { get; set; }

        [StringLength(4096, MinimumLength = 1)]
        public string? Description { get; set; }

        [MinLength(1)]
        [MaxLength(16)]
        public UserAPI_Document_DTO[]? Documents { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(128)]
        public UserAPI_Section_DTO[]? Sections { get; set; }

        public DateTime? ActivationDate { get; set; }
        public DateTime? DeactivationDate { get; set; }

        [Required]
        public bool Shuffle { get; set; }

        public Dictionary<string, int>? RatingScale { get; set; }
    }
}