﻿using System.Collections.Generic;

namespace AqoTesting.Shared.DTOs.DB.Tests
{
    public class TestsDB_Section_DTO
    {
        public string Title { get; set; }
        public Dictionary<string, TestsDB_Question_DTO> Questions { get; set; }
        public int AttemptQuestionsNumber { get; set; }
        public bool Shuffle { get; set; }
        public int Weight { get; set; }
    }
}