using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.DTOs.BDModels
{
    public struct FullTest
    {
        public Test Test { get; set; }
        public Section[] Sections { get; set; }
        public Question[] Questions { get; set; }
    }

    public struct FullTestWithFullSections
    {
        public Test Test { get; set; }
        public SectionWithQuestions[] Sections { get; set; }
    }
}
