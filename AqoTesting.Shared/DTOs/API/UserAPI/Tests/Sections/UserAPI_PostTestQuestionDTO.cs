﻿using AqoTesting.Shared.Attributes;
using AqoTesting.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections
{
    public class UserAPI_PostTestQuestionDTO
    {
        public bool Deleted { get; set; } = false;

        [RequiredIf("Deleted", false)]
        public QuestionTypes Type { get; set; }

        [StringLength(1024, MinimumLength = 1)]
        public string? Text { get; set; }
        [StringLength(2048)]
        [Url]
        public string? ImageUrl { get; set; }
        public bool Shuffle { get; set; } = false;

        [RequiredIf("Deleted", false)]
        [MinLength(2)]
        [MaxLength(16)]
        public UserAPI_TestCommonOptionDTO[]? Options { get; set; } = new UserAPI_TestCommonOptionDTO[0];

        public int Cost { get; set; } = 1;

        public int Weight { get; set; } = 0;
    }
}
