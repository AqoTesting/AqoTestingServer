﻿using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.MemberAPI.Attempts
{
    public class MemberAPI_CommonTestAnswer_DTO
    {
        public int? SelectedOption { get; set; }
        public int[]? SelectedOptions { get; set; }
        public int[]? Sequence { get; set; }
        public int[]? LeftSequence { get; set; }
        public int[]? RightSequence { get; set; }
        
        [Required]
        [Range(0, int.MaxValue)]
        public int BlurTimeAddition { get; set; }
        
        [Required]
        [Range(0, int.MaxValue)]
        public int TotalTimeAddition { get; set; }
    }
}
