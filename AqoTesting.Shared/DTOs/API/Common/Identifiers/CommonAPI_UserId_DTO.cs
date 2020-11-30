﻿using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.Common.Identifiers
{
    public class CommonAPI_UserId_DTO
    {
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [RegularExpression(@"^[0-9abcdef]+$")]
        public string? UserId { get; set; }
    }
}