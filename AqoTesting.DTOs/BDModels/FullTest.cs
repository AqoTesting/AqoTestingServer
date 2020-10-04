using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.DTOs.BDModels
{
    public struct FullTest
    {
        /*
        public Test Test;
        public Section[] Sections;
        public Question[] Questions;
        public FullTest(Test test, Section[] sections, Question[] questions)
        {
            Test = test;
            Sections = sections;
            Questions = questions;
        }
        */
        public Test Test { get; set; }
        public Section[] Sections { get; set; }
        public Question[] Questions { get; set; }
    }
}
