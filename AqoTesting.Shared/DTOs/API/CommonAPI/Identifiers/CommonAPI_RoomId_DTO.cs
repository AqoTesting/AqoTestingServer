﻿using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers
{
    public class CommonAPI_RoomIdDTO
    {
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [RegularExpression(@"^[0-9abcdef]+$")]
        public string? RoomId { get; set; }
    }
}
