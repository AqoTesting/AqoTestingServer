﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.Members
{
    public class SignUpMemberDTO
    {
        [Required]
        [StringLength(32, MinimumLength = 1)]
        public string Login { get; set; }

        [Required]
        [StringLength(128, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(320, MinimumLength = 6)]
        public string Email { get; set; }

        [Required]
        [StringLength(24, MinimumLength = 24)]
        [RegularExpression(@"^[0-9abcdef]+$")]
        public string RoomId { get; set; }

        [Required]
        //[StringLength(512)]
        public Dictionary<string, string> Fields { get; set; }
    }
}
