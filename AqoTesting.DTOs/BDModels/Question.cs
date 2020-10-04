using AqoTesting.DTOs.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.DTOs.BDModels
{
    public class Question
    {
        public int Id { get; set; }
        public int SectionId { get; set; }
        public QuestionTypeEnum Type { get; set; }
        public string? Text { get; set; }
        public bool Shuffle { get; set; }
        public string OptionsJson { get; set; }
    }
}
