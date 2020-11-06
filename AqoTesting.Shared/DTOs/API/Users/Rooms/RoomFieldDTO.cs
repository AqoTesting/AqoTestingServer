﻿using System.ComponentModel.DataAnnotations;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces.DTO;

namespace AqoTesting.Shared.DTOs.API.Users.Rooms
{
    public class RoomFieldDTO
    {
        [Required]
        [StringLength(64, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        public FieldType Type { get; set; }

        [Required]
        public bool IsRequired { get; set; }

        [StringLength(64, MinimumLength = 1)]
        public string Placeholder { get; set; }

        [StringLength(5)]
        public string Mask { get; set; }

        [MinLength(2)]
        [MaxLength(32)]
        public string[]? Options { get; set; } = null;
    }
}