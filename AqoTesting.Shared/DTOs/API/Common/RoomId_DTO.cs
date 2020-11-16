﻿using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.Common
{
    public class RoomId_DTO
    {
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [RegularExpression(@"^[0-9abcdef]+$")]
        public string? RoomId { get; set; }
    }
}