﻿using AqoTesting.Shared.DTOs.DB.Attempts.Options;

namespace AqoTesting.Shared.DTOs.DB.Attempts.OptionsData
{
    public class AttemptsDB_MatchingOptionsContainer
    {
        public AttemptsDB_PositionalOption[] LeftSequence { get; set; }
        public AttemptsDB_PositionalOption[] RightSequence { get; set; }
    }
}