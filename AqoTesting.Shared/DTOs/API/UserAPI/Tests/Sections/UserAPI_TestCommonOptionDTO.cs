﻿using AqoTesting.Shared.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections
{
    public class UserAPI_TestCommonOptionDTO
    {
        public bool IsCorrect { get; set; } = false;

        [StringLength(512, MinimumLength = 1)]
        public string? Text { get; set; }

        [StringLength(2048)]
        [Url]
        public string? ImageUrl { get; set; }

        [StringLength(512, MinimumLength = 1)]
        public string? RightText { get; set; }

        [StringLength(512, MinimumLength = 1)]
        public string? LeftText { get; set; }

        [StringLength(2048)]
        [Url]
        public string? RightImageUrl { get; set; }

        [StringLength(2048)]
        [Url]
        public string? LeftImageUrl { get; set; }
        public bool IsBlank { get; set; }

        [ArrayStringLength(512, MinimumLength = 1)]
        public string[]? CorrectTexts { get; set; }
    }
}
